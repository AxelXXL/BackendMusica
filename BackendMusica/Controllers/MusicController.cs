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
    public class MusicController : ApiController
    {
        #region Configurations
        private MusicServices _musicServices;

        public MusicController()
        {
            _musicServices = new MusicServices();
        }
        #endregion

        #region GET

        [Auth]
        [System.Web.Http.Route("api/Canciones")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetCanciones(int? ID_Cancion)
        {
            return _musicServices.GetCanciones(ID_Cancion);
        }

        [Auth]
        [System.Web.Http.Route("api/GetSongsPerArtist")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetSongsPerArtist(int ID_Artist)
        {
            return _musicServices.GetSongsPerArtist(ID_Artist);
        }
        
        [Auth]
        [System.Web.Http.Route("api/GetSongsPerAlbum")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetSongsPerAlbum(int ID_Album)
        {
            return _musicServices.GetSongsPerAlbum(ID_Album);
        }

        #endregion

        #region POST
        [Auth]
        [System.Web.Http.Route("api/SaveSong")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveMetadataSong(string archivo)
        {
            return _musicServices.SaveMetadataSong(archivo);
        }
        #endregion
    }
}