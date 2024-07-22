using BackendMusica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net.Http.Formatting;
using System.IO;

namespace BackendMusica.Services
{
    public class ArtistsService : BaseServices
    {

        //[HttpPost]
        //public HttpResponseMessage UploadImageArtist(int ID_Artist)
        //{
        //    try
        //    {
        //        if (Request.Files.Count > 0)
        //        {
        //            var artist = db.Tb_Artista.FirstOrDefault(x => x.ID_Artista == ID_Artist);

        //            if (artist == null)
        //            {
        //                logger.Warn($"No se encontró al artista con el ID: {ID_Artist}");

        //                return new HttpResponseMessage(HttpStatusCode.NoContent)
        //                {
        //                    Content = new StringContent("No se encontró el artista solicitado.")
        //                };
        //            }

        //            foreach (string fileName in Request.Files)
        //            {
        //                HttpPostedFileBase file = Request.Files[fileName];

        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    file.InputStream.CopyTo(ms);
        //                    byte[] imageBytes = ms.ToArray();

        //                    artist.Imagen_Artista = imageBytes;
        //                    db.SaveChanges();
        //                }
        //            }

        //            return new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new StringContent($"La imagen se guardó correctamente para el artista {artist.Nombre_Artista}.")
        //            };
        //        }
        //        else
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.NoContent)
        //            {
        //                Content = new StringContent("No se seleccionó ningún archivo.")
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        return new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //        {
        //            Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
        //        };
        //    }
        //}

    }
}