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
        public HttpResponseMessage GetUsers(string ID_User)
        {
            return _userServices.GetUsers(ID_User);
        }

        [Auth]
        [System.Web.Http.Route("api/GetRoles")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRoles()
        {
            return _userServices.GetRoles();
        }

        [Auth]
        [System.Web.Http.Route("api/CreateUser")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CreateUser([FromBody] string newUser)
        {
            return _userServices.CreateUser(newUser);
        }


        [Auth]
        [System.Web.Http.Route("api/EditUser")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage EditUser(string ID_User,[FromBody] EditRequestUser user)
        {
            return _userServices.EditUser(ID_User, user);
        }
        

        [Auth]
        [System.Web.Http.Route("api/DeleteUser")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteUser([FromBody] string ID_User)
        {
            return _userServices.DeleteUser(ID_User);
        }
    }
}