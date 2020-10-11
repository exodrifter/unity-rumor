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

			var result = new CharParser('h').Parse(state);
			Assert.AreEqual('h', result.Value);
		}

		[Test]
		public static void CharParserIndexSuccess()
		{
			var state = new State("world", 4, 3);

			var result = new CharParser('l').Parse(state);
			Assert.AreEqual('l', result.Value);
		}

		[Test]
		public static void CharParserFail()
		{
			var state = new State("h", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				new CharParser('H').Parse(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "H" }, exception.Expected);
		}

		[Test]
		public static void CharParserIndexFail()
		{
			var state = new State("world", 4, 3);

			var exception = Assert.Throws<ParserException>(() => 
				new CharParser('L').Parse(state)
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

			var result = new IndentedParser().Parse(state);
			Assert.AreEqual(0, result.Value);
		}

		[Test]
		public static void IndentedSuccess()
		{
			var state = new State("    hello world!", 4, 4);

			var result = new IndentedParser().Parse(state);
			Assert.AreEqual(4, result.Value);
		}

		[Test]
		public static void IndentedTabSuccess()
		{
			var state = new State("  \thello world!", 4, 3);

			var result = new IndentedParser().Parse(state);
			Assert.AreEqual(4, result.Value);
		}

		[Test]
		public static void IndentedLineSuccess()
		{
			var state = new State("\n  \thello world!", 4, 4);

			var result = new IndentedParser().Parse(state);
			Assert.AreEqual(4, result.Value);
		}

		[Test]
		public static void IndentedFailure()
		{
			var state = new State("    hello world!", 4, 4)
				.SetIndent()
				.AddIndex(-2);

			var exception = Assert.Throws<ParserException>(() =>
				new IndentedParser().Parse(state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(new string[] { "indented line" }, exception.Expected);
		}

		[Test]
		public static void IndentedLineFailure()
		{
			var state = new State("\n    hello world!", 4, 5)
				.SetIndent()
				.AddIndex(-2);

			var exception = Assert.Throws<ParserException>(() =>
				new IndentedParser().Parse(state)
			);
			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(new string[] { "indented line" }, exception.Expected);
		}

		#endregion

		#region String

		[Test]
		public static void StringParserSuccess()
		{
			var state = new State("hello world!", 4, 0);

			var result = new StringParser("hello world!").Parse(state);
			Assert.AreEqual("hello world!", result.Value);
		}

		[Test]
		public static void StringParserIndexSuccess()
		{
			var state = new State("hello world!", 4, 6);

			var result = new StringParser("world!").Parse(state);
			Assert.AreEqual("world!", result.Value);
		}

		[Test]
		public static void StringParserFail()
		{
			var state = new State("hello world!", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				new StringParser("HELLO WORLD!").Parse(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "HELLO WORLD!" }, exception.Expected);
		}

		[Test]
		public static void StringParserIndexFail()
		{
			var state = new State("hello world!", 4, 6);

			var exception = Assert.Throws<ParserException>(() =>
				new StringParser("WORLD!").Parse(state)
			);
			Assert.AreEqual(6, exception.Index);
			Assert.AreEqual(new string[] { "WORLD!" }, exception.Expected);
		}

		#endregion
	}
}
