namespace AtelierTomato.MediaDB.Model
{
	public class Part
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; }
		public Part(ulong seriesID, PartID partID)
		{
			SeriesID = seriesID;
			PartID = partID;
		}
	}
}
