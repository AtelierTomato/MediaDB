using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.APIClient
{
	public class SeriesNameClient
	{
		private readonly RestClient _restClient;
		public SeriesNameClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<bool> WriteSeriesNameAsync(SeriesName seriesName)
			=> await _restClient.PostAsync(nameof(SeriesName), seriesName);

		public async Task<bool> WriteSeriesNameRangeAsync(IEnumerable<SeriesName> seriesNameRange)
			=> await _restClient.PostAsync($"{nameof(SeriesName)}/range", seriesNameRange);

		public async Task<SeriesName?> ReadSeriesNameAsync(ulong ID, CultureInfo language, ScriptType script)
			=> await _restClient.GetAsync<SeriesName?>($"{nameof(SeriesName)}/{ID}/{language.Name}/{script}");

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeAsync(IEnumerable<ulong> IDs, CultureInfo language, ScriptType script)
			=> await _restClient.GetAsync<IEnumerable<SeriesName>>($"{nameof(SeriesName)}/range?IDs={string.Join(',', IDs)}&language={language.Name}&script={script}");

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesAsync(ulong ID)
			=> await _restClient.GetAsync<IEnumerable<SeriesName>>($"{nameof(SeriesName)}/{ID}");

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesPluralAsync(IEnumerable<ulong> IDs)
			=> await _restClient.GetAsync<IEnumerable<SeriesName>>($"{nameof(SeriesName)}/range?IDs={string.Join(',', IDs)}");

		public async Task<IEnumerable<SeriesName>> ReadAllSeriesNamesAsync()
			=> await _restClient.GetAsync<IEnumerable<SeriesName>>($"{nameof(SeriesName)}/all");

		public async Task<bool> DeleteSeriesNameAsync(ulong ID, CultureInfo language, ScriptType script)
			=> await _restClient.DeleteAsync($"{nameof(SeriesName)}/{ID}/{language.Name}/{script}");

		public async Task<bool> DeleteSeriesNameRangeForSeriesAsync(ulong ID)
			=> await _restClient.DeleteAsync($"{nameof(SeriesName)}/{ID}");

		public async Task<int> CountSeriesNamesAsync()
			=> await _restClient.GetAsync<int>($"{nameof(SeriesName)}/count");
	}
}
