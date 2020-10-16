using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Simple
	{
		#region Indented

		[Test]
		public static void ZeroSameOrIndentedSuccess()
		{
			var state = new State("hello world!", 4, 0);

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(1, result);
		}

		[Test]
		public static void SameOrIndentedSuccess()
		{
			var state = new State("    hello world!", 4, 4);

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(5, result);
		}

		[Test]
		public static void SameOrIndentedTabSuccess()
		{
			var state = new State("  \thello world!", 4, 3);

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(4, result);
		}

		[Test]
		public static void SameOrIndentedLineSuccess()
		{
			var state = new State("\n  \thello world!", 4, 4);

			var result = Parse.SameOrIndented(state);
			Assert.AreEqual(4, result);
		}

		[Test]
		public static void SameOrIndentedFailure()
		{
			var state = new State("    hello world!", 4, 4);
			state.IndentIndex = state.Index;
			state.Index -= 2;

			var exception = Assert.Throws<ParserException>(() =>
				Parse.SameOrIndented(state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(
				new string[] { "line indented to column 5 or more" },
				exception.Expected
			);
		}

		[Test]
		public static void SameOrIndentedLineFailure()
		{
			var state = new State("\n    hello world!", 4, 5);
			state.IndentIndex = state.Index;
			state.Index -= 2;

			var exception = Assert.Throws<ParserException>(() =>
				Parse.SameOrIndented(state)
			);
			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(
				new string[] { "line indented to column 5 or more" },
				exception.Expected
			);
		}

		[Test]
		public static void BlockSuccess()
		{
			var state = new State("  a\n  a\n a", 4, 2);
			state.IndentIndex = state.Index;

			var result = Parse.Block(Parse.Char('a'), Parse.SameOrIndented)
				.String()(state);
			Assert.AreEqual("aa", result);
		}

		[Test]
		public static void BlockFailure()
		{
			var state = new State("  ab\n  a\n a", 4, 2);
			state.IndentIndex = state.Index;

			var exception = Assert.Throws<ParserException>(() =>
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
			var state = new State("hello world!", 4, 0);

			var result = Parse.String("hello world!")(state);
			Assert.AreEqual("hello world!", result);
		}

		[Test]
		public static void StringParserIndexSuccess()
		{
			var state = new State("hello world!", 4, 6);

			var result = Parse.String("world!")(state);
			Assert.AreEqual("world!", result);
		}

		[Test]
		public static void StringParserFail()
		{
			var state = new State("hello world!", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("HELLO WORLD!")(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "HELLO WORLD!" }, exception.Expected);
		}

		[Test]
		public static void StringParserLengthFail()
		{
			var state = new State("hello world!", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("HELLO WORLD!!")(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "HELLO WORLD!!" }, exception.Expected);
		}

		[Test]
		public static void StringParserIndexFail()
		{
			var state = new State("hello world!", 4, 6);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("WORLD!")(state)
			);
			Assert.AreEqual(6, exception.Index);
			Assert.AreEqual(new string[] { "WORLD!" }, exception.Expected);
		}

		[Test]
		public static void StringParserMultiSuccess()
		{
			var state = new State("bar", 4, 0);

			var result = Parse.String("foo", "bar")(state);
			Assert.AreEqual("bar", result);
		}

		#endregion
	}
}
