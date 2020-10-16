using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Identifier
	{
		[Test]
		public static void IdentifierAlphaSuccess()
		{
			var state = new State("Alice", 4, 0);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("Alice", result);
		}

		[Test]
		public static void IdentifierNumericSuccess()
		{
			var state = new State("123456789", 4, 0);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("123456789", result);
		}
	}
}