using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Model.Converters;
using System.Globalization;
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

		public async Task<bool> CheckHealthAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("health");

				return response.IsSuccessStatusCode;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// PART
		public async Task<bool> WritePartAsync(Part part)
			=> await PostAsync($"api/{nameof(Part)}", part);

		public async Task<bool> WritePartRangeAsync(IEnumerable<Part> partRange)
			=> await PostAsync($"api/{nameof(Part)}/range", partRange);

		public async Task<Part?> ReadPartAsync(ulong seriesID, PartID partID)
			=> await GetAsync<Part?>($"api/{nameof(Part)}/{seriesID}/{partID}");

		public async Task<IEnumerable<Part>> ReadPartRangeBySeriesAsync(ulong seriesID)
			=> await GetAsync<IEnumerable<Part>>($"api/{nameof(Part)}/{seriesID}");

		public async Task<IEnumerable<Part>> ReadPartRangeBySeriesRangeAsync(IEnumerable<ulong> seriesIDs)
			=> await GetAsync<IEnumerable<Part>>($"api/{nameof(Part)}/range?seriesIDs={string.Join(',', seriesIDs)}");

		public async Task<IEnumerable<Part>> ReadAllPartsAsync()
			=> await GetAsync<IEnumerable<Part>>($"api/{nameof(Part)}/all");

		public async Task<IEnumerable<Part>> SearchPartByNameAsync(string name)
			=> await GetAsync<IEnumerable<Part>>($"api/{nameof(Part)}/search/name/{name}");

		public async Task<bool> DeletePartAsync(ulong seriesID, PartID partID)
			=> await DeleteAsync($"api/{nameof(Part)}/{seriesID}/{partID}");

		public async Task<bool> DeletePartRangeInSeriesAsync(ulong seriesID, IEnumerable<PartID> partIDs)
			=> await DeleteAsync($"api/{nameof(Part)}/{seriesID}?partIDs={string.Join(',', partIDs.Select(p => p.ToString()))}");

		public async Task<int> CountPartsAsync()
			=> await GetAsync<int>($"api/{nameof(Part)}/count");

		// PART GROUP INFO
		public async Task<bool> WritePartGroupInfoAsync(PartGroupInfo partGroupInfo)
			=> await PostAsync($"api/{nameof(PartGroupInfo)}", partGroupInfo);

		public async Task<bool> WritePartGroupInfoRangeAsync(IEnumerable<PartGroupInfo> partGroupInfoRange)
			=> await PostAsync($"api/{nameof(PartGroupInfo)}/range", partGroupInfoRange);

		public async Task<PartGroupInfo?> ReadPartGroupInfoAsync(ulong seriesID, PartID? parentPartID)
			=> await GetAsync<PartGroupInfo?>($"api/{nameof(PartGroupInfo)}/{seriesID}/{parentPartID?.ToString() ?? null}");

		public async Task<IEnumerable<PartGroupInfo>> ReadPartGroupInfoRangeForSeriesAsync(ulong seriesID)
			=> await GetAsync<IEnumerable<PartGroupInfo>>($"api/{nameof(PartGroupInfo)}/{seriesID}");

		public async Task<IEnumerable<PartGroupInfo>> ReadAllPartGroupInfos()
			=> await GetAsync<IEnumerable<PartGroupInfo>>($"api/{nameof(PartGroupInfo)}/all");

		public async Task<bool> DeletePartGroupInfoAsync(ulong seriesID, PartID? parentPartID)
			=> await DeleteAsync($"api/{nameof(PartGroupInfo)}/{seriesID}/{parentPartID?.ToString() ?? null}");

		public async Task<bool> DeletePartGroupInfoRangeForSeriesAsync(ulong seriesID)
			=> await DeleteAsync($"api/{nameof(PartGroupInfo)}/{seriesID}");

		public async Task<int> CountPartGroupInfoAsync()
			=> await GetAsync<int>($"api/{nameof(PartGroupInfo)}/count");

		// PART GROUP NAME
		public async Task<bool> WritePartGroupNameAsync(PartGroupName partGroupName)
			=> await PostAsync($"api/{nameof(PartGroupName)}", partGroupName);

		public async Task<bool> WritePartGroupNameRangeAsync(IEnumerable<PartGroupName> partGroupNameRange)
			=> await PostAsync($"api/{nameof(PartGroupName)}/range", partGroupNameRange);

		public async Task<PartGroupName?> ReadPartGroupNameAsync(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
			=> await GetAsync<PartGroupName?>($"api/{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}/{language}/{script}");

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesWithLanguageAsync(ulong seriesID, CultureInfo language, ScriptType script)
			=> await GetAsync<IEnumerable<PartGroupName>>($"api/{nameof(PartGroupName)}/{seriesID}/all/{language}/{script}");

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForPartAsync(ulong seriesID, PartID? parentPartID)
			=> await GetAsync<IEnumerable<PartGroupName>>($"api/{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}");

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesAsync(ulong seriesID)
			=> await GetAsync<IEnumerable<PartGroupName>>($"api/{nameof(PartGroupName)}/{seriesID}");

		public async Task<IEnumerable<PartGroupName>> ReadAllPartGroupNamesAsync()
			=> await GetAsync<IEnumerable<PartGroupName>>($"api/{nameof(PartGroupName)}/all");

		public async Task<bool> DeletePartGroupNameAsync(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
			=> await DeleteAsync($"api/{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}/{language}/{script}");

		public async Task<bool> DeletePartGroupNameRangeForPartAsync(ulong seriesID, PartID? parentPartID)
			=> await DeleteAsync($"api/{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}");

		public async Task<bool> DeletePartGroupNameRangeForSeriesAsync(ulong seriesID)
			=> await DeleteAsync($"api/{nameof(PartGroupName)}/{seriesID}");

		public async Task<int> CountPartGroupNamesAsync()
			=> await GetAsync<int>($"api/{nameof(PartGroupName)}/count");

		// PART NAME
		public async Task<bool> WritePartNameAsync(PartName partName)
			=> await PostAsync($"api/{nameof(PartName)}", partName);

		public async Task<bool> WritePartNameRangeAsync(IEnumerable<PartName> partNameRange)
			=> await PostAsync($"api/{nameof(PartName)}/range", partNameRange);

		public async Task<PartName?> ReadPartNameAsync(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
			=> await GetAsync<PartName?>($"api/{nameof(PartName)}/{seriesID}/{partID}/{language}/{script}");

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesWithLanguageAsync(ulong seriesID, CultureInfo language, ScriptType script)
			=> await GetAsync<IEnumerable<PartName>>($"api/{nameof(PartName)}/{seriesID}/all/{language}/{script}");

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForPartAsync(ulong seriesID, PartID partID)
			=> await GetAsync<IEnumerable<PartName>>($"api/{nameof(PartName)}/{seriesID}/{partID}");

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesAsync(ulong seriesID)
			=> await GetAsync<IEnumerable<PartName>>($"api/{nameof(PartName)}/{seriesID}");

		public async Task<IEnumerable<PartName>> ReadAllPartNamesAsync()
			=> await GetAsync<IEnumerable<PartName>>($"api/{nameof(PartName)}/all");

		public async Task<bool> DeletePartNameAsync(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
			=> await DeleteAsync($"api/{nameof(PartName)}/{seriesID}/{partID}/{language}/{script}");

		public async Task<bool> DeletePartNameRangeForPartAsync(ulong seriesID, PartID partID)
			=> await DeleteAsync($"api/{nameof(PartName)}/{seriesID}/{partID}");

		public async Task<bool> DeletePartNameRangeForSeriesAsync(ulong seriesID)
			=> await DeleteAsync($"api/{nameof(PartName)}/{seriesID}");

		public async Task<int> CountPartNamesAsync()
			=> await GetAsync<int>($"api/{nameof(PartName)}/count");

		// SERIES
		public async Task<ulong> WriteNewSeriesAsync(Series series)
			=> await PostAsyncWithResponse<ulong>($"api/{nameof(Series)}", series);

		public async Task<IEnumerable<ulong>> WriteNewSeriesRangeAsync(IEnumerable<Series> seriesRange)
			=> await PostAsyncWithResponse<IEnumerable<ulong>>($"api/{nameof(Series)}/range", seriesRange);

		public async Task<bool> WriteSeriesAsync(Series series)
			=> await PutAsync($"api/{nameof(Series)}", series);

		public async Task<bool> WriteSeriesRangeAsync(IEnumerable<Series> seriesRange)
			=> await PutAsync($"api/{nameof(Series)}/range", seriesRange);

		public async Task<Series?> ReadSeriesAsync(ulong ID)
			=> await GetAsync<Series?>($"api/{nameof(Series)}/{ID}");

		public async Task<IEnumerable<Series>> ReadSeriesRangeAsync(IEnumerable<ulong> IDs)
			=> await GetAsync<IEnumerable<Series>>($"api/{nameof(Series)}/range?IDs={string.Join(',', IDs)}");

		public async Task<IEnumerable<Series>> ReadAllSeriesAsync()
			=> await GetAsync<IEnumerable<Series>>($"api/{nameof(Series)}/all");

		public async Task<IEnumerable<Series>> SearchSeriesByNameAsync(string name)
			=> await GetAsync<IEnumerable<Series>>($"api/{nameof(Series)}/search/name/{name}");

		public async Task<bool> DeleteSeriesAsync(ulong ID)
			=> await DeleteAsync($"api/{nameof(Series)}/{ID}");

		public async Task<int> CountSeriesAsync()
			=> await GetAsync<int>($"api/{nameof(Series)}/count");

		// SERIES NAME
		public async Task<bool> WriteSeriesNameAsync(SeriesName seriesName)
			=> await PostAsync($"api/{nameof(SeriesName)}", seriesName);

		public async Task<bool> WriteSeriesNameRangeAsync(IEnumerable<SeriesName> seriesNameRange)
			=> await PostAsync($"api/{nameof(SeriesName)}/range", seriesNameRange);

		public async Task<SeriesName?> ReadSeriesNameAsync(ulong ID, CultureInfo language, ScriptType script)
			=> await GetAsync<SeriesName?>($"api/{nameof(SeriesName)}/{ID}/{language.Name}/{script}");

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeAsync(IEnumerable<ulong> IDs, CultureInfo language, ScriptType script)
			=> await GetAsync<IEnumerable<SeriesName>>($"api/{nameof(SeriesName)}/range?IDs={string.Join(',', IDs)}&language={language.Name}&script={script}");

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesAsync(ulong ID)
			=> await GetAsync<IEnumerable<SeriesName>>($"api/{nameof(SeriesName)}/{ID}");

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesPluralAsync(IEnumerable<ulong> IDs)
			=> await GetAsync<IEnumerable<SeriesName>>($"api/{nameof(SeriesName)}/range?IDs={string.Join(',', IDs)}");

		public async Task<IEnumerable<SeriesName>> ReadAllSeriesNamesAsync()
			=> await GetAsync<IEnumerable<SeriesName>>($"api/{nameof(SeriesName)}/all");

		public async Task<bool> DeleteSeriesNameAsync(ulong ID, CultureInfo language, ScriptType script)
			=> await DeleteAsync($"api/{nameof(SeriesName)}/{ID}/{language.Name}/{script}");

		public async Task<bool> DeleteSeriesNameRangeForSeriesAsync(ulong ID)
			=> await DeleteAsync($"api/{nameof(SeriesName)}/{ID}");

		public async Task<int> CountSeriesNamesAsync()
			=> await GetAsync<int>($"api/{nameof(SeriesName)}/count");


		public async Task<bool> WriteSeriesParentAsync(SeriesParent seriesParent)
			=> await PostAsync($"api/{nameof(SeriesParent)}", seriesParent);

		public async Task<bool> WriteSeriesParentRangeAsync(IEnumerable<SeriesParent> seriesParentRange)
			=> await PostAsync($"api/{nameof(SeriesParent)}/range", seriesParentRange);

		public async Task<SeriesParent?> ReadSeriesParentAsync(ulong ID, ulong parentID)
			=> await GetAsync<SeriesParent?>($"api/{nameof(SeriesParent)}/{ID}/{parentID}");

		public async Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeBySeriesAsync(ulong ID)
			=> await GetAsync<IEnumerable<SeriesParent>>($"api/{nameof(SeriesParent)}/byseries/{ID}");

		public async Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeByParentSeriesAsync(ulong parentID)
			=> await GetAsync<IEnumerable<SeriesParent>>($"api/{nameof(SeriesParent)}/byparent/{parentID}");

		public async Task<IEnumerable<SeriesParent>> ReadAllSeriesParentsAsync()
			=> await GetAsync<IEnumerable<SeriesParent>>($"api/{nameof(SeriesParent)}/all");

		public async Task<bool> DeleteSeriesParentAsync(ulong ID, ulong parentID)
			=> await DeleteAsync($"api/{nameof(SeriesParent)}/{ID}/{parentID}");

		public async Task<bool> DeleteSeriesParentRangeBySeriesAsync(ulong ID)
			=> await DeleteAsync($"api/{nameof(SeriesParent)}/byseries/{ID}");

		public async Task<bool> DeleteSeriesParentRangeByParentSeriesAsync(ulong parentID)
			=> await DeleteAsync($"api/{nameof(SeriesParent)}/byparent/{parentID}");

		public async Task<int> CountSeriesParentsAsync()
			=> await GetAsync<int>($"api/{nameof(SeriesParent)}/count");
	}
}
