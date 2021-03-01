using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Choose
	{
		[Test]
		public static void ChooseSuccess()
		{
			var state = new ParserState("choose", 4);

			var node = Compiler.Choose(state);
			Assert.AreEqual(new ChooseNode(), node);
		}

		[Test]
		public static void ChooseTimeoutSuccess()
		{
			var state = new ParserState("choose in { 2 } seconds or jump foobar", 4);

			var node = Compiler.Choose(state);
			Assert.AreEqual(
				new ChooseNode(new NumberLiteral(2), "foobar"),
				node
			);
		}
	}
}
