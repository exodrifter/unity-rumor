using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Jump
	{
		[Test]
		public static void JumpSuccess()
		{
			var state = new ParserState("jump start", 4);

			var node = Compiler.Jump(state);
			Assert.AreEqual(new JumpNode("start"), node);
		}
	}
}
