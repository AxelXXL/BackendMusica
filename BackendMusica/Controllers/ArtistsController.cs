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
    public class ArtistsController : ApiController
    {
        #region Configuration

        private ArtistsService _artistsServices;

        public ArtistsController()
        {
            _artistsServices = new ArtistsService();
        }
        #endregion

        //[Auth]
        //[System.Web.Http.Route("api/UploadImageArtist")]
        //[System.Web.Http.HttpPost]
        //public HttpResponseMessage UploadImageArtist(int ID_Artist)
        //{
        //    return _artistsServices.UploadImageArtist(ID_Artist);
        //}
    }
}