using AtelierTomato.MediaDB.Model.ID;
using FluentAssertions;

namespace AtelierTomato.MediaDB.Model.Test
{
	public class OIDTests
	{
		[Fact]
		public void SeriesPartOIDParseTest()
		{
			var spIDStr = "1.2.3:1.2.3";
			var spID = SeriesPartOID.Parse(spIDStr);
			spID.Should().BeEquivalentTo(new SeriesPartOID(new SeriesOID(3, new SeriesOID(2, new SeriesOID(1))), new PartOID(3, new PartOID(2, new PartOID(1)))));
		}
		[Theory]
		[InlineData("1")]
		[InlineData("1:2:3")]
		public void SeriesPartOIDFailTest(string input)
		{
			Action act = () => SeriesPartOID.Parse(input);
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(SeriesPartOID)} could not be split from '{input}' on character ':' into exactly two strings. (Parameter 'input')");
		}
		[Fact]
		public void SeriesPartOIDToStringTest()
		{
			var spID = new SeriesPartOID(new SeriesOID(3, new SeriesOID(2, new SeriesOID(1))), new PartOID(3, new PartOID(2, new PartOID(1))));
			var spIDStr = spID.ToString();
			spIDStr.Should().Be("1.2.3:1.2.3");
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

		[Fact]
		public void SeriesOIDNotIntegerTest()
		{
			Action act = () => SeriesOID.Parse("3.a.1");
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(SeriesOID)} failed to parse as 'a' is not a valid ulong value. (Parameter 'input')");
		}
		[Fact]
		public void SeriesOIDEmptyTest()
		{
			Action act = () => SeriesOID.Parse([]);
			act.Should().Throw<ArgumentException>().WithMessage($"{nameof(SeriesOID)} failed to parse as input is empty. (Parameter 'input')");
		}
	}
}