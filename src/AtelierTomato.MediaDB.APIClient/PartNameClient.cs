using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.APIClient
{
	public class PartNameClient
	{
		private readonly RestClient _restClient;
		public PartNameClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<bool> WritePartNameAsync(PartName partName)
			=> await _restClient.PostAsync(nameof(PartName), partName);

		public async Task<bool> WritePartNameRangeAsync(IEnumerable<PartName> partNameRange)
			=> await _restClient.PostAsync($"{nameof(PartName)}/range", partNameRange);

		public async Task<PartName?> ReadPartNameAsync(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
			=> await _restClient.GetAsync<PartName?>($"{nameof(PartName)}/{seriesID}/{partID}/{language}/{script}");

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesWithLanguageAsync(ulong seriesID, CultureInfo language, ScriptType script)
			=> await _restClient.GetAsync<IEnumerable<PartName>>($"{nameof(PartName)}/{seriesID}/all/{language}/{script}");

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForPartAsync(ulong seriesID, PartID partID)
			=> await _restClient.GetAsync<IEnumerable<PartName>>($"{nameof(PartName)}/{seriesID}/{partID}");

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesAsync(ulong seriesID)
			=> await _restClient.GetAsync<IEnumerable<PartName>>($"{nameof(PartName)}/{seriesID}");

		public async Task<IEnumerable<PartName>> ReadAllPartNamesAsync()
			=> await _restClient.GetAsync<IEnumerable<PartName>>($"{nameof(PartName)}/all");

		public async Task<bool> DeletePartNameAsync(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
			=> await _restClient.DeleteAsync($"{nameof(PartName)}/{seriesID}/{partID}/{language}/{script}");

		public async Task<bool> DeletePartNameRangeForPartAsync(ulong seriesID, PartID partID)
			=> await _restClient.DeleteAsync($"{nameof(PartName)}/{seriesID}/{partID}");

		public async Task<bool> DeletePartNameRangeForSeriesAsync(ulong seriesID)
			=> await _restClient.DeleteAsync($"{nameof(PartName)}/{seriesID}");

		public async Task<int> CountPartNamesAsync()
			=> await _restClient.GetAsync<int>($"{nameof(PartName)}/count");
	}
}
