using Ejercicio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace EjercicioWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true && Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                try
                {
                    // Obtener usuario a partir de la cookie
                    string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                    string roles = string.Empty;

                    // Llamar a WebApi para obtener roles
                    using (WebApiClient client = new WebApiClient())
                    {
                        User usuario = client.GetUser(username);
                        if (usuario != null)
                        {
                            roles = usuario.Roles;
                        }
                    }

                    // Setear el usuario, identificado como una identidad (nombre y tipo de autenticación) y un conjunto de roles
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                        new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
