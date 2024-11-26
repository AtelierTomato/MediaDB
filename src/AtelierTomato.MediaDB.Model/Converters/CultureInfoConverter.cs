using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtelierTomato.MediaDB.Model.Converters
{
	public class CultureInfoConverter : JsonConverter<CultureInfo>
	{
		public override CultureInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			// Just return a new CultureInfo based on the Name (e.g., "en-US")
			string? cultureName = reader.GetString();
			if (string.IsNullOrEmpty(cultureName))
			{
				throw new JsonException("Invalid or null culture name encountered.");
			}

			return new CultureInfo(cultureName);
		}

		public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
		{
			// Serialize only the Name (e.g., "en-US")
			writer.WriteStringValue(value.Name);
		}
	}
}
