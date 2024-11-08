namespace AtelierTomato.MediaDB.Model
{
	public class Part
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; }
		public TimeSpan? LengthTime { get; set; }
		public int? LengthWords { get; set; }
		public DateTimeOffset? StartTime { get; set; }
		public DateTimeOffset? EndTime { get; set; }
		public Part(ulong seriesID, PartID partID, TimeSpan? lengthTime = null, int? lengthWords = null, DateTimeOffset? startTime = null, DateTimeOffset? endTime = null)
		{
			SeriesID = seriesID;
			PartID = partID;
			LengthTime = lengthTime;
			LengthWords = lengthWords;
			StartTime = startTime;
			EndTime = endTime;
		}
	}
}
