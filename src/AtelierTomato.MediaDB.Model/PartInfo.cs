using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class PartInfo
	{
		public ulong SeriesID { get; set; }
		public PartID PartID { get; set; }
		public CultureInfo Language { get; set; }
		public ScriptType Script { get; set; }
		public string Name { get; set; }
		public TimeSpan? LengthTime { get; set; }
		public int? LengthWords { get; set; }
		public PartInfo(ulong seriesID, PartID partID, CultureInfo language, ScriptType script, string name, TimeSpan? lengthTime = null, int? lengthWords = null)
		{
			SeriesID = seriesID;
			PartID = partID;
			Language = language;
			Script = script;
			Name = name;
			LengthTime = lengthTime;
			LengthWords = lengthWords;
		}
	}
}
