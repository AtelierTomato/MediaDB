using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class PartNameRow
	{
		public ulong SeriesID { get; set; }
		public string PartID { get; set; }
		public string Language { get; set; }
		public string Script { get; set; }
		public string Name { get; set; }
		public PartNameRow(ulong seriesID, string partID, string language, string script, string name)
		{
			SeriesID = seriesID;
			PartID = partID;
			Language = language;
			Script = script;
			Name = name;
		}
		public PartNameRow(PartName partName)
		{
			SeriesID = partName.SeriesID;
			PartID = partName.PartID.ToString();
			Language = partName.Language.Name;
			Script = partName.Script.ToString();
			Name = partName.Name;
		}
		public PartName ToPartName()
		{
			if (!Enum.TryParse<ScriptType>(Script, out var script))
			{
				throw new InvalidOperationException($"{Script} is not a valid type of {nameof(ScriptType)}.");
			}
			return new PartName(SeriesID, MediaDB.Model.PartID.Parse(PartID), CultureInfo.GetCultureInfo(Language), script, Name);
		}
	}
}
