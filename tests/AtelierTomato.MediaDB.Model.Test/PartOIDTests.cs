using FluentAssertions;

namespace AtelierTomato.MediaDB.Model.Test
{
	public class PartOIDTests
	{
		[Fact]
		public void PartOIDParseTest()
		{
			var partOIDString = "1.2.3";
			var partOID = PartOID.Parse(partOIDString);
			partOID.Should().BeEquivalentTo(new PartOID(3, new PartOID(2, new PartOID(1))));
		}
		[Fact]
		public void SeriesPartOIDToStringTest()
		{
			var partOID = new PartOID(3, new PartOID(2, new PartOID(1)));
			var partOIDString = partOID.ToString();
			partOIDString.Should().Be("1.2.3");
		}
		[Fact]
		public void PartOIDNotIntegerTest()
		{
			Action act = () => PartOID.Parse("3.a.1");
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(PartOID)} failed to parse as 'a' is not a valid integer value. (Parameter 'input')");
		}
		[Fact]
		public void PartOIDEmptyTest()
		{
			Action act = () => PartOID.Parse([]);
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(PartOID)} failed to parse as input is empty. (Parameter 'input')");
		}
	}
}