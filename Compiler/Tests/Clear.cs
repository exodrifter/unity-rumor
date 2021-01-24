using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Clear
	{
		#region Node

		[Test]
		public static void ClearSuccess()
		{
			var state = new ParserState("clear", 4);

			var node = Compiler.Clear(state);
			Assert.AreEqual(new ClearNode(ClearType.All), node);
		}

		[Test]
		public static void ClearAllSuccess()
		{
			var state = new ParserState("clear all", 4);

			var node = Compiler.Clear(state);
			Assert.AreEqual(new ClearNode(ClearType.All), node);
		}

		[Test]
		public static void ClearChoicesSuccess()
		{
			var state = new ParserState("clear choices", 4);

			var node = Compiler.Clear(state);
			Assert.AreEqual(new ClearNode(ClearType.Choices), node);
		}

		[Test]
		public static void ClearDialogSuccess()
		{
			var state = new ParserState("clear dialog", 4);

			var node = Compiler.Clear(state);
			Assert.AreEqual(new ClearNode(ClearType.Dialog), node);
		}

		#endregion
	}
}
