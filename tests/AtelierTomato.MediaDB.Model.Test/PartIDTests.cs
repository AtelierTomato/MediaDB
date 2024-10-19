using FluentAssertions;

namespace AtelierTomato.MediaDB.Model.Test
{
	public class PartIDTests
	{
		[Fact]
		public void PartIDParseTest()
		{
			var partIDString = "1.2.3";
			var partID = PartID.Parse(partIDString);
			partID.Should().BeEquivalentTo(new PartID(3, new PartID(2, new PartID(1))));
		}
		[Fact]
		public void PartIDToStringTest()
		{
			var partID = new PartID(3, new PartID(2, new PartID(1)));
			var partIDString = partID.ToString();
			partIDString.Should().Be("1.2.3");
		}
		[Fact]
		public void PartIDNotIntegerTest()
		{
			Action act = () => PartID.Parse("3.a.1");
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(PartID)} failed to parse as 'a' is not a valid integer value. (Parameter 'input')");
		}
		[Fact]
		public void PartIDEmptyTest()
		{
			Action act = () => PartID.Parse([]);
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(PartID)} failed to parse as input is empty. (Parameter 'input')");
		}
		[Theory]
		[InlineData("1", 1)]
		[InlineData("1.2", 2)]
		[InlineData("1.2.3", 3)]
		[InlineData("1.2.3.4", 4)]
		public void DepthTests(string input, int output)
		{
			var partID = PartID.Parse(input);
			partID.Depth().Should().Be(output);
		}
	}
}