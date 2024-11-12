using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage
{
	public interface ISeriesParentAccess
	{
		Task WriteSeriesParent(SeriesParent seriesParent);
		Task WriteSeriesParentRange(IEnumerable<SeriesParent> seriesParentRange);
		Task<SeriesParent?> ReadSeriesParent(ulong ID, ulong parentID);
		Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeBySeries(ulong ID);
		Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeByParentSeries(ulong parentID);
		Task<IEnumerable<SeriesParent>> ReadAllSeriesParents();
		Task DeleteSeriesParent(ulong ID, ulong parentID);
		Task DeleteSeriesParentRangeBySeries(ulong ID);
		Task DeleteSeriesParentRangeByParentSeries(ulong parentID);
	}
}
