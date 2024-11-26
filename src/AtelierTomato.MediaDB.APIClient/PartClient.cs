using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.APIClient
{
	public class PartClient
	{
		private readonly RestClient _restClient;
		public PartClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<bool> WritePartAsync(Part part)
			=> await _restClient.PostAsync($"{nameof(Part)}", part);

		public async Task<bool> WritePartRangeAsync(IEnumerable<Part> partRange)
			=> await _restClient.PostAsync($"{nameof(Part)}/range", partRange);

		public async Task<Part?> ReadPartAsync(ulong seriesID, PartID partID)
			=> await _restClient.GetAsync<Part?>($"{nameof(Part)}/{seriesID}/{partID}");

		public async Task<IEnumerable<Part>> ReadPartRangeBySeriesAsync(ulong seriesID)
			=> await _restClient.GetAsync<IEnumerable<Part>>($"{nameof(Part)}/{seriesID}");

		public async Task<IEnumerable<Part>> ReadPartRangeBySeriesRangeAsync(IEnumerable<ulong> seriesIDs)
			=> await _restClient.GetAsync<IEnumerable<Part>>($"{nameof(Part)}/range?seriesIDs={string.Join(',', seriesIDs)}");

		public async Task<IEnumerable<Part>> ReadAllPartsAsync()
			=> await _restClient.GetAsync<IEnumerable<Part>>($"{nameof(Part)}/all");

		public async Task<IEnumerable<Part>> SearchPartByNameAsync(string name)
			=> await _restClient.GetAsync<IEnumerable<Part>>($"{nameof(Part)}/search/name/{name}");

		public async Task<bool> DeletePartAsync(ulong seriesID, PartID partID)
			=> await _restClient.DeleteAsync($"{nameof(Part)}/{seriesID}/{partID}");

		public async Task<bool> DeletePartRangeInSeriesAsync(ulong seriesID, IEnumerable<PartID> partIDs)
			=> await _restClient.DeleteAsync($"{nameof(Part)}/{seriesID}?partIDs={string.Join(',', partIDs.Select(p => p.ToString()))}");

		public async Task<int> CountPartsAsync()
			=> await _restClient.GetAsync<int>($"{nameof(Part)}/count");
	}
}
