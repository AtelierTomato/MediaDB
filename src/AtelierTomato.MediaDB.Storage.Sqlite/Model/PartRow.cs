using AtelierTomato.MediaDB.Model;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class PartRow
	{
		public ulong SeriesID { get; set; }
		public string PartID { get; set; } = string.Empty;
		public string? LengthTime { get; set; }
		public int? LengthPages { get; set; }
		public string? StartTime { get; set; }
		public string? EndTime { get; set; }
		public PartRow() { }
		public PartRow(ulong seriesID, string partID, string? lengthTime, int? lengthPages, string? startTime, string? endTime)
		{
			SeriesID = seriesID;
			PartID = partID;
			LengthTime = lengthTime;
			LengthPages = lengthPages;
			StartTime = startTime;
			EndTime = endTime;
		}
		public PartRow(Part part)
		{
			SeriesID = part.SeriesID;
			PartID = part.PartID.ToString();
			LengthTime = part.LengthTime?.ToString("c");
			LengthPages = part.LengthPages;
			StartTime = part.StartTime?.ToString("o");
			EndTime = part.EndTime?.ToString("o");
		}
		public Part ToPart()
		{
			TimeSpan? lengthTime = null;
			DateTimeOffset? startTime = null, endTime = null;
			if (LengthTime is not null)
			{
				TimeSpan.TryParse(LengthTime, out var result);
				lengthTime = result;
			}
			if (StartTime is not null)
			{
				DateTimeOffset.TryParse(StartTime, out var result);
				startTime = result;
			}
			if (EndTime is not null)
			{
				DateTimeOffset.TryParse(EndTime, out var result);
				endTime = result;
			}
			return new Part(SeriesID, MediaDB.Model.PartID.Parse(PartID), lengthTime, LengthPages, startTime, endTime);
		}
	}
}
