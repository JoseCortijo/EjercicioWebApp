using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ejercicio.Domain;

namespace EjercicioWebApi.Repository
{
    public class InMemoryDB
    {
        private List<User> _usuarios;

        public InMemoryDB()
        {
            _usuarios = InitUsers();
        }

        public IEnumerable<User> GetAll()
        {
            return _usuarios;
        }

        public void AddUser(User usuario)
        {
            _usuarios.Add(usuario);
        }

        public void RemoveUser(User usuario)
        {
            _usuarios.Remove(usuario);
        }

        public static InMemoryDB Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly InMemoryDB instance = new InMemoryDB();
            //internal static InMemoryDB instance = new InMemoryDB();
        }

        private List<User> InitUsers()
        {
            List<User> usuarios = new List<User>();

            User user1 = new User();
            user1.Id = Guid.NewGuid();
            user1.UserName = "user1";
            user1.Password = "user1";
            user1.Roles = "PAGE_1";

            User user2 = new User();
            user2.Id = Guid.NewGuid();
            user2.UserName = "user2";
            user2.Password = "user2";
            user2.Roles = "PAGE_2";

            User user3 = new User();
            user3.Id = Guid.NewGuid();
            user3.UserName = "user3";
            user3.Password = "user3";
            user3.Roles = "PAGE_3";

            User user4 = new User();
            user4.Id = Guid.NewGuid();
            user4.UserName = "admin";
            user4.Password = "admin";
            user4.Roles = "PAGE_1;PAGE_2;PAGE_3;ADMIN";

            usuarios.Add(user1);
            usuarios.Add(user2);
            usuarios.Add(user3);
            usuarios.Add(user4);

            return usuarios;
        }
    }
}