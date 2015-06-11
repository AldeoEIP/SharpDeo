using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpDeo.Model;

namespace SharpDeo
{
	public class Client
	{
		private const string BaseUrl = "https://aldeo.io";

		public Task<List<Foo>> GetFooAsync() { return GetAsync<List<Foo>>("api/model"); }

		private static async Task<T> GetAsync<T>(string resource) where T : new()
		{
			using (var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) })
			{
				var json = await httpClient.GetStringAsync(resource).ConfigureAwait(false);
				return JsonConvert.DeserializeObject<T>(json);
			}
		}
	}
}
