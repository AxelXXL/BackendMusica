using BackendMusica.Models;
using BackendMusica.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BackendMusica.Controllers
{
    public class LoginController : ApiController

    {
        #region Configuration

        private LoginServices _loginServices;

        public LoginController()
        {
            _loginServices = new LoginServices();
        }
        #endregion

        [Auth]
        [System.Web.Http.Route("api/Register")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Register(RegisterModel user)
        {
            return _loginServices.Register(user);
        }

        [Auth]
        [System.Web.Http.Route("api/Login")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Login(LoginRequestModel user)
        {
            return _loginServices.Login(user);
        }
    }
}