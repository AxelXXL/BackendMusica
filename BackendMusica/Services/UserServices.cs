﻿using BackendMusica.Models;
using Newtonsoft.Json;
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
                    users = db.Tb_Usuario.Where(x => x.ID_Usuario == idUser).Select(x => new
                    {
                        x.ID_Usuario,
                        x.Nombre_Usuario,
                        x.Tb_Rol.ID_Rol,
                        x.Tb_Rol.Rol,
                        x.Activo
                    }).ToList();
                }
                else
                {
                    users = db.Tb_Usuario.Select(x => new
                    {
                        x.ID_Usuario,
                        x.Nombre_Usuario,
                        x.Tb_Rol.ID_Rol,
                        x.Tb_Rol.Rol,
                        x.Activo
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
            var roles = db.Tb_Rol.Select(x => new {x.ID_Rol, x.Rol, x.Descripcion}).ToList();

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<object>>(roles.Cast<object>().ToList(), new JsonMediaTypeFormatter())
            };
        }

        public HttpResponseMessage GetRol(int ID_Rol)
        {
            var rol = db.Tb_Rol.Where(x => x.ID_Rol == ID_Rol).Select(x => new { x.ID_Rol, x.Rol, x.Descripcion }).First();

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<object>(rol, new JsonMediaTypeFormatter())
            };
        }
        #endregion

        #region Create

        public HttpResponseMessage CreateUser(string newUser)
        {
            try
            {
                string decryptJson = Security.DecryptParams(newUser);

                Tb_Usuario createNewUser = JsonConvert.DeserializeObject<Tb_Usuario>(decryptJson);

                if(createNewUser.Contrasena == createNewUser.ConfirmarContrasena)
                {
                    createNewUser.Contrasena = Security.Encrypt(createNewUser.Contrasena);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent("Las contraseñas no coinciden")
                    };
                }

                db.Tb_Usuario.Add(createNewUser);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"Usuario {createNewUser.Nombre_Usuario} creado correctamente.")
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

        public HttpResponseMessage CreateRol(string newRol)
        {
            try
            {
                string decryptJson = Security.DecryptParams(newRol);
                Tb_Rol createNewRol = JsonConvert.DeserializeObject<Tb_Rol>(decryptJson);

                var rolExists = db.Tb_Rol.Where(x => x.Rol.ToUpper() == createNewRol.Rol.ToUpper()).FirstOrDefault();

                if(rolExists != null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent("Ya existe un rol con el nombre asignado.")
                    };
                }

                db.Tb_Rol.Add(createNewRol);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Rol creado exitosamente.")
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

        #endregion

        #region Edit
        public HttpResponseMessage EditUser(string ID_User, EditRequestUser EditUser)
        {
            try
            {
                string decryptID = Security.DecryptParams(ID_User);
                int idUser;
                int.TryParse(decryptID, out idUser);

                var userExist = db.Tb_Usuario.Where(x => x.ID_Usuario == idUser).FirstOrDefault();

                if(userExist == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("El permiso asignado no existe.")
                    };
                }

                if (userExist.Nombre_Usuario != EditUser.Nombre_Usuario 
                    || userExist.ID_Rol != EditUser.ID_Rol
                    || userExist.Activo != EditUser.Active)
                {
                    db.Tb_Usuario.Attach(userExist);

                    userExist.Nombre_Usuario = EditUser.Nombre_Usuario;
                    userExist.ID_Rol = EditUser.ID_Rol;
                    userExist.Activo = EditUser.Active;

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

        public HttpResponseMessage EditRol(string ID_Rol, RolModel editRol)
        {
            try
            {
                string decryptID = Security.DecryptParams(ID_Rol);
                int idRol;
                int.TryParse(decryptID, out idRol);

                var rolEqual = db.Tb_Rol.Where(x => x.Rol == editRol.Rol).FirstOrDefault();
                var rolExist = db.Tb_Rol.Where(x => x.ID_Rol == editRol.ID_Rol).FirstOrDefault();

                if (rolEqual == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("Ya existe un rol con el mismo nombre.")
                    };
                }

                db.Tb_Rol.Attach(rolExist);

                rolExist.Rol = editRol.Rol;
                rolExist.Descripcion = editRol.Descripcion;

                db.Entry(rolExist).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"El rol se edito correctamente.")
                };

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
        public HttpResponseMessage DeleteUser(string ID_User)
        {
            try
            {

                string decryptID = Security.DecryptParams(ID_User);
                int idUser;
                int.TryParse(decryptID, out idUser);

                var user = db.Tb_Usuario.Where(x => x.ID_Usuario == idUser).FirstOrDefault();

                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No se encontro al usuario.")
                    };
                }
                else
                {
                    db.Tb_Usuario.Remove(user);
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

        public HttpResponseMessage DeleteRol(string ID_Rol)
        {
            try
            {
                string decryptID = Security.DecryptParams(ID_Rol);
                int idRol;
                int.TryParse(decryptID, out idRol);

                var rol = db.Tb_Rol.Where(x => x.ID_Rol == idRol).FirstOrDefault();

                db.Tb_Rol.Remove(rol);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"El rol se elimino correctamente.")
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

        #endregion


    }
}