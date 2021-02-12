using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Many
	{
		/// <summary>
		/// Test what happens in the expected use case for this parser.
		/// </summary>
		[Test]
		public static void ManySuccess()
		{
			var state = new ParserState("aaa", 4);

			var result = Parse.Char('a').Many(0)(state);
			Assert.AreEqual(new char[] { 'a', 'a', 'a' }, result);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is empty.
		/// </summary>
		[Test]
		public static void ManyEmptySuccess()
		{
			var state = new ParserState("", 4);

			var result = Parse.Char('a').Many(0)(state);
			Assert.AreEqual(new char[] { }, result);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test that the parser succeeds when followed by input that the
		/// original parser cannot parse.
		/// </summary>
		[Test]
		public static void ManyTrailingSuccess()
		{
			var state = new ParserState("az", 4);

			var result = Parse.Char('a').Many(0)(state);
			Assert.AreEqual(new char[] { 'a' }, result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the original parser will not succeed.
		/// </summary>
		[Test]
		public static void Many0Success()
		{
			var state = new ParserState("z", 4);

			var result = Parse.Char('a').Many(0)(state);
			Assert.AreEqual(new char[] { }, result);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test that the parser works when a minimum number of successes is
		/// specified.
		/// </summary>
		[Test]
		public static void Many1Success()
		{
			var state = new ParserState("a", 4);

			var result = Parse.Char('a').Many(1)(state);
			Assert.AreEqual(new char[] { 'a' }, result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test that the parser fails when a minimum number of successes is not
		/// met.
		/// </summary>
		[Test]
		public static void ManyFailure()
		{
			var state = new ParserState("a", 4);

			var exception = Assert.Throws<ReasonException>(() =>
				Parse.Char('a').Many(2)(state)
			);
			Assert.AreEqual(
				"parse exception at index 1: expected at least 1 more " +
				"instance(s) of the parser to succeed",
				exception.Message
			);
			Assert.AreEqual(
				"parse exception at index 1: expected 'a'",
				exception.InnerException.Message
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
