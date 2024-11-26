using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class SeriesRow
	{
		public ulong ID { get; set; }
		public string MediaType { get; set; } = "Unknown";
		public string ReleaseType { get; set; } = "Unknown";
		public string OriginCountries { get; set; } = string.Empty;
		public string? OriginLanguage { get; set; }
		public string? OriginScript { get; set; }
		public string? StartTime { get; set; }
		public string? EndTime { get; set; }
		public SeriesRow() { }
		public SeriesRow(ulong ID, string mediaType, string releaseType, string originCountries, string? originLanguage, string? originScript, string? startTime, string? endTime)
		{
			this.ID = ID;
			MediaType = mediaType;
			ReleaseType = releaseType;
			OriginCountries = originCountries;
			OriginLanguage = originLanguage;
			OriginScript = originScript;
			StartTime = startTime;
			EndTime = endTime;
		}
		public SeriesRow(Series series)
		{
			ID = series.ID;
			MediaType = series.MediaType.ToString();
			ReleaseType = series.ReleaseType.ToString();
			OriginCountries = string.Join(' ', series.OriginCountries.Select(c => c.Name));
			OriginLanguage = series.OriginLanguage?.Name;
			OriginScript = series.OriginScript?.ToString();
			StartTime = series.StartTime?.ToString("o");
			EndTime = series.EndTime?.ToString("o");
		}
		public Series ToSeries()
		{
			if (!Enum.TryParse<MediaType>(MediaType, out var mediaType))
			{
				throw new InvalidOperationException($"{MediaType} is not a valid type of {nameof(MediaType)}.");
			}
			if (!Enum.TryParse<ReleaseType>(ReleaseType, out var releaseType))
			{
				throw new InvalidOperationException($"{ReleaseType} is not a valid type of {nameof(MediaType)}.");
			}
			IReadOnlyList<RegionInfo> originCountries = OriginCountries.Split(' ').Select(c => new RegionInfo(c)).ToList();
			CultureInfo? originLanguage = null;
			if (OriginLanguage is not null)
			{
				originLanguage = CultureInfo.GetCultureInfo(OriginLanguage);
			}
			ScriptType? originScript = null;
			if (OriginScript is not null)
			{
				if (!Enum.TryParse<ScriptType>(OriginScript, out var result))
				{
					throw new InvalidOperationException($"{OriginScript} is not a valid type of {nameof(ScriptType)}.");
				}
				originScript = result;
			}
			DateTimeOffset? startTime = null, endTime = null;
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
			return new Series(ID, mediaType, releaseType, originCountries, originLanguage, originScript, startTime, endTime);
		}
	}
}
