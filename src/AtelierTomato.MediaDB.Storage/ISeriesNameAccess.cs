using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage
{
	public interface ISeriesNameAccess
	{
		Task WriteSeriesName(SeriesName SeriesName);
		Task WriteSeriesNameRange(IEnumerable<SeriesName> SeriesNameRange);
		Task<SeriesName?> ReadSeriesName(ulong ID, CultureInfo Language, ScriptType Script);
		Task<IEnumerable<SeriesName>> ReadSeriesNameRange(IEnumerable<ulong> IDs, CultureInfo Language, ScriptType Script);
		Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeries(ulong ID);
		Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesPlural(IEnumerable<ulong> IDs);
		Task<IEnumerable<SeriesName>> ReadAllSeriesNames();
		Task DeleteSeriesName(ulong ID, CultureInfo Language, ScriptType Script);
		Task DeleteSeriesNameRangeForSeries(ulong ID);
		Task DeleteSeriesNameRangeForSeriesPlural(IEnumerable<ulong> ID);
	}
}
