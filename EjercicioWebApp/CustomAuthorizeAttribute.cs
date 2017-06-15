using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EjercicioWebApp
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated && !this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // El usuario está autenticado y no tiene el ningún rol => Página de error customizada
                filterContext.Result = new RedirectResult("~/Error/Prohibido");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}