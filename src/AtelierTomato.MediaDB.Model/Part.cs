namespace AtelierTomato.MediaDB.Model
{
	public class Part
	{
		public ulong SeriesID { get; set; }
		public PartOID PartID { get; set; }
		public string Name { get; set; }
		public Part(ulong seriesID, PartOID partID, string name)
		{
			SeriesID = seriesID;
			PartID = partID;
			Name = name;
		}
	}
}
