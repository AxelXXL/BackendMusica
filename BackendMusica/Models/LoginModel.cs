using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendMusica.Models
{
    public class LoginModel
    {
    }

    public class RegisterModel
    {
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string ConfirmPassword { get; set; }
        public int ID_Rol { get; set; }
        public string Email {  get; set; }
    }

    public class LoginRequestModel
    {
        public string Nombre_Usuario { get; set; }
        public string Contrasena { get; set; }
    }

    public class LoginResponseModel
    {
        public string Nombre_Usuario { get; set; }
        public int ID_Rol { get; set; }
    }

    public class EditRequestUser
    {
        public string Nombre_Usuario { get; set; }
        public int ID_Rol { get; set; }
        public bool Active { get; set; }
    }
}