using BackendMusica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace BackendMusica.Services
{
    public class UserServices : BaseServices
    {

        #region Get
        public HttpResponseMessage GetUsers(string IdUser = null)
        {
            try
            {
                int idUser;
                if (!string.IsNullOrEmpty(IdUser))
                {
                    string decryptID = Security.DecryptParams(IdUser);
                    int.TryParse(decryptID, out idUser);
                }
                else
                {
                    idUser = 0;
                }
                

                IEnumerable<object> users;

                if (idUser != 0)
                {
                    users = db.tb_Usuario.Where(x => x.ID_USUARIO == idUser).Select(x => new
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

        public HttpResponseMessage GetRoles()
        {
            var roles = db.tb_RolesPrivacidad.Select(x => new {x.ID_ROL, x.Rol, x.Description}).ToList();

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<object>>(roles.Cast<object>().ToList(), new JsonMediaTypeFormatter())
            };
        }
        #endregion

        #region Edit
        public HttpResponseMessage EditUser(string ID_User, EditRequestUser EditUser)
        {
            try
            {
                string decryptID = Security.DecryptParams(ID_User);
                int idUser;
                int.TryParse(decryptID, out idUser);

                var userExist = db.tb_Usuario.Where(x => x.ID_USUARIO == idUser).FirstOrDefault();

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
        #endregion

        #region Delete
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

        #endregion


    }
}