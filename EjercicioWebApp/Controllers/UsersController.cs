using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ejercicio.Domain;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EjercicioWebApp.Controllers
{
    public class UsersController : Controller
    {
        private WebApiClient client = new WebApiClient();

        // GET: Users
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Index()
        {
            IEnumerable<User> usuarios = new List<User>();

            try
            {
                usuarios = client.GetUsers();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(usuarios);
        }

        // GET: Users/Details/5
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Details(Guid? id)
        {
            User user = new User();
            try
            { 
                if (id == null)
                {
                    throw new Exception("Falta el parámetro Id");
                }
                user = client.GetUser(id.Value);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(user);
        }

        // GET: Users/Create
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,Roles")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Id = Guid.NewGuid();
                    client.PostUser(user);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Edit(Guid? id)
        {
            User user = new User();

            try
            {
                if (id == null)
                {
                    throw new Exception("Falta el parámetro Id");
                }
                user = client.GetUser(id.Value);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,Roles")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    client.PutUser(user);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult Delete(Guid? id)
        {
            User user = new User();

            try
            {
                if (id == null)
                {
                    throw new Exception("Falta el parámetro Id");
                }
                user = client.GetUser(id.Value);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "ADMIN")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                client.DeleteUser(id);
            }
            catch (Exception e)
            {
                User user = new User();
                ModelState.AddModelError("", e.Message);
                return View(user);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
