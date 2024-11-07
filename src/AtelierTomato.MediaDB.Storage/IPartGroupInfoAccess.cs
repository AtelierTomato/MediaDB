using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartGroupInfoAccess
	{
		Task WritePartGroupInfo(PartGroupInfo partGroupInfo);
		Task WritePartGroupInfoRange(IEnumerable<PartGroupInfo> partGroupInfoRange);
		Task<PartGroupInfo?> ReadPartGroupInfo(ulong SeriesID, PartID? ParentPartID);
		Task<IEnumerable<PartGroupInfo>> ReadPartGroupInfoRangeForSeries(ulong SeriesID);
		Task<IEnumerable<PartGroupInfo>> ReadAllPartGroupInfos();
		Task DeletePartGroupInfo(ulong SeriesID, PartID? ParentPartID);
		Task DeletePartGroupInfoRangeForSeries(ulong SeriesID);
	}
}
