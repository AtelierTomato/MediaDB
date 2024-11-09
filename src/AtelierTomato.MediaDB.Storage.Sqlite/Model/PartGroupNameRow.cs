using AtelierTomato.MediaDB.Model;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite.Model
{
	public class PartGroupNameRow
	{
		public ulong SeriesID { get; set; }
		public string ParentPartID { get; set; }
		public string Language { get; set; }
		public string Script { get; set; }
		public string Name { get; set; }
		public PartGroupNameRow(ulong seriesID, string parentPartID, string language, string script, string name)
		{
			SeriesID = seriesID;
			ParentPartID = parentPartID;
			Language = language;
			Script = script;
			Name = name;
		}
		public PartGroupNameRow(PartGroupName partGroupName)
		{
			SeriesID = partGroupName.SeriesID;
			ParentPartID = partGroupName.ParentPartID?.ToString() ?? string.Empty;
			Language = partGroupName.Language.Name;
			Script = partGroupName.Script.ToString();
			Name = partGroupName.Name;
		}
		public PartGroupName ToPartGroupName()
		{
			PartID? parentPartID = null;
			if (!string.IsNullOrEmpty(ParentPartID))
			{
				parentPartID = PartID.Parse(ParentPartID);
			}
			if (!Enum.TryParse<ScriptType>(Script, out var script))
			{
				throw new InvalidOperationException($"{Script} is not a valid type of {nameof(ScriptType)}.");
			}
			return new PartGroupName(SeriesID, parentPartID, CultureInfo.GetCultureInfo(Language), script, Name);
		}
	}
}
