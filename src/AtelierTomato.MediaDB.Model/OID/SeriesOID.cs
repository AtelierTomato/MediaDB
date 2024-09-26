using System.Text;

namespace AtelierTomato.MediaDB.Model.ID
{
	public class SeriesOID
	{
		public ulong ID { get; set; }
		public SeriesOID? ParentSeriesOID { get; set; } = null;
		public SeriesOID(ulong ID, SeriesOID? parentSeriesOID = null)
		{
			this.ID = ID;
			ParentSeriesOID = parentSeriesOID;
		}
		public override string ToString()
		{
			StringBuilder sb = new();
			if (ParentSeriesOID is not null)
			{
				sb.Append(ParentSeriesOID.ToString() + '.');
			}
			sb.Append(ID);
			return sb.ToString();
		}
		public static SeriesOID Parse(string input)
		{
			var IDs = input.Split('.').Select(i => ulong.TryParse(i, out ulong result) ? result :
				throw new ArgumentException($"{nameof(SeriesOID)} failed to parse as '{i}' is not a valid ulong value.", nameof(input))
			);
			return Parse(IDs);
		}
		public static SeriesOID Parse(IEnumerable<ulong> input)
		{
			if (!input.Any())
				throw new ArgumentException($"{nameof(SeriesOID)} failed to parse as {nameof(input)} is empty.", nameof(input));
			SeriesOID? seriesOID = null;
			foreach (var ID in input)
			{
				seriesOID = new(ID, seriesOID);
			}
			return seriesOID!;
		}
	}
}
