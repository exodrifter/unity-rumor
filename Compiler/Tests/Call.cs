using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Call
	{
		[Test]
		public static void CallSuccess()
		{
			var state = new ParserState("call start", 4);

			var node = Compiler.Call(state);
			Assert.AreEqual(new CallNode("start"), node);
		}
	}
}
