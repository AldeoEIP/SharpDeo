using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpDeo.Model;

namespace SharpDeo {
    public class Client {
        private const string BaseUrl = "http://aldeo.io/api/";

        private string Token { get; set; }

        public async Task LoginAsync(string login, string password) {
            //users / login
            using (var httpClient = new HttpClient ()) {

                var values = new Dictionary<string, string> { { "login", login }, { "password", password } };
                var content = new FormUrlEncodedContent (values);

                var response = await httpClient.PostAsync (BaseUrl + "users/login", content).ConfigureAwait (false);
                if (response.StatusCode == HttpStatusCode.OK) { // else throw error?
                    Token = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
                }
            }
        }

        public Task<List<Foo>> GetFooAsync() { return GetAsync<List<Foo>> ("api/model"); }

        private static async Task<T> GetAsync<T>(string resource) where T : new() {
            using (var httpClient = new HttpClient { BaseAddress = new Uri (BaseUrl) }) {
                var json = await httpClient.GetStringAsync (resource).ConfigureAwait (false);
                return JsonConvert.DeserializeObject<T> (json);
            }
        }
    }
}