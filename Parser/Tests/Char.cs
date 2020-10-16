using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Char
	{
		[Test]
		public static void CharParserSuccess()
		{
			var state = new State("h", 4, 0);

			var result = Parse.Char('h')(state);
			Assert.AreEqual('h', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		[Test]
		public static void CharParserIndexSuccess()
		{
			var state = new State("world", 4, 3);

			var result = Parse.Char('l')(state);
			Assert.AreEqual('l', result);

			Assert.AreEqual(4, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		[Test]
		public static void CharParserFail()
		{
			var state = new State("h", 4, 0);
			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('H')(state)
			);

			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "H" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		[Test]
		public static void CharParserIndexFail()
		{
			var state = new State("world", 4, 3);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('L')(state)
			);
			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(new string[] { "L" }, exception.Expected);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}