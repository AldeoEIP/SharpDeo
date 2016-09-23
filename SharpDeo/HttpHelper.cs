using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharpDeo
{
    public class HttpHelper : IDisposable {
        private static HttpClient _httpClient;

        public HttpHelper(string baseUrl) {
            _httpClient = new HttpClient { BaseAddress = new Uri (baseUrl) };
        }

        public static Task<string> GetAsync(string token, string requestUri) {
            return SendAsync (token, HttpMethod.Get, requestUri, new StringContent (string.Empty));
        }

        public static Task<HttpResponseMessage> PostAsync(string request, HttpContent content) {
            return _httpClient.PostAsync (request, content);
        }

        public static Task<string> PostAsync(string token, string requestUri, string key, string value) {
            var content = new KeyValuePair<string, string> (key, value);
            return PostCollectionAsync (token, requestUri, new Dictionary<string, string> { { key, value } });
        }

        public static Task<string> PostCollectionAsync(string token, string requestUri, IDictionary<string, string> content) {
            var httpContent = new FormUrlEncodedContent (content); // smallest enumerable?
            return SendAsync (token, HttpMethod.Post, requestUri, httpContent);
        }

        public static async Task<string> SendAsync(string token, HttpMethod method, string requestUri, HttpContent content) {
            using (var httpRequest = new HttpRequestMessage (method, requestUri) {
                Headers = { { "auth", token } },
                Content = content
            }) {
                var httpResponse = await _httpClient.SendAsync (httpRequest).ConfigureAwait (false);
                httpResponse.EnsureSuccessStatusCode ();
                var response = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
                return response;
            }
        }

        public void Dispose() {
            _httpClient.Dispose ();
        }
    }
}