using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Simple
	{
		#region Indented

		[Test]
		public static void ZeroSameOrIndentedSuccess()
		{
			var state = new ParserState("hello world!", 4);

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(1, result);
		}

		[Test]
		public static void SameOrIndentedSuccess()
		{
			var state = new ParserState("    hello world!", 4);
			state.Index = 4;

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(5, result);
		}

		[Test]
		public static void SameOrIndentedTabSuccess()
		{
			var state = new ParserState("  \thello world!", 4);
			state.Index = 3;

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(4, result);
		}

		[Test]
		public static void SameOrIndentedLineSuccess()
		{
			var state = new ParserState("\n  \thello world!", 4);
			state.Index = 4;

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(4, result);
		}

		[Test]
		public static void SameOrIndentedFailure()
		{
			var state = new ParserState("    hello world!", 4);
			state.Index = 2;
			state.IndentIndex = 4;

			var exception = Assert.Throws<ReasonException>(() =>
				Parse.SameOrIndented(state)
			);
			Assert.AreEqual(
				"parse exception at index 2: line indented to column 5 or more",
				exception.Message
			);
		}

		[Test]
		public static void SameOrIndentedLineFailure()
		{
			var state = new ParserState("\n    hello world!", 4);
			state.Index = 3;
			state.IndentIndex = 5;

			var exception = Assert.Throws<ReasonException>(() =>
				Parse.SameOrIndented(state)
			);
			Assert.AreEqual(
				"parse exception at index 3: line indented to column 5 or more",
				exception.Message
			);
		}

		[Test]
		public static void BlockSuccess()
		{
			var state = new ParserState("  a\n  a\n a", 4);
			state.Index = 2;
			state.IndentIndex = 2;

			var result = Parse.Block(Parse.Char('a'), Parse.SameOrIndented)
				.String()(state);
			Assert.AreEqual("aa", result);
		}

		[Test]
		public static void BlockFailure()
		{
			var state = new ParserState("  ab\n  a\n a", 4);
			state.Index = 2;
			state.IndentIndex = 2;

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Block(Parse.Char('a'), Parse.SameOrIndented)
					.String()(state)
			);

			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(
				new string[] { "space" },
				exception.Expected
			);
		}

		#endregion

		#region String

		[Test]
		public static void StringParserSuccess()
		{
			var state = new ParserState("hello world!", 4);

			var result = Parse.String("hello world!")(state);
			Assert.AreEqual("hello world!", result);
		}

		[Test]
		public static void StringParserIndexSuccess()
		{
			var state = new ParserState("hello world!", 4);
			state.Index = 6;

			var result = Parse.String("world!")(state);
			Assert.AreEqual("world!", result);
		}

		[Test]
		public static void StringParserFail()
		{
			var state = new ParserState("hello world!", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.String("HELLO WORLD!")(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(
				new string[] { "\"HELLO WORLD!\"" },
				exception.Expected
			);
		}

		[Test]
		public static void StringParserLengthFail()
		{
			var state = new ParserState("hello world!", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.String("HELLO WORLD!!")(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(
				new string[] { "\"HELLO WORLD!!\"" },
				exception.Expected
			);
		}

		[Test]
		public static void StringParserIndexFail()
		{
			var state = new ParserState("hello world!", 4);
			state.Index = 6;

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.String("WORLD!")(state)
			);
			Assert.AreEqual(6, exception.Index);
			Assert.AreEqual(new string[] { "\"WORLD!\"" }, exception.Expected);
		}

		[Test]
		public static void StringParserMultiSuccess()
		{
			var state = new ParserState("bar", 4);

			var result = Parse.String("foo", "bar")(state);
			Assert.AreEqual("bar", result);
		}

		#endregion
	}
}
