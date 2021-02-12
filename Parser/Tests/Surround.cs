using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Surround
	{
		/// <summary>
		/// Test what happens in the expected use case for this parser.
		/// </summary>
		[Test]
		public static void SurroundSuccess()
		{
			var state = new ParserState("(a)", 4);

			var result = Parse.Surround('(', ')', Parse.Char('a'))(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the delimiters are padded by whitespace.
		/// </summary>
		[Test]
		public static void SurroundPaddedSuccess()
		{
			var state = new ParserState("(\n a\t \n)", 4);

			var result = Parse.Surround('(', ')', Parse.Char('a'))(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(8, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the right delimiter parser fails.
		/// </summary>
		[Test]
		public static void SurroundLeftFailure()
		{
			var state = new ParserState(".a)", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Surround('(', ')', Parse.Char('a'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "'('" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is missing the right delimiter.
		/// </summary>
		[Test]
		public static void SurroundLeftMissingFailure()
		{
			var state = new ParserState("a)", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Surround('(', ')', Parse.Char('a'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "'('" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the right delimiter parser fails.
		/// </summary>
		[Test]
		public static void SurroundRightFailure()
		{
			var state = new ParserState("(a.", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Surround('(', ')', Parse.Char('a'))(state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(new string[] { "')'" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is missing the right delimiter.
		/// </summary>
		[Test]
		public static void SurroundRightMissingFailure()
		{
			var state = new ParserState("(a", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Surround('(', ')', Parse.Char('a'))(state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(new string[] { "')'" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
