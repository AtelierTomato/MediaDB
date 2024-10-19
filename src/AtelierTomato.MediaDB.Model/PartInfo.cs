﻿using System.Globalization;

namespace AtelierTomato.MediaDB.Model
{
	public class PartInfo
	{
		public ulong SeriesID { get; set; }
		public PartOID? ParentPartID { get; set; }
		public CultureInfo Language { get; set; }
		public ScriptType Script { get; set; }
		public string PartName { get; set; }
		public PartInfo(ulong seriesID, PartOID? parentPartID, CultureInfo language, ScriptType script, string partName)
		{
			SeriesID = seriesID;
			ParentPartID = parentPartID;
			Language = language;
			Script = script;
			PartName = partName;
		}
	}
}
