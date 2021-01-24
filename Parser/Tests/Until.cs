using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Until
	{
		/// <summary>
		/// Test the expected use case for this parser.
		/// </summary>
		[Test]
		public static void UntilSuccess()
		{
			var state = new ParserState("aaab", 4);

			var result = Parse.Char('a').Until(Parse.Char('b'))(state);
			Assert.AreEqual(new char[] { 'a', 'a', 'a' }, result);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is empty.
		/// </summary>
		[Test]
		public static void UntilEmptyFailure()
		{
			var state = new ParserState("", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Char('a').Until(Parse.Char('b'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "'b'" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the parser fails, but the end parser
		/// succeeds.
		/// </summary>
		[Test]
		public static void Until0Success()
		{
			var state = new ParserState("b", 4);

			var result = Parse.Char('a').Until(Parse.Char('b'))(state);
			Assert.AreEqual(new char[] { }, result);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the end parser will never succeed.
		/// </summary>
		[Test]
		public static void UntilFailure()
		{
			var state = new ParserState("aaaa", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Char('a').Until(Parse.Char('b'))(state)
			);
			Assert.AreEqual(4, exception.Index);
			Assert.AreEqual(new string[] { "'b'" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
