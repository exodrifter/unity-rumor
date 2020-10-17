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
			var state = new ParserState("choose", 4, 0);

			var node = Compiler.Choose(state);
			Assert.AreEqual(new ChooseNode(), node);
		}
	}
}
