using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class SurroundBlock
	{
		/// <summary>
		/// Test what happens in the expected use case for this parser.
		/// </summary>
		[Test]
		public static void SurroundBlockSuccess()
		{
			var state = new ParserState("(\n a \n)", 4);

			var result = Parse.SurroundBlock('(', ')',
				Parse.Char('a'), Parse.SameOrIndented
			)(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(7, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the content is not indented properly
		/// </summary>
		[Test]
		public static void SurroundBlockFailure()
		{
			var state = new ParserState("(\na \n)", 4);

			var exception = Assert.Throws<ReasonException>(() =>
				Parse.SurroundBlock('(', ')',
					Parse.Char('a'), Parse.Indented
				)(state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(
				"parse exception at index 2: line indented to column 1 or more",
				exception.Message
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
