using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorInvoice.Data.Extensions
{
	public static class HttpClientExtensions
	{
		public static async Task<R> GetJsonAsync<R>(this HttpClient httpClient, string url)
		{
			var httpResponse = await httpClient.GetAsync(url);

			if(httpResponse.IsSuccessStatusCode)
			{
				var result = JsonConvert.DeserializeObject<R>(await httpResponse.Content.ReadAsStringAsync());
				return result;
			}

			return default(R);
		}

		public static async Task<R> PostJsonAsync<T, R>(this HttpClient httpClient, T model, string url)
		{
			var json = JsonConvert.SerializeObject(model, Formatting.None);
			var byteContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

			var httpResponse = await httpClient.PostAsync(url, byteContent);

			if (httpResponse.IsSuccessStatusCode)
			{
				var result = JsonConvert.DeserializeObject<R>(await httpResponse.Content.ReadAsStringAsync());
				return result;
			}

			return default(R);
		}
	}
}
