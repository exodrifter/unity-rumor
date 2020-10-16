using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Wait
	{
		[Test]
		public static void WaitSuccess()
		{
			var state = new State("wait", 4, 0);

			var node = Compiler.Wait(state);
			Assert.AreEqual(new WaitNode(), node);
		}
	}
}
