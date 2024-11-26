using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.APIClient
{
	public class SeriesParentClient
	{
		private readonly RestClient _restClient;
		public SeriesParentClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<bool> WriteSeriesParentAsync(SeriesParent seriesParent)
			=> await _restClient.PostAsync(nameof(SeriesParent), seriesParent);

		public async Task<bool> WriteSeriesParentRangeAsync(IEnumerable<SeriesParent> seriesParentRange)
			=> await _restClient.PostAsync($"{nameof(SeriesParent)}/range", seriesParentRange);

		public async Task<SeriesParent?> ReadSeriesParentAsync(ulong ID, ulong parentID)
			=> await _restClient.GetAsync<SeriesParent?>($"{nameof(SeriesParent)}/{ID}/{parentID}");

		public async Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeBySeriesAsync(ulong ID)
			=> await _restClient.GetAsync<IEnumerable<SeriesParent>>($"{nameof(SeriesParent)}/byseries/{ID}");

		public async Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeByParentSeriesAsync(ulong parentID)
			=> await _restClient.GetAsync<IEnumerable<SeriesParent>>($"{nameof(SeriesParent)}/byparent/{parentID}");

		public async Task<IEnumerable<SeriesParent>> ReadAllSeriesParentsAsync()
			=> await _restClient.GetAsync<IEnumerable<SeriesParent>>($"{nameof(SeriesParent)}/all");

		public async Task<bool> DeleteSeriesParentAsync(ulong ID, ulong parentID)
			=> await _restClient.DeleteAsync($"{nameof(SeriesParent)}/{ID}/{parentID}");

		public async Task<bool> DeleteSeriesParentRangeBySeriesAsync(ulong ID)
			=> await _restClient.DeleteAsync($"{nameof(SeriesParent)}/byseries/{ID}");

		public async Task<bool> DeleteSeriesParentRangeByParentSeriesAsync(ulong parentID)
			=> await _restClient.DeleteAsync($"{nameof(SeriesParent)}/byparent/{parentID}");

		public async Task<int> CountSeriesParentsAsync()
			=> await _restClient.GetAsync<int>($"{nameof(SeriesParent)}/count");
	}
}
