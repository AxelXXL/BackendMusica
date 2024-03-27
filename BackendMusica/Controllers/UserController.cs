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
    public class UserController : ApiController
    {
        #region Configurations
        private UserServices _userServices;

        public UserController()
        {
            _userServices = new UserServices();
        }
        #endregion

        [Auth]
        [System.Web.Http.Route("api/GetUsuarios")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetUsers(int? ID_User)
        {
            return _userServices.GetUsers(ID_User);
        }
        
        
        [Auth]
        [System.Web.Http.Route("api/EditUser")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage EditUser(int ID_User,[FromBody] EditRequestUser user)
        {
            return _userServices.EditUser(ID_User, user);
        }
        

        [Auth]
        [System.Web.Http.Route("api/DeleteUser")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteUser(int ID_User)
        {
            return _userServices.DeleteUser(ID_User);
        }
    }
}