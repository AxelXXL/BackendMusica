using BackendMusica.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BackendMusica.Services
{
    public class API
    {
        public static string GenerateNewToken(DateTime RecordDate, Guid ID_App)
        {
            string idAppString = ConfigurationManager.AppSettings["ID_App"];
            Guid idAppGuid;
            Guid.TryParse(idAppString, out idAppGuid);

            if (ID_App == Guid.Empty || ID_App != idAppGuid)
                return "El AppID es invalido";

            if (RecordDate == new DateTime())
                return "El RecordDate no es valor valido";

            TokenRequestModel model = new TokenRequestModel()
            {
                RecordDate = RecordDate,
                AppID = ID_App
            };

            return String.Format("bearer {0}", Security.Encrypt(JsonConvert.SerializeObject(model)));
        }

        public static bool isValidToken(String Token, Int32 Minutes = 360)
        {
            bool isValid = true;
            if (!Token.Contains("bearer "))
                return false;

            String JsonModel = Security.Decrypt(Token.Replace("bearer ", ""));
            TokenRequestModel model = JsonConvert.DeserializeObject<TokenRequestModel>(JsonModel);

            if (DateTime.Now >= model.RecordDate.AddMinutes(Minutes))
                return false;

            return isValid;
        }
    }
}