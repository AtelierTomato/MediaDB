using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartNameAccess
	{
		Task WritePartName(PartName partName);
		Task WritePartNameRange(IEnumerable<PartName> partNameRange);
		Task<PartName?> ReadPartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script);
		Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesWithLanguage(ulong seriesID, CultureInfo language, ScriptType script);
		Task<IEnumerable<PartName>> ReadPartNameRangeForPart(ulong seriesID, PartID partID);
		Task<IEnumerable<PartName>> ReadPartNameRangeForSeries(ulong seriesID);
		Task<IEnumerable<PartName>> ReadAllPartNames();
		Task DeletePartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script);
		Task DeletePartNameRangeForPart(ulong seriesID, PartID partID);
		Task DeletePartNameRangeForSeries(ulong seriesID);
		Task<int> CountPartNames();
	}
}
