using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EjercicioWebApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Prohibido()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}