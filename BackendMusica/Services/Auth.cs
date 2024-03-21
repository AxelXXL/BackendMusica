using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;

namespace BackendMusica.Services
{
    public class Auth : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (HttpContext.Current.Request.Headers.Count == 0 || HttpContext.Current.Request.Headers["Authorization"] == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("No se ha encontrado el token de autenticidad"),
                    ReasonPhrase = "Error Token"
                });
            }
            else
            {
                if (!API.isValidToken(HttpContext.Current.Request.Headers["Authorization"]))
                {
                    throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("El token usado no es valido"),
                        ReasonPhrase = "Error Token"
                    });
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //base.OnActionExecuted(actionExecutedContext);
        }
    }
}