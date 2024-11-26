using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage
{
	public interface ISeriesAccess
	{
		Task<ulong> WriteNewSeries(Series series);
		Task<IEnumerable<ulong>> WriteNewSeriesRange(IEnumerable<Series> seriesRange);
		Task WriteSeries(Series series);
		Task WriteSeriesRange(IEnumerable<Series> seriesRange);
		Task<Series?> ReadSeries(ulong ID);
		Task<IEnumerable<Series>> ReadSeriesRange(IEnumerable<ulong> IDs);
		Task<IEnumerable<Series>> ReadAllSeries();
		Task DeleteSeries(ulong ID);
		Task<int> CountSeries();
	}
}
