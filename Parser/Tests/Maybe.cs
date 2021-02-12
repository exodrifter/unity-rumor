using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Maybe
	{
		/// <summary>
		/// Test what happens when the parser will succeed.
		/// </summary>
		[Test]
		public static void MaybeJustSuccess()
		{
			var state = new ParserState("a", 4);

			var result = Parse.Char('a').Maybe()(state);
			Assert.AreEqual(new Maybe<char>('a'), result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the parser will fail.
		/// </summary>
		[Test]
		public static void MaybeNothingSuccess()
		{
			var state = new ParserState("b", 4);

			var result = Parse.Char('a').Maybe()(state);
			Assert.AreEqual(new Maybe<char>(), result);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is empty.
		/// </summary>
		[Test]
		public static void MaybeEmptySuccess()
		{
			var state = new ParserState("", 4);

			var result = Parse.Char('a').Maybe()(state);
			Assert.AreEqual(new Maybe<char>(), result);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
