using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class PartRow
	{
		public ulong SeriesID { get; set; }
		public string PartID { get; set; }
		public string? LengthTime { get; set; }
		public int? LengthWords { get; set; }
		public string? StartTime { get; set; }
		public string? EndTime { get; set; }
		public PartRow(ulong seriesID, string partID, string? lengthTime, int? lengthWords, string? startTime, string? endTime)
		{
			SeriesID = seriesID;
			PartID = partID;
			LengthTime = lengthTime;
			LengthWords = lengthWords;
			StartTime = startTime;
			EndTime = endTime;
		}
		public PartRow(Part part)
		{
			SeriesID = part.SeriesID;
			PartID = part.PartID.ToString();
			LengthTime = part.LengthTime?.ToString("c");
			LengthWords = part.LengthWords;
			StartTime = part.StartTime?.ToString("o");
			EndTime = part.EndTime?.ToString("o");
		}
		public Part ToPart()
		{
			TimeSpan? lengthTime = null;
			DateTimeOffset? startTime = null, endTime = null;
			if (LengthTime is not null)
			{
				TimeSpan.TryParseExact(LengthTime, "c", null, out var result);
				lengthTime = result;
			}
			if (StartTime is not null)
			{
				DateTimeOffset.TryParseExact(StartTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
				startTime = result;
			}
			if (EndTime is not null)
			{
				DateTimeOffset.TryParseExact(EndTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
				endTime = result;
			}
			return new Part(SeriesID, MediaDB.Model.PartID.Parse(PartID), lengthTime, LengthWords, startTime, endTime);
		}
	}
}
