using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage
{
	public interface ISeriesNameAccess
	{
		Task WriteSeriesName(SeriesName seriesName);
		Task WriteSeriesNameRange(IEnumerable<SeriesName> seriesNameRange);
		Task<SeriesName?> ReadSeriesName(ulong ID, CultureInfo language, ScriptType script);
		Task<IEnumerable<SeriesName>> ReadSeriesNameRange(IEnumerable<ulong> IDs, CultureInfo language, ScriptType script);
		Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeries(ulong ID);
		Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesPlural(IEnumerable<ulong> IDs);
		Task<IEnumerable<SeriesName>> ReadAllSeriesNames();
		Task DeleteSeriesName(ulong ID, CultureInfo language, ScriptType script);
		Task DeleteSeriesNameRangeForSeries(ulong ID);
	}
}
