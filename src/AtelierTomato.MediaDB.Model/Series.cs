using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class Series
	{
		public ulong ID { get; set; }
		public MediaType MediaType { get; set; } = MediaType.Unknown;
		public ReleaseType ReleaseType { get; set; } = ReleaseType.Unknown;
		public IReadOnlyList<RegionInfo> OriginCountries { get; set; } = [];
		public CultureInfo? OriginLanguage { get; set; } = null;
		public ScriptType? OriginScript { get; set; } = null;
		public DateTimeOffset? StartTime { get; set; }
		public DateTimeOffset? EndTime { get; set; }
		public Series() { }
		public Series(ulong ID, MediaType mediaType, ReleaseType releaseType, IReadOnlyList<RegionInfo>? originCountries = null, CultureInfo? originLanguage = null, ScriptType? originScript = null, DateTimeOffset? startTime = null, DateTimeOffset? endTime = null)
		{
			this.ID = ID;
			MediaType = mediaType;
			ReleaseType = releaseType;
			OriginCountries = originCountries ?? [];
			OriginLanguage = originLanguage;
			OriginScript = originScript;
			StartTime = startTime;
			EndTime = endTime;
		}
	}
}
