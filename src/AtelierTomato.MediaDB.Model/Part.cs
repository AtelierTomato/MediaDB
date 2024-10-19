namespace AtelierTomato.MediaDB.Model
{
	public class Part
	{
		public ulong SeriesID { get; set; }
		public PartOID PartID { get; set; }
		public Part(ulong seriesID, PartOID partID)
		{
			SeriesID = seriesID;
			PartID = partID;
		}
	}
}
