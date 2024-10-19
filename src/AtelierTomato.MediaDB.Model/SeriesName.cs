using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class SeriesName
	{
		public ulong ID { get; set; }
		public CultureInfo Language { get; set; }
		public ScriptType Script { get; set; }
		public string Name { get; set; }
		public SeriesName(ulong ID, CultureInfo language, ScriptType script, string name)
		{
			this.ID = ID;
			Language = language;
			Script = script;
			Name = name;
		}
	}
}
