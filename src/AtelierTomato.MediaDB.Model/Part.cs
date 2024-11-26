namespace AtelierTomato.MediaDB.Model
{
	public class Part
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; } = new(0);
		public TimeSpan? LengthTime { get; set; }
		public int? LengthPages { get; set; }
		public DateTimeOffset? StartTime { get; set; }
		public DateTimeOffset? EndTime { get; set; }
		public Part() { }
		public Part(ulong seriesID, PartID partID, TimeSpan? lengthTime = null, int? lengthPages = null, DateTimeOffset? startTime = null, DateTimeOffset? endTime = null)
		{
			SeriesID = seriesID;
			PartID = partID;
			LengthTime = lengthTime;
			LengthPages = lengthPages;
			StartTime = startTime;
			EndTime = endTime;
		}
	}
}
