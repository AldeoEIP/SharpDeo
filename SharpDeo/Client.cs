using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpDeo.Extensions;
using SharpDeo.Model;

namespace SharpDeo {
    public class Client : IDisposable {
        private const string BaseUrl = "https://aldeo.io/api/";
        private static readonly HttpClient HttpClient = new HttpClient {BaseAddress = new Uri(BaseUrl)};
        private string Token { get; set; }

        public async Task LoginAsync(string login, string password) {
            var encodedContent = new FormUrlEncodedContent (
                new Dictionary<string, string> { { "login", login }, { "password", password } });

            var httpResponse = await HttpClient.PostAsync ("users/login", encodedContent).ConfigureAwait (false);
            httpResponse.EnsureSuccessStatusCode ();
            var content = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
            Token = content.Replace ("\"", string.Empty);
        }

        public Task<IEnumerable<User>> GetUsersAsync() {
            const string requestUri = "users";
            return GetTableAsync<User> (Token, requestUri);
        }

        public async Task<User> GetUserAsync(int id) {
            const string requestUri = "users";
            var json = await GetAsync (Token, $"{requestUri}/{id}").ConfigureAwait (false);
            var table = JsonConvert.DeserializeObject<User> (json);
            return table;
        }

        public async Task<string> CreateUserAsync(Noob noob) {
            const string requestUri = "users";
            var response = await CreateFieldAsync (noob, Token, requestUri).ConfigureAwait (false);
            // check for fail
            return response;
        }

        public Task<Knowledge> GetDictionaryDefintionAsync(string word) {
            const string requestUri = "wiktionaries/getDefinition";
            const string key = "word";
            return GetKnowledgeAsync (requestUri, key, word.ToLower ());
        }

        public Task<Knowledge> GetEncyclopediaDefintionAsync(string word) {
            const string requestUri = "wikipedias/getExtract";
            const string key = "text";
            return GetKnowledgeAsync (requestUri, key, word.ToLower ());
        }

        public async Task<Knowledge> GetKnowledgeAsync(string requestUri, string key, string notion) {
            var json = await PostAsync (Token, requestUri, key, notion).ConfigureAwait (false);
            var knowledge = JsonConvert.DeserializeObject<Knowledge> (json);
            return knowledge;
        }

        public Task<string> GetAnswerAsync(string question) {
            const string requestUri = "aimls/answer";
            const string key = "text";
            return PostAsync (Token, requestUri, key, question);
        }

        // avoir le résultat d’un calcul
        public Task<string> ResolveAsync(string input) {
            const string requestUri = "wolframalphas/getMath";
            const string key = "input";
            return PostAsync (Token, requestUri, key, input.ToLower ());
        }

        // http://stackoverflow.com/questions/4737970/what-does-where-t-class-new-mean
        // voir tous les champs existants
        public static Task<IEnumerable<T>> GetTableAsync<T>(string token, string requestUri) where T : class {
            return GetFieldAsync<IEnumerable<T>> (token, requestUri);
        }

        // voir un champs
        // ne supporte pas /light
        public static async Task<T> GetFieldAsync<T>(string token, string requestUri) where T : class {
            var json = await GetAsync (token, requestUri).ConfigureAwait (false);
            var t = JsonConvert.DeserializeObject<T> (json);
            return t;
        }

        //créer un champs
        public static Task<string> CreateFieldAsync<T>(T field, string token, string requestUri) {

            Func<string, string> e = s => $"\"s\"";

            using (var multipartFormDataContent = new MultipartFormDataContent ()) {
                foreach (var keyValuePair in new[]
                {
                    new KeyValuePair<string, string>("login", "TheLordOfTheRings"),
                    new KeyValuePair<string, string>("email", "Soso91@hotmail.fr"),
                    new KeyValuePair<string, string>("password", "1ringToRule"),
                    new KeyValuePair<string, string>("firstname", "Sauron"),
                    new KeyValuePair<string, string>("lastname", "Duhamel"),
                    new KeyValuePair<string, string>("companionName", "Saroumane")
                })
                    multipartFormDataContent.Add (new StringContent (keyValuePair.Value), e (keyValuePair.Key));

                //var encodedContent = new FormUrlEncodedContent(field.ToDictionaryOfString<T>());
                var httpContent = multipartFormDataContent;
                return SendAsync (token, HttpMethod.Post, requestUri + "/create", httpContent);
            }
        }

        // editer un champs
        //public static Task EditFieldAsync<T>(string token, string requestUri) {
        //    var httpContent = new StringContent (string.Empty);  // should add user in content as json?
        //    return SendAsync (token, HttpMethod.Put, requestUri, httpContent);
        //}

        // supprimer un champs
        //public static Task DeleteFieldAsync<T>(string token, string requestUri) {
        //    var httpContent = new StringContent (string.Empty);  // should add user in content as json?
        //    return SendAsync (token, HttpMethod.Delete, requestUri, httpContent);
        //}

        public static Task<string> GetAsync(string token, string requestUri) {
            return SendAsync (token, HttpMethod.Get, requestUri, new StringContent (string.Empty));
        }

        private static Task<string> PostAsync(string token, string requestUri, string key, string value) {
            var content = new KeyValuePair<string, string> (key, value);
            var httpContent = new FormUrlEncodedContent (Enumerable.Repeat (content, 1)); // smallest enumerable?
            return SendAsync (token, HttpMethod.Post, requestUri, httpContent);
        }

        private static async Task<string> SendAsync(string token, HttpMethod method, string requestUri, HttpContent content) {
            using (var httpRequest = new HttpRequestMessage (method, requestUri) {
                Headers = { { "auth", token } },
                Content = content
            }) {
                var httpResponse = await HttpClient.SendAsync (httpRequest).ConfigureAwait (false);
                httpResponse.EnsureSuccessStatusCode ();
                var response = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
                return response;
            }
        }

        public void Dispose() {
            HttpClient.Dispose ();
        }
    }
}