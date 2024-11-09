using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class PartGroupInfoRow
	{
		public ulong SeriesID { get; set; }
		public string ParentPartID { get; set; }
		public string? AverageLengthTime { get; set; }
		public int? AverageLengthWords { get; set; }
		public string MediaType { get; set; }
		public string ReleaseType { get; set; }
		public PartGroupInfoRow(ulong seriesID, string parentPartID, string? averageLengthTime, int? averageLengthWords, string mediaType, string releaseType)
		{
			SeriesID = seriesID;
			ParentPartID = parentPartID;
			AverageLengthTime = averageLengthTime;
			AverageLengthWords = averageLengthWords;
			MediaType = mediaType;
			ReleaseType = releaseType;
		}
		public PartGroupInfoRow(PartGroupInfo partGroupInfo)
		{
			SeriesID = partGroupInfo.SeriesID;
			ParentPartID = partGroupInfo.ParentPartID?.ToString() ?? string.Empty;
			AverageLengthTime = partGroupInfo.AverageLengthTime?.ToString("c");
			AverageLengthWords = partGroupInfo.AverageLengthWords;
			MediaType = partGroupInfo.MediaType.ToString();
			ReleaseType = partGroupInfo.ReleaseType.ToString();
		}
		public PartGroupInfo ToPartGroupInfo()
		{
			PartID? parentPartID = null;
			if (!string.IsNullOrEmpty(ParentPartID))
			{
				parentPartID = PartID.Parse(ParentPartID);
			}
			TimeSpan? averageLengthTime = null;
			if (AverageLengthTime is not null)
			{
				TimeSpan.TryParseExact(AverageLengthTime, "c", null, out var result);
				averageLengthTime = result;
			}
			if (!Enum.TryParse<MediaType>(MediaType, out var mediaType))
			{
				throw new InvalidOperationException($"{MediaType} is not a valid type of {nameof(MediaDB.Model.MediaType)}.");
			}
			if (!Enum.TryParse<ReleaseType>(ReleaseType, out var releaseType))
			{
				throw new InvalidOperationException($"{ReleaseType} is not a valid type of {nameof(MediaDB.Model.MediaType)}.");
			}
			return new PartGroupInfo(SeriesID, parentPartID, averageLengthTime, AverageLengthWords, mediaType, releaseType);
		}
	}
}
