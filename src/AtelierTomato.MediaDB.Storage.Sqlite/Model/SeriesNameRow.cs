using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class SeriesNameRow
	{
		public ulong ID { get; set; }
		public string Language { get; set; }
		public string Script { get; set; }
		public string Name { get; set; }
		public SeriesNameRow(ulong ID, string language, string script, string name)
		{
			this.ID = ID;
			Language = language;
			Script = script;
			Name = name;
		}
		public SeriesNameRow(SeriesName seriesName)
		{
			ID = seriesName.ID;
			Language = seriesName.Language.Name;
			Script = seriesName.Script.ToString();
			Name = seriesName.Name;
		}
		public SeriesName ToSeriesName()
		{
			if (!Enum.TryParse<ScriptType>(Script, out var script))
			{
				throw new InvalidOperationException($"{Script} is not a valid type of {nameof(ScriptType)}.");
			}
			return new SeriesName(ID, CultureInfo.GetCultureInfo(Language), script, Name);
		}
	}
}
