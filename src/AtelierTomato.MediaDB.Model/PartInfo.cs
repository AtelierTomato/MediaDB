namespace AtelierTomato.MediaDB.Model
{
	public class PartInfo
	{
		public ulong SeriesID { get; set; }
		public PartOID? ParentPartID { get; set; }
		public string PartName { get; set; }
		public PartInfo(ulong seriesID, PartOID? parentPartID, string partName)
		{
			SeriesID = seriesID;
			ParentPartID = parentPartID;
			PartName = partName;
		}
	}
}
