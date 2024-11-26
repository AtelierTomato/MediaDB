using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage
{
	public interface IPartAccess
	{
		Task WritePart(Part part);
		Task WritePartRange(IEnumerable<Part> partRange);
		Task<Part?> ReadPart(ulong seriesID, PartID partID);
		Task<IEnumerable<Part>> ReadPartRangeBySeries(ulong seriesID);
		Task<IEnumerable<Part>> ReadPartRangeBySeriesRange(IEnumerable<ulong> seriesIDRange);
		Task<IEnumerable<Part>> ReadAllParts();
		Task DeletePart(ulong seriesID, PartID partID);
		Task DeletePartRangeInSeries(ulong seriesID, IEnumerable<PartID> partIDRange);
		Task<int> CountParts();
		Task<IEnumerable<Part>> SearchPartByName(string name);
	}
}
