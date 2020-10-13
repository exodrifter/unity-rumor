using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Simple
	{
		#region Char

		[Test]
		public static void CharParserSuccess()
		{
			var state = new State("h", 4, 0);

			var result = Parse.Char('h')(state);
			Assert.AreEqual('h', result.Value);
		}

		[Test]
		public static void CharParserIndexSuccess()
		{
			var state = new State("world", 4, 3);

			var result = Parse.Char('l')(state);
			Assert.AreEqual('l', result.Value);
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
		}

		#endregion

		#region Indented

		[Test]
		public static void ZeroIndentedSuccess()
		{
			var state = new State("hello world!", 4, 0);

			var result = Parse.SameOrIndented()(state);
			Assert.AreEqual(1, result.Value);
		}

		[Test]
		public static void IndentedSuccess()
		{
			var state = new State("    hello world!", 4, 4);

			var result = Parse.SameOrIndented()(state);
			Assert.AreEqual(5, result.Value);
		}

		[Test]
		public static void IndentedTabSuccess()
		{
			var state = new State("  \thello world!", 4, 3);

			var result = Parse.SameOrIndented()(state);
			Assert.AreEqual(4, result.Value);
		}

		[Test]
		public static void IndentedLineSuccess()
		{
			var state = new State("\n  \thello world!", 4, 4);

			var result = Parse.SameOrIndented()(state);
			Assert.AreEqual(4, result.Value);
		}

		[Test]
		public static void IndentedFailure()
		{
			var state = new State("    hello world!", 4, 4)
				.SetIndent()
				.AddIndex(-2);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.SameOrIndented()(state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(
				new string[] { "line indented to column 5 or more" },
				exception.Expected
			);
		}

		[Test]
		public static void IndentedLineFailure()
		{
			var state = new State("\n    hello world!", 4, 5)
				.SetIndent()
				.AddIndex(-2);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.SameOrIndented()(state)
			);
			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(
				new string[] { "line indented to column 5 or more" },
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
			Assert.AreEqual("hello world!", result.Value);
		}

		[Test]
		public static void StringParserIndexSuccess()
		{
			var state = new State("hello world!", 4, 6);

			var result = Parse.String("world!")(state);
			Assert.AreEqual("world!", result.Value);
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
		public static void StringParserIndexFail()
		{
			var state = new State("hello world!", 4, 6);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("WORLD!")(state)
			);
			Assert.AreEqual(6, exception.Index);
			Assert.AreEqual(new string[] { "WORLD!" }, exception.Expected);
		}

		#endregion
	}
}
