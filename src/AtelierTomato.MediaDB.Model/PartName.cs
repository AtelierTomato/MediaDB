using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class PartName
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; } = new(0);
		public CultureInfo Language { get; set; } = CultureInfo.InvariantCulture;
		public ScriptType Script { get; set; }
		public string Name { get; set; } = string.Empty;
		public PartName() { }
		public PartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script, string name)
		{
			SeriesID = seriesID;
			PartID = partID;
			Language = language;
			Script = script;
			Name = name;
		}
	}
}
