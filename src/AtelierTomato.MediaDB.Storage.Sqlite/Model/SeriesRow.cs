using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class SeriesRow
	{
		public ulong ID { get; set; }
		public string OriginCountries { get; set; }
		public string? OriginLanguage { get; set; }
		public string? OriginScript { get; set; }
		public string? StartTime { get; set; }
		public string? EndTime { get; set; }
		public SeriesRow(ulong ID, string originCountries, string? originLanguage, string? originScript, string? startTime, string? endTime)
		{
			this.ID = ID;
			OriginCountries = originCountries;
			OriginLanguage = originLanguage;
			OriginScript = originScript;
			StartTime = startTime;
			EndTime = endTime;
		}
		public SeriesRow(Series series)
		{
			ID = series.ID;
			OriginCountries = string.Join(' ', series.OriginCountries.Select(c => c.Name));
			OriginLanguage = series.OriginLanguage?.Name;
			OriginScript = series.OriginScript?.ToString();
			StartTime = series.StartTime?.ToString("o");
			EndTime = series.EndTime?.ToString("o");
		}
		public Series ToSeries()
		{
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
				DateTimeOffset.TryParseExact(StartTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
				startTime = result;
			}
			if (EndTime is not null)
			{
				DateTimeOffset.TryParseExact(EndTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
				endTime = result;
			}
			return new Series(ID, originCountries, originLanguage, originScript, startTime, endTime);
		}
	}
}
