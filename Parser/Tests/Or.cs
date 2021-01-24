using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Or
	{
		/// <summary>
		/// Test what happens when the left parser succeeds.
		/// </summary>
		[Test]
		public static void OrLeftSuccess()
		{
			var state = new ParserState("a", 4);

			var result = Parse.Char('a').Or(Parse.Char('z'))(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the right parser succeeds.
		/// </summary>
		[Test]
		public static void OrRightSuccess()
		{
			var state = new ParserState("z", 4);

			var result = Parse.Char('a').Or(Parse.Char('z'))(state);
			Assert.AreEqual('z', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is empty.
		/// </summary>
		[Test]
		public static void OrEmptyFailure()
		{
			var state = new ParserState("", 4);

			var exception = Assert.Throws<MultipleParserException>(() =>
				Parse.Char('a').Or(Parse.Char('z'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(
				"parse exception at index 0: expected one of the following " +
				"exceptions to not happen: parse exception at index 0: " +
				"expected 'a'; parse exception at index 0: expected 'z'",
				exception.Message
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when neither parser will succeed.
		/// </summary>
		[Test]
		public static void OrFailure()
		{
			var state = new ParserState("m", 4);

			var exception = Assert.Throws<MultipleParserException>(() =>
				Parse.Char('a').Or(Parse.Char('z'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(
				"parse exception at index 0: expected one of the following " +
				"exceptions to not happen: parse exception at index 0: " +
				"expected 'a'; parse exception at index 0: expected 'z'",
				exception.Message
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}