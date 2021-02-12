using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Block
	{
		/// <summary>
		/// Ensure that the last newline in a block is not consumed.
		/// </summary>
		[Test]
		public static void BlockDoesNotConsumesNewline()
		{
			var state = new ParserState("  a\n ", 4);
			state.IndentIndex = 2;

			var result = Parse.Block(Parse.Char('a'), Parse.Same)(state);
			Assert.AreEqual(new char[] { 'a' }, result);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(2, state.IndentIndex);
		}

		/// <summary>
		/// Ensure that prefix blocks work as expected.
		/// </summary>
		[Test]
		public static void PrefixBlockSingleSuccess()
		{
			var state = new ParserState("  >a\n ", 4);
			state.IndentIndex = 2;

			var result = Parse.PrefixBlock(
				Parse.Char('>'), Parse.Char('a'), Parse.Same
			)(state);
			Assert.AreEqual(new char[] { 'a' }, result);

			Assert.AreEqual(4, state.Index);
			Assert.AreEqual(2, state.IndentIndex);
		}

		/// <summary>
		/// Ensure that prefix blocks work as expected.
		/// </summary>
		[Test]
		public static void PrefixBlockMultilineSuccess()
		{
			var state = new ParserState("  >a\n  >a", 4);
			state.IndentIndex = 2;

			var result = Parse.PrefixBlock(
				Parse.Char('>'), Parse.Char('a'), Parse.Same
			)(state);
			Assert.AreEqual(new char[] { 'a', 'a' }, result);

			Assert.AreEqual(9, state.Index);
			Assert.AreEqual(2, state.IndentIndex);
		}
	}
}
