using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BackendMusica.Services
{
    public class Security
    {
        public static String GenerateNewToken(Guid ID_App)
        {
            String Token = String.Empty;
            if (ID_App == Guid.Empty)
                Token = "Aceeso denegado para la aplicación";
            else
                Token = API.GenerateNewToken(DateTime.Now, ID_App);

            return Token;
        }

        #region Cryptography
        public static string Encrypt(string clearText, string key = "abc123")
        {
            string encryptionKey = key;
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string key = "abc123")
        {
            string encryptionKey = key;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        #endregion

        #region Cryptography Params

        public static string EncryptParams(string plainText, string keyString = "abc123abc123abc123abc123abc12312", string ivString = "1234567890123456")
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] iv = Encoding.UTF8.GetBytes(ivString);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        byte[] cipherTextBytes = memoryStream.ToArray();
                        return Convert.ToBase64String(cipherTextBytes);
                    }
                }
            }
        }

        public static string DecryptParams(string cipherText, string key = "abc123abc123abc123abc123abc12312", string ivString = "1234567890123456")
        {
            try
            {
                // Decodificar la cadena de URL a Base64
                string decodedCipherText = System.Net.WebUtility.UrlDecode(cipherText);

                if (!IsBase64String(decodedCipherText))
                {
                    throw new FormatException("La cadena no es una cadena Base64 válida.");
                }

                decodedCipherText = decodedCipherText.Trim().Replace(" ", "+");

                byte[] cipherBytes = Convert.FromBase64String(decodedCipherText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(ivString);

                using (Aes encryptor = Aes.Create())
                {
                    encryptor.Key = keyBytes;
                    encryptor.IV = ivBytes;
                    encryptor.Mode = CipherMode.CBC;
                    encryptor.Padding = PaddingMode.PKCS7;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                        }
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (CryptographicException ex)
            {
                throw new Exception("Error al descifrar los datos: " + ex.Message);
            }
            catch (FormatException ex)
            {
                throw new Exception("Formato inválido para Base64: " + ex.Message);
            }
        }


        public static bool IsBase64String(string base64)
        {
            base64 = base64.Trim().Replace(" ", "+");

            // Verificar que la longitud sea múltiplo de 4
            if (base64.Length % 4 != 0)
            {
                return false;
            }

            // Verificar si los caracteres son válidos para Base64
            string base64Pattern = @"^[a-zA-Z0-9\+/]*={0,2}$";
            return Regex.IsMatch(base64, base64Pattern, RegexOptions.None);
        }

        #endregion
    }
}