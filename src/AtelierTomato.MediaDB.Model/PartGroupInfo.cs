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
		public int? AverageLengthWords { get; set; }
		public MediaType MediaType { get; set; }
		public ReleaseType ReleaseType { get; set; }
	}
}
