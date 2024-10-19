using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class Series
	{
		public ulong ID { get; set; }
		public string Name { get; set; }
		public IReadOnlyList<RegionInfo> OriginCountries { get; set; } = [];
		public CultureInfo? OriginLanguage { get; set; } = null;
		public Series(ulong ID, string name, IReadOnlyList<RegionInfo>? originCountries = null, CultureInfo? originLanguage = null)
		{
			this.ID = ID;
			Name = name;
			OriginCountries = originCountries ?? [];
			OriginLanguage = originLanguage;
		}
	}
}
