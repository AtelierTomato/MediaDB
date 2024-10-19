using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class PartName
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; }
		public CultureInfo Language { get; set; }
		public ScriptType Script { get; set; }
		public string Name { get; set; }
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
