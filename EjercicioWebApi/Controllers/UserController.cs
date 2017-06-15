using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EjercicioWebApi.Repository;
using Ejercicio.Domain;

namespace EjercicioWebApi.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<User> Get()
        {
            return InMemoryDB.Instance.GetAll();
        }

        // GET: api/User?userName=user1
        public User Get(string userName)
        {
            return InMemoryDB.Instance.GetAll().Where(x => x.UserName == userName).FirstOrDefault();
        }

        // GET api/User/<guid>
        public User Get(Guid id)
        {
            return InMemoryDB.Instance.GetAll().SingleOrDefault(x => x.Id == id);
        }

        // POST api/User
        public HttpResponseMessage Post(User value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InMemoryDB.Instance.AddUser(value);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/User/<guid>
        public HttpResponseMessage Put(Guid id, User value)
        {
            try
            {
                User usuario = InMemoryDB.Instance.GetAll().SingleOrDefault(x => x.Id == id);
                if (usuario != null)
                {
                    usuario.UserName = value.UserName;
                    usuario.Password = value.Password;
                    usuario.Roles = value.Roles;
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/User/<guid>
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                User usuario = InMemoryDB.Instance.GetAll().SingleOrDefault(x => x.Id == id);
                if (usuario != null)
                {
                    InMemoryDB.Instance.RemoveUser(usuario);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
