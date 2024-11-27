namespace AtelierTomato.MediaDB.Client.Panels.Models
{
	public record MediaDBStats(
		int Series,
		int SeriesNames,
		int SeriesParentPairs,
		int Parts,
		int PartNames,
		int PartGroupInfos,
		int PartGroupNames
	);
}