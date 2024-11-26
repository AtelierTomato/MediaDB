using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtelierTomato.MediaDB.Model.Converters
{
	public class RegionInfoConverter : JsonConverter<RegionInfo>
	{
		public override RegionInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			// Just return a new RegionInfo based on the Name (e.g., "US")
			string? regionName = reader.GetString();
			if (string.IsNullOrEmpty(regionName))
			{
				throw new JsonException("Invalid or null region name encountered.");
			}

			return new RegionInfo(regionName);
		}

		public override void Write(Utf8JsonWriter writer, RegionInfo value, JsonSerializerOptions options)
		{
			// Serialize only the Name (e.g., "US")
			writer.WriteStringValue(value.Name);
		}
	}
}
