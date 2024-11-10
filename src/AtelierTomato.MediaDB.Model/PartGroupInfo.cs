namespace AtelierTomato.MediaDB.Model
{
	/// <summary>
	/// The general info of a group of parts.
	/// </summary>
	public class PartGroupInfo
	{
		public ulong SeriesID { get; set; }
		public PartID? ParentPartID { get; set; }
		public TimeSpan? AverageLengthTime { get; set; }
		public int? AverageLengthPages { get; set; }
		public MediaType MediaType { get; set; }
		public ReleaseType ReleaseType { get; set; }
		public PartGroupInfo(ulong seriesID, PartID? parentPartID, TimeSpan? averageLengthTime, int? averageLengthPages, MediaType mediaType, ReleaseType releaseType)
		{
			SeriesID = seriesID;
			ParentPartID = parentPartID;
			AverageLengthTime = averageLengthTime;
			AverageLengthPages = averageLengthPages;
			MediaType = mediaType;
			ReleaseType = releaseType;
		}
	}
}
