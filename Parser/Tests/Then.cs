using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Then
	{
		/// <summary>
		/// Test the expected use case for this parser.
		/// </summary>
		[Test]
		public static void ThenSuccess()
		{
			var state = new ParserState("ab", 4);

			var result = Parse.Char('a').Then(Parse.Char('b'))(state);
			Assert.AreEqual('b', result);

			Assert.AreEqual(2, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the first parser fails.
		/// </summary>
		[Test]
		public static void ThenLeftFailure()
		{
			var state = new ParserState("ab", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Char('z').Then(Parse.Char('b'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "'z'" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the second parser fails.
		/// </summary>
		[Test]
		public static void ThenRightFailure()
		{
			var state = new ParserState("ab", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Char('a').Then(Parse.Char('z'))(state)
			);
			Assert.AreEqual(1, exception.Index);
			Assert.AreEqual(new string[] { "'z'" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
