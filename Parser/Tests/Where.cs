using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Where
	{
		/// <summary>
		/// Test what happens when the parser will succeed.
		/// </summary>
		[Test]
		public static void WhereSuccess()
		{
			var state = new ParserState("a", 4, 0);

			var result = Parse.AnyChar.Where(ch => ch == 'a', "a")(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the parser will fail.
		/// </summary>
		[Test]
		public static void WhereFailure()
		{
			var state = new ParserState("b", 4, 0);

			var exception = Assert.Throws<ReasonException>(() =>
				Parse.AnyChar.Where(ch => ch == 'a', "message")(state)
			);
			Assert.AreEqual(
				"parse exception at index 0: message",
				exception.Message
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
