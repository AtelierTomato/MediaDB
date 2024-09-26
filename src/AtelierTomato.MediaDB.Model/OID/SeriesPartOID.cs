using System.Text;

namespace AtelierTomato.MediaDB.Model.ID
{
	public class SeriesPartOID
	{
		public SeriesOID SeriesOID { get; set; }
		public PartOID PartOID { get; set; }
		public SeriesPartOID(SeriesOID series, PartOID part)
		{
			SeriesOID = series;
			PartOID = part;
		}
		public override string ToString()
		{
			StringBuilder sb = new();
			sb.Append(SeriesOID.ToString());
			sb.Append(':');
			sb.Append(PartOID.ToString());
			return sb.ToString();
		}
		public static SeriesPartOID Parse(string input)
		{
			var tokenizedInput = input.Split(':');
			if (tokenizedInput.Length is not 2)
				throw new ArgumentException($"{nameof(SeriesPartOID)} could not be split from '{input}' on character ':' into exactly two strings.", nameof(input));
			return Parse(tokenizedInput[0], tokenizedInput[1]);
		}
		public static SeriesPartOID Parse(string seriesOIDStr, string partOIDStr) => new(SeriesOID.Parse(seriesOIDStr), PartOID.Parse(partOIDStr));
	}
}
