using BackendMusica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;

namespace BackendMusica.Services
{
    public class UserServices : BaseServices
    {
        public HttpResponseMessage GetUsers(int? IdUser)
        {
            try
            {
                IEnumerable<object> users;

                if (IdUser != null)
                {
                    users = db.tb_Usuario.Where(x => x.ID_USUARIO == IdUser).Select(x => new
                    {
                        x.ID_USUARIO,
                        x.Nombre_Usuario,
                        x.tb_RolesPrivacidad.ID_ROL,
                        x.tb_RolesPrivacidad.Rol,
                        x.Active
                    }).ToList();
                }
                else
                {
                    users = db.tb_Usuario.Select(x => new
                    {
                        x.ID_USUARIO,
                        x.Nombre_Usuario,
                        x.tb_RolesPrivacidad.ID_ROL,
                        x.tb_RolesPrivacidad.Rol,
                        x.Active
                    }).ToList();
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<List<object>>(users.Cast<object>().ToList(), new JsonMediaTypeFormatter())
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

        public HttpResponseMessage EditUser(int ID_User, EditRequestUser EditUser)
        {
            try
            {
                var userExist = db.tb_Usuario.Where(x => x.ID_USUARIO == ID_User).FirstOrDefault();

                if(userExist == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("El permiso asignado no existe.")
                    };
                }

                if (userExist.Nombre_Usuario != EditUser.Nombre_Usuario 
                    || userExist.ID_Rol != EditUser.ID_Rol
                    || userExist.Active != EditUser.Active)
                {
                    db.tb_Usuario.Attach(userExist);

                    userExist.Nombre_Usuario = EditUser.Nombre_Usuario;
                    userExist.ID_Rol = EditUser.ID_Rol;
                    userExist.Active = EditUser.Active;

                    db.Entry(userExist).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent($"El usuario {EditUser.Nombre_Usuario} se edito correctamente.")
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No se encontraron cambios.")
                    };
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }

        public HttpResponseMessage DeleteUser(int ID_User)
        {
            try
            {
                var user = db.tb_Usuario.Where(x => x.ID_USUARIO == ID_User).FirstOrDefault();

                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No se encontro al usuario.")
                    };
                }
                else
                {
                    db.tb_Usuario.Remove(user);
                    db.SaveChanges();

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("El usuario se elimino correctamente")
                    };
                }


            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Ocurrió un error inesperado. Más información: {ex.Message}")
                };
            }
        }


    }
}