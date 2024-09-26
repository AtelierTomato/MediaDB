using System.Text;

namespace AtelierTomato.MediaDB.Model
{
	public class PartOID
	{
		public int Number { get; set; }
		public PartOID? ParentPartOID { get; set; } = null;
		public PartOID(int number, PartOID? parentPartOID = null)
		{
			Number = number;
			ParentPartOID = parentPartOID;
		}
		public override string ToString()
		{
			StringBuilder sb = new();
			if (ParentPartOID is not null)
			{
				sb.Append(ParentPartOID.ToString() + '.');
			}
			sb.Append(Number);
			return sb.ToString();
		}
		public static PartOID Parse(string input)
		{
			var numbers = input.Split('.').Select(n => int.TryParse(n, out int result) ? result :
				throw new ArgumentException($"{nameof(PartOID)} failed to parse as '{n}' is not a valid integer value.", nameof(input))
			);
			return Parse(numbers);
		}
		public static PartOID Parse(IEnumerable<int> input)
		{
			if (!input.Any())
				throw new ArgumentException($"{nameof(PartOID)} failed to parse as {nameof(input)} is empty.", nameof(input));
			PartOID? partOID = null;
			foreach (var number in input)
			{
				partOID = new(number, partOID);
			}
			return partOID!;
		}
	}
}
