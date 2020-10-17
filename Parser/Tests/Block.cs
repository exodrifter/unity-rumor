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
			var state = new ParserState("  a\n ", 4, 0);
			state.IndentIndex = 2;

			var result = Parse.Block(Parse.Char('a'), Parse.Same)(state);
			Assert.AreEqual(new char[] { 'a' }, result);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(2, state.IndentIndex);
		}
	}
}
