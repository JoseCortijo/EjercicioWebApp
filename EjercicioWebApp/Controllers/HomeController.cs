using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using Ejercicio.Domain;
using System.Threading.Tasks;

namespace EjercicioWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string username = model.UserName;
                    string password = model.Password;

                    using (WebApiClient client = new WebApiClient())
                    {
                        User usuario = client.GetUser(username);
                        if (usuario != null && usuario.Password == password) // Login Ok
                        {
                            FormsAuthentication.SetAuthCookie(username, false);
                            if (!string.IsNullOrEmpty(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("UserDashBoard");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Usuario o password incorrectos.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        [CustomAuthorize]
        public ActionResult UserDashBoard()
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [CustomAuthorize(Roles = "PAGE_1")]
        public ActionResult Page1()
        {
            ViewBag.Message = "Sólo visible por rol PAGE_1";
            return View();
        }

        [CustomAuthorize(Roles = "PAGE_2")]
        public ActionResult Page2()
        {
            ViewBag.Message = "Sólo visible por rol PAGE_2";
            return View();
        }

        [CustomAuthorize(Roles = "PAGE_3")]
        public ActionResult Page3()
        {
            ViewBag.Message = "Sólo visible por rol PAGE_3";
            return View();
        }

        [CustomAuthorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}