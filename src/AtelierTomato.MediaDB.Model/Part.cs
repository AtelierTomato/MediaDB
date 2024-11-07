namespace AtelierTomato.MediaDB.Model
{
	public class Part
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; }
		public TimeSpan? LengthTime { get; set; }
		public int? LengthWords { get; set; }
		public Part(ulong seriesID, PartID partID, TimeSpan? lengthTime, int? lengthWords)
		{
			SeriesID = seriesID;
			PartID = partID;
			LengthTime = lengthTime;
			LengthWords = lengthWords;
		}
	}
}
