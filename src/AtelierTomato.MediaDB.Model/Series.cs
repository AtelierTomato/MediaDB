using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class Series
	{
		public ulong ID { get; set; }
		public IReadOnlyList<RegionInfo> OriginCountries { get; set; } = [];
		public CultureInfo? OriginLanguage { get; set; } = null;
		public ScriptType? OriginScript { get; set; } = null;
		public DateTimeOffset? StartTime { get; set; }
		public DateTimeOffset? EndTime { get; set; }
		public Series() { }
		public Series(ulong ID, IReadOnlyList<RegionInfo>? originCountries = null, CultureInfo? originLanguage = null, ScriptType? originScript = null, DateTimeOffset? startTime = null, DateTimeOffset? endTime = null)
		{
			this.ID = ID;
			OriginCountries = originCountries ?? [];
			OriginLanguage = originLanguage;
			OriginScript = originScript;
			StartTime = startTime;
			EndTime = endTime;
		}
	}
}
