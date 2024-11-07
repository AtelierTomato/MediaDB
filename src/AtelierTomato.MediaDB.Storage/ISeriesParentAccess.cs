using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage
{
	public interface ISeriesParentAccess
	{
		Task WriteSeriesParent(SeriesParent seriesParent);
		Task WriteSeriesParentRange(IEnumerable<SeriesParent> seriesParentRange);
		Task<SeriesParent?> ReadSeriesParent(ulong ID, ulong ParentID);
		Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeBySeries(ulong ID);
		Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeByParentSeries(ulong ParentID);
		Task<IEnumerable<SeriesParent>> ReadAllSeriesParents();
		Task DeleteSeriesParent(ulong ID, ulong ParentID);
		Task DeleteSeriesParentRangeBySeries(ulong ID);
		Task DeleteSeriesParentRangeByParentSeries(ulong ParentID);
	}
}
