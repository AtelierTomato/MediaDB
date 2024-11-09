using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartGroupNameAccess
	{
		Task WritePartGroupName(PartGroupName partGroupName);
		Task WritePartGroupNameRange(IEnumerable<PartGroupName> partGroupNameRange);
		Task<PartGroupName?> ReadPartGroupName(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script);
		Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesWithLanguage(ulong seriesID, CultureInfo language, ScriptType script);
		Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForParentPart(ulong seriesID, PartID? parentPartID);
		Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeries(ulong seriesID);
		Task<IEnumerable<PartGroupName>> ReadAllPartGroupNames();
		Task DeletePartGroupName(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script);
		Task DeletePartGroupNameRangeForPart(ulong seriesID, PartID? parentPartID);
		Task DeletePartGroupNameRangeForSeries(ulong seriesID);
	}
}
