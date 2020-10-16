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
			var state = new State("jump start", 4, 0);

			var node = Compiler.Jump(state);
			Assert.AreEqual(new JumpNode("start"), node);
		}
	}
}
