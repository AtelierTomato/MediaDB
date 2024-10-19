using System.Text;

namespace AtelierTomato.MediaDB.Model
{
	public class PartID
	{
		public int Number { get; set; }
		public PartID? ParentPartID { get; set; } = null;
		public PartID(int number, PartID? parentPartID = null)
		{
			Number = number;
			ParentPartID = parentPartID;
		}
		public override string ToString()
		{
			StringBuilder sb = new();
			if (ParentPartID is not null)
			{
				sb.Append(ParentPartID.ToString() + '.');
			}
			sb.Append(Number);
			return sb.ToString();
		}
		public static PartID Parse(string input)
		{
			var numbers = input.Split('.').Select(n => int.TryParse(n, out int result) ? result :
				throw new ArgumentException($"{nameof(PartID)} failed to parse as '{n}' is not a valid integer value.", nameof(input))
			);
			return Parse(numbers);
		}
		public static PartID Parse(IEnumerable<int> input)
		{
			if (!input.Any())
				throw new ArgumentException($"{nameof(PartID)} failed to parse as {nameof(input)} is empty.", nameof(input));
			PartID? partID = null;
			foreach (var number in input)
			{
				partID = new(number, partID);
			}
			return partID!;
		}
		public int Depth()
		{
			int depth = 1;
			if (ParentPartID is not null)
			{
				depth += ParentPartID.Depth();
			}
			return depth;
		}
	}
}
