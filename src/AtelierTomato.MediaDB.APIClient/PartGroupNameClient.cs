using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.APIClient
{
	public class PartGroupNameClient
	{
		public readonly RestClient _restClient;
		public PartGroupNameClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<bool> WritePartGroupNameAsync(PartGroupName partGroupName)
			=> await _restClient.PostAsync(nameof(PartGroupName), partGroupName);

		public async Task<bool> WritePartGroupNameRangeAsync(IEnumerable<PartGroupName> partGroupNameRange)
			=> await _restClient.PostAsync($"{nameof(PartGroupName)}/range", partGroupNameRange);

		public async Task<PartGroupName?> ReadPartGroupNameAsync(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
			=> await _restClient.GetAsync<PartGroupName?>($"{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}/{language}/{script}");

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesWithLanguageAsync(ulong seriesID, CultureInfo language, ScriptType script)
			=> await _restClient.GetAsync<IEnumerable<PartGroupName>>($"{nameof(PartGroupName)}/{seriesID}/all/{language}/{script}");

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForPartAsync(ulong seriesID, PartID? parentPartID)
			=> await _restClient.GetAsync<IEnumerable<PartGroupName>>($"{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}");

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesAsync(ulong seriesID)
			=> await _restClient.GetAsync<IEnumerable<PartGroupName>>($"{nameof(PartGroupName)}/{seriesID}");

		public async Task<IEnumerable<PartGroupName>> ReadAllPartGroupNamesAsync()
			=> await _restClient.GetAsync<IEnumerable<PartGroupName>>($"{nameof(PartGroupName)}/all");

		public async Task<bool> DeletePartGroupNameAsync(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
			=> await _restClient.DeleteAsync($"{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}/{language}/{script}");

		public async Task<bool> DeletePartGroupNameRangeForPartAsync(ulong seriesID, PartID? parentPartID)
			=> await _restClient.DeleteAsync($"{nameof(PartGroupName)}/{seriesID}/{parentPartID?.ToString() ?? "null"}");

		public async Task<bool> DeletePartGroupNameRangeForSeriesAsync(ulong seriesID)
			=> await _restClient.DeleteAsync($"{nameof(PartGroupName)}/{seriesID}");

		public async Task<int> CountPartGroupNamesAsync()
			=> await _restClient.GetAsync<int>($"{nameof(PartGroupName)}/count");
	}
}
