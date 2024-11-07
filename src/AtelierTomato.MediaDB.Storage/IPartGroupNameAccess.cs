using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartGroupNameAccess
	{
		Task WritePartGroupName(PartGroupName partGroupName);
		Task WritePartGroupNameRange(IEnumerable<PartGroupName> partGroupNameRange);
		Task<PartGroupName?> ReadPartGroupName(ulong SeriesID, PartID? ParentPartID, CultureInfo Language, ScriptType Script);
		Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesWithLanguage(ulong SeriesID, CultureInfo Language, ScriptType Script);
		Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForParentPart(ulong SeriesID, PartID? ParentPartID);
		Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeries(ulong SeriesID);
		Task<IEnumerable<PartGroupName>> ReadAllPartGroupNames();
		Task DeletePartGroupName(ulong SeriesID, PartID? ParentPartID, CultureInfo Language, ScriptType Script);
		Task DeletePartGroupNameRangeForPart(ulong SeriesID, PartID? ParentPartID);
		Task DeletePartGroupNameRangeForSeries(ulong SeriesID);
	}
}
