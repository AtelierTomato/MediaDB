using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	/// <summary>
	/// Refers to the general name of a group of parts. Example: 'Series', 'Episode', et cetera.
	/// </summary>
	public class PartGroupName
	{
		public ulong SeriesID { get; set; }
		public PartID? ParentPartID { get; set; }
		public CultureInfo Language { get; set; }
		public ScriptType Script { get; set; }
		public string Name { get; set; }

		public PartGroupName(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script, string partName)
		{
			SeriesID = seriesID;
			ParentPartID = parentPartID;
			Language = language;
			Script = script;
			Name = partName;
		}
	}
}
