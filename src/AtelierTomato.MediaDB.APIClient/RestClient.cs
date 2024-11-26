using AtelierTomato.MediaDB.Model.Converters;
using System.Text;
using System.Text.Json;

namespace AtelierTomato.MediaDB.APIClient
{
	public class RestClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _jsonOptions;

		public RestClient(string baseAddress)
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(baseAddress)
			};

			_jsonOptions = new JsonSerializerOptions
			{
				Converters =
				{
					new CultureInfoConverter(),
					new RegionInfoConverter(),
					new PartIDConverter(),
				}
			};
		}

		public async Task<T> GetAsync<T>(string endpoint)
		{
			var response = await _httpClient.GetAsync(endpoint);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);

			return result ?? default!;
		}

		public async Task<bool> PostAsync(string endpoint, object data) => (await PostAsyncWithResponse<HttpResponseMessage>(endpoint, data)).IsSuccessStatusCode;
		public async Task<T> PostAsyncWithResponse<T>(string endpoint, object data)
		{
			var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(endpoint, content);
			return await ParseResponseAsync<T>(response);
		}

		public async Task<bool> PutAsync(string endpoint, object data) => (await PutAsyncWithResponse<HttpResponseMessage>(endpoint, data)).IsSuccessStatusCode;
		public async Task<T> PutAsyncWithResponse<T>(string endpoint, object data)
		{
			var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(endpoint, content);
			return await ParseResponseAsync<T>(response);
		}

		public async Task<bool> DeleteAsync(string endpoint) => (await _httpClient.DeleteAsync(endpoint)).IsSuccessStatusCode;

		private async Task<T> ParseResponseAsync<T>(HttpResponseMessage response)
		{
			if (typeof(T) == typeof(HttpResponseMessage))
				return (T)(object)response;

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(content, _jsonOptions) ?? default!;
		}
	}
}
