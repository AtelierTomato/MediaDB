using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.APIClient
{
	public class PartGroupInfoClient
	{
		private readonly RestClient _restClient;
		public PartGroupInfoClient(RestClient restClient)
		{
			_restClient = restClient;
		}

		public async Task<bool> WritePartGroupInfoAsync(PartGroupInfo partGroupInfo)
			=> await _restClient.PostAsync(nameof(PartGroupInfo), partGroupInfo);

		public async Task<bool> WritePartGroupInfoRangeAsync(IEnumerable<PartGroupInfo> partGroupInfoRange)
			=> await _restClient.PostAsync($"{nameof(PartGroupInfo)}/range", partGroupInfoRange);

		public async Task<PartGroupInfo?> ReadPartGroupInfoAsync(ulong seriesID, PartID? parentPartID)
			=> await _restClient.GetAsync<PartGroupInfo?>($"{nameof(PartGroupInfo)}/{seriesID}/{parentPartID?.ToString() ?? null}");

		public async Task<IEnumerable<PartGroupInfo>> ReadPartGroupInfoRangeForSeriesAsync(ulong seriesID)
			=> await _restClient.GetAsync<IEnumerable<PartGroupInfo>>($"{nameof(PartGroupInfo)}/{seriesID}");

		public async Task<IEnumerable<PartGroupInfo>> ReadAllPartGroupInfos()
			=> await _restClient.GetAsync<IEnumerable<PartGroupInfo>>($"{nameof(PartGroupInfo)}/all");

		public async Task<bool> DeletePartGroupInfoAsync(ulong seriesID, PartID? parentPartID)
			=> await _restClient.DeleteAsync($"{nameof(PartGroupInfo)}/{seriesID}/{parentPartID?.ToString() ?? null}");

		public async Task<bool> DeletePartGroupInfoRangeForSeriesAsync(ulong seriesID)
			=> await _restClient.DeleteAsync($"{nameof(PartGroupInfo)}/{seriesID}");

		public async Task<int> CountPartGroupInfoAsync()
			=> await _restClient.GetAsync<int>($"{nameof(PartGroupInfo)}/count");
	}
}
