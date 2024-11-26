using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.APIClient
{
	public class SeriesClient
	{
		private readonly RestClient _restClient;
		public SeriesClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<ulong> WriteNewSeriesAsync(Series series)
			=> await _restClient.PostAsyncWithResponse<ulong>(nameof(Series), series);

		public async Task<IEnumerable<ulong>> WriteNewSeriesRangeAsync(IEnumerable<Series> seriesRange)
			=> await _restClient.PostAsyncWithResponse<IEnumerable<ulong>>($"{nameof(Series)}/range", seriesRange);

		public async Task<bool> WriteSeriesAsync(Series series)
			=> await _restClient.PutAsync(nameof(Series), series);

		public async Task<bool> WriteSeriesRangeAsync(IEnumerable<Series> seriesRange)
			=> await _restClient.PutAsync($"{nameof(Series)}/range", seriesRange);

		public async Task<Series?> ReadSeriesAsync(ulong ID)
			=> await _restClient.GetAsync<Series?>($"{nameof(Series)}/{ID}");

		public async Task<IEnumerable<Series>> ReadSeriesRangeAsync(IEnumerable<ulong> IDs)
			=> await _restClient.GetAsync<IEnumerable<Series>>($"{nameof(Series)}/range?IDs={string.Join(',', IDs)}");

		public async Task<IEnumerable<Series>> ReadAllSeriesAsync()
			=> await _restClient.GetAsync<IEnumerable<Series>>($"{nameof(Series)}/all");

		public async Task<IEnumerable<Series>> SearchSeriesByNameAsync(string name)
			=> await _restClient.GetAsync<IEnumerable<Series>>($"{nameof(Series)}/search/name/{name}");

		public async Task<bool> DeleteSeriesAsync(ulong ID)
			=> await _restClient.DeleteAsync($"{nameof(Series)}/{ID}");

		public async Task<int> CountSeriesAsync()
			=> await _restClient.GetAsync<int>($"{nameof(Series)}/count");
	}
}
