using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartNameAccess
	{
		Task WritePartName(PartName partName);
		Task WritePartNameRange(IEnumerable<PartName> partNameRange);
		Task<PartName?> ReadPartName(ulong SeriesID, PartID PartID, CultureInfo Language, ScriptType Script);
		Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesWithLanguage(ulong SeriesID, CultureInfo Language, ScriptType Script);
		Task<IEnumerable<PartName>> ReadPartNameRangeForPart(ulong SeriesID, PartID PartID);
		Task<IEnumerable<PartName>> ReadPartNameRangeForSeries(ulong SeriesID);
		Task<IEnumerable<PartName>> ReadAllPartNames();
		Task DeletePartName(ulong SeriesID, PartID PartID, CultureInfo Language, ScriptType Script);
		Task DeletePartNameRangeForPart(ulong SeriesID, PartID PartID);
		Task DeletePartNameRangeForSeries(ulong SeriesID);
	}
}
