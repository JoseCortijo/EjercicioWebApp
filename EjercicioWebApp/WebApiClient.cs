using Ejercicio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace EjercicioWebApp
{
    public class WebApiClient : IDisposable
    {
        private HttpClient client;

        public WebApiClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50665");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IEnumerable<User> GetUsers()
        {
            HttpResponseMessage response = client.GetAsync("api/User").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<IEnumerable<User>>().Result;
            }
            else
            {
                throw new Exception(string.Format("Error {0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
        }

        public User GetUser(string userName)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("api/User?userName={0}", userName)).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<User>().Result;
            }
            else
            {
                throw new Exception(string.Format("Error {0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
        }

        public User GetUser(Guid id)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("api/User/{0}", id)).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<User>().Result;
            }
            else
            {
                throw new Exception(string.Format("Error {0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
        }

        public void PostUser(User user)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/User", user).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("Error {0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
        }

        public void PutUser(User user)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(string.Format("api/User/{0}", user.Id), user).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("Error {0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
        }

        public void DeleteUser(Guid id)
        {
            HttpResponseMessage response = client.DeleteAsync(string.Format("api/User/{0}", id)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("Error {0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
        }

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }
    }
}