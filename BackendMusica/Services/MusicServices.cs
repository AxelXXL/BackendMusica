using BackendMusica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using TagLib;
using System.Data.Entity;

namespace BackendMusica.Services
{
    public class MusicServices : BaseServices
    {

        #region GET
        [HttpGet]
        public HttpResponseMessage GetCanciones(int? ID_Cancion)
        {
            try
            {

                IEnumerable<CancionData> canciones;
                if (ID_Cancion != null)
                {
                    canciones = db.Tb_Cancion
                        .Where(x => x.ID_Cancion == ID_Cancion && x.Activo)
                        .Include(b => b.Tb_CancionContents)
                        .Select(x => new CancionData
                        {
                            ID_Cancion = x.ID_Cancion,
                            Nombre_Cancion = x.Nombre_Cancion,
                            Numero_Cancion = x.Numero_Cancion,
                            Duracion_Cancion = x.Duracion_Cancion,
                            ID_Artista = x.ID_Artista,
                            Nombre_Artista = x.Tb_Artista.Nombre_Artista,
                            ID_Album = x.ID_Album,
                            Nombre_Album = x.Tb_Album.Nombre_Album,
                            Ruta_Audio = x.Ruta_Audio,
                            Caratula_Cancion = x.Caratula_Cancion,
                        }).ToList();

                    //logger.Info($"Se consulto metodo de canciones por parametro con el ID {ID_Cancion}");
                }
                else
                {
                    canciones = db.Tb_Cancion.Where(x => x.Activo).Select(x => new CancionData
                    {
                        ID_Cancion = x.ID_Cancion,
                        Nombre_Cancion = x.Nombre_Cancion,
                        Numero_Cancion = x.Numero_Cancion,
                        Duracion_Cancion = x.Duracion_Cancion,
                        ID_Artista = x.ID_Artista,
                        Nombre_Artista = x.Tb_Artista.Nombre_Artista,
                        ID_Album = x.ID_Album,
                        Nombre_Album = x.Tb_Album.Nombre_Album,
                        Ruta_Audio = x.Ruta_Audio,
                        Caratula_Cancion = x.Caratula_Cancion,
                    }).ToList();
                }

                if (canciones.Count() == 0)
                {
                    logger.Warn("No se encontro ninguna cancion con el ID " + ID_Cancion);

                    return new HttpResponseMessage(HttpStatusCode.NoContent)
                    {
                        Content = new StringContent("No se encontro la canción solicitada.")
                    };
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<List<CancionData>>(canciones.Cast<CancionData>().ToList(), new JsonMediaTypeFormatter())
                };
            }
            catch (Exception ex)
            {
                logger.Error($"Ocurrió un error inesperado. Más información: {ex}");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSongData(int ID_Cancion)
        {
            try
            {
                if (ID_Cancion == 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent($"No se selecciono ninguna pista.")
                    };
                }

                //Recuperar File_Content por separado
                byte[] contenido = db.Tb_CancionContents
                        .Where(cc => cc.ID_Cancion == ID_Cancion)
                        .Select(cc => cc.File_Content)
                        .AsNoTracking()
                        .FirstOrDefault();


                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<byte[]>(contenido, new JsonMediaTypeFormatter())
                };

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Intenta más tarde.")
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSongsPerArtist(int ID_Artist)
        {
            try
            {
                IEnumerable<object> songs;

                songs = db.Tb_Cancion.Where(x => x.ID_Artista == ID_Artist).Select(x => new
                {
                    x.ID_Cancion,
                    x.Nombre_Cancion,
                    x.Numero_Cancion,
                    x.Duracion_Cancion,
                    x.ID_Artista,
                    x.ID_Album,
                    x.Ruta_Audio,
                    x.Caratula_Cancion
                }).ToList();

                if (songs == null || songs.Count() <= 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent)
                    {
                        Content = new StringContent("No se encontro ninguna cancion con el artista indicado.")
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<List<object>>(songs.Cast<object>().ToList(), new JsonMediaTypeFormatter())
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage GetNovedades(int Take)
        {
            try
            {
                var songs = db.Tb_Cancion.Take(Take).
                    Select(x => new
                    {
                        x.ID_Cancion,
                        x.Nombre_Cancion,
                        x.Tb_Artista.Nombre_Artista,
                        x.Tb_Album.Nombre_Album,
                        x.Caratula_Cancion,
                        x.Ruta_Audio
                    }).ToList();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<List<object>>(songs.Cast<object>().ToList(), new JsonMediaTypeFormatter())
                };

            }
            catch (Exception ex)
            {
                logger.Error($"Ocurrió un error inesperado. Más información: {ex}");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage SearchSongs(string txt)
        {
            try
            {
                var items = db.Tb_Cancion
                .Where(x => x.Nombre_Cancion.Contains(txt)
                || x.Tb_Album.Nombre_Album.Contains(txt)
                || x.Tb_Album.Genero.Contains(txt)
                || x.Tb_Artista.Nombre_Artista.Contains(txt))
                .Select(x => new
                {
                    x.ID_Cancion,
                    x.Nombre_Cancion,
                    x.Tb_Artista.Nombre_Artista,
                    x.Tb_Album.Nombre_Album
                })
                .ToList();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<List<object>>(items.Cast<object>().ToList(), new JsonMediaTypeFormatter())
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSongsPerAlbum(int ID_Album)
        {
            try
            {
                IEnumerable<object> songs;

                songs = db.Tb_Cancion.Where(x => x.ID_Album == ID_Album).Select(x => new
                {
                    x.ID_Cancion,
                    x.Nombre_Cancion,
                    x.Numero_Cancion,
                    x.Duracion_Cancion,
                    x.ID_Artista,
                    x.ID_Album,
                    x.Ruta_Audio,
                    x.Caratula_Cancion
                }).ToList();

                if (songs == null || songs.Count() <= 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent)
                    {
                        Content = new StringContent("No se encontro ninguna cancion con el album indicado.")
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<List<object>>(songs.Cast<object>().ToList(), new JsonMediaTypeFormatter())
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAlbumsInfo()
        {
            try
            {
                List<AlbumInfo> albums = db.Tb_Album.
                    Select(x => new AlbumInfo()
                    {
                        ID_Album = x.ID_Album,
                        ID_Artista = x.ID_Artista,
                        Nombre_Album = x.Nombre_Album,
                        Nombre_Artista = x.Tb_Artista.Nombre_Artista,
                        Genero = x.Genero,
                        Año_Album = x.Año_Album,
                        Caratula_Album = x.Caratula_Album,
                    }).ToList();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<List<AlbumInfo>>(albums.Cast<AlbumInfo>().ToList(), new JsonMediaTypeFormatter())
                };
            }
            catch(Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado.")
                };
            }
        }

        #endregion

        #region POST

        [HttpPost]
        public HttpResponseMessage SaveMetadataSong(string archivo)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    if (archivo != null && archivo.Length > 0)
                    {
                        using (File archivoAudio = TagLib.File.Create(new File.LocalFileAbstraction(archivo)))
                        {
                            var audioData = archivoAudio.Properties.MediaTypes;
                            string[] artistas = archivoAudio.Tag.Performers;
                            string nomArtista = artistas[0];
                            string nomAlbum = archivoAudio.Tag.Album;
                            string genero = archivoAudio.Tag.FirstGenre;
                            int añoAlbum = (int)archivoAudio.Tag.Year;
                            int numeroCancion = (int)archivoAudio.Tag.Track;
                            int duracionSegundos = (int)archivoAudio.Properties.Duration.TotalSeconds;
                            string tituloCancion = archivoAudio.Tag.Title;

                            // Imagen
                            var caratula = archivoAudio.Tag.Pictures[0];
                            string base64Image = Convert.ToBase64String(caratula.Data.Data);
                            byte[] base64ImageBytes = Convert.FromBase64String(base64Image);

                            // Leer el contenido del archivo de audio
                            byte[] contenidoAudio = System.IO.File.ReadAllBytes(archivo);

                            // Buscar si el artista ya existe en la bd
                            var artistaID = db.Tb_Artista.Where(x => x.Nombre_Artista == nomArtista).Select(x => x.ID_Artista).FirstOrDefault();

                            // Buscar si el álbum ya existe en la bd
                            var albumID = db.Tb_Album.Where(x => x.Nombre_Album == nomAlbum).Select(x => x.ID_Album).FirstOrDefault();

                            if (artistaID == 0)
                            {
                                // El artista no existe en la base de datos, crea un nuevo registro
                                Tb_Artista artista = new Tb_Artista
                                {
                                    Nombre_Artista = nomArtista,
                                };
                                db.Tb_Artista.Add(artista);
                                db.SaveChanges(); // Guardar el artista para obtener su ID
                                artistaID = artista.ID_Artista; // Obtener el ID del artista recién creado
                            }

                            if (albumID == 0)
                            {
                                // El álbum no existe en la base de datos, crea un nuevo registro
                                Tb_Album album = new Tb_Album
                                {
                                    Nombre_Album = nomAlbum,
                                    Genero = genero,
                                    Año_Album = añoAlbum,
                                    ID_Artista = artistaID, // Asignar el ID del artista
                                    Caratula_Album = base64ImageBytes,
                                    Activo = true,
                                };
                                db.Tb_Album.Add(album);
                                db.SaveChanges(); // Guardar el álbum para obtener su ID
                                albumID = album.ID_Album; // Obtener el ID del álbum recién creado
                            }

                            Tb_Cancion cancion = new Tb_Cancion
                            {
                                Nombre_Cancion = tituloCancion,
                                Duracion_Cancion = duracionSegundos,
                                Numero_Cancion = numeroCancion,
                                ID_Artista = artistaID,
                                ID_Album = albumID,
                                Ruta_Audio = archivo,
                                Caratula_Cancion = base64ImageBytes,
                                Activo = true,
                            };
                            db.Tb_Cancion.Add(cancion);
                            db.SaveChanges();

                            // Guardar el contenido del archivo de audio
                            Tb_CancionContents cancionContents = new Tb_CancionContents
                            {
                                ID_Cancion = cancion.ID_Cancion,
                                File_Content = contenidoAudio
                            };
                            db.Tb_CancionContents.Add(cancionContents);
                            db.SaveChanges();

                            tran.Commit();
                            return new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent("Archivo guardado exitosamente.")
                            };
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.NoContent)
                        {
                            Content = new StringContent("No se encontró el archivo.")
                        };
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Ocurrió un error inesperado. Más información: " + ex.Message)
                    };
                }
            }
        }


        #endregion

        #region Delete

        public HttpResponseMessage DeleteSong(int ID_Song)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var contentSong = db.Tb_CancionContents.Where(x => x.ID_Cancion == ID_Song).First();
                    var metadataSong = db.Tb_Cancion.Find(ID_Song);

                    db.Tb_CancionContents.Remove(contentSong);
                    db.Tb_Cancion.Remove(metadataSong);

                    db.SaveChanges();

                    tran.Commit();
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("Cancion eliminada exitosamente.")
                    };

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Ocurrió un error inesperado. Más información: " + ex.Message)
                    };
                }
            }
        }

        #endregion

    }
}