using AtelierTomato.MediaDB.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtelierTomato.MediaDB.API.Converters
{
	public class PartIDConverter : JsonConverter<PartID>
	{
		public override PartID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			// Just return a new PartID parsed from a string
			string? partIDName = reader.GetString();
			if (string.IsNullOrEmpty(partIDName))
			{
				throw new JsonException("Invalid or null region name encountered.");
			}

			return PartID.Parse(partIDName);
		}

		public override void Write(Utf8JsonWriter writer, PartID value, JsonSerializerOptions options)
		{
			// Serialize only the .ToString() value
			writer.WriteStringValue(value.ToString());
		}
	}
}
