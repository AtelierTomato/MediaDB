using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartGroupInfoAccess
	{
		Task WritePartGroupInfo(PartGroupInfo partGroupInfo);
		Task WritePartGroupInfoRange(IEnumerable<PartGroupInfo> partGroupInfoRange);
		Task<PartGroupInfo?> ReadPartGroupInfo(ulong seriesID, PartID? parentPartID);
		Task<IEnumerable<PartGroupInfo>> ReadPartGroupInfoRangeForSeries(ulong seriesID);
		Task<IEnumerable<PartGroupInfo>> ReadAllPartGroupInfos();
		Task DeletePartGroupInfo(ulong seriesID, PartID? parentPartID);
		Task DeletePartGroupInfoRangeForSeries(ulong seriesID);
		Task<int> CountPartGroupInfo();
	}
}
