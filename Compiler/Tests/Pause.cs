using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Pause
	{
		#region Node

		[Test]
		public static void PauseMillisecondsSuccess()
		{
			var state = new ParserState("pause { 100 } milliseconds", 4);

			var node = Compiler.Pause(state);
			Assert.AreEqual(new PauseNode(0.1d), node);
		}

		[Test]
		public static void PauseMsSuccess()
		{
			var state = new ParserState("pause { 100 } ms", 4);

			var node = Compiler.Pause(state);
			Assert.AreEqual(new PauseNode(0.1d), node);
		}

		[Test]
		public static void PauseSecondsSuccess()
		{
			var state = new ParserState("pause { 100 } seconds", 4);

			var node = Compiler.Pause(state);
			Assert.AreEqual(new PauseNode(100d), node);
		}


		[Test]
		public static void PauseSSuccess()
		{
			var state = new ParserState("pause { 100 } s", 4);

			var node = Compiler.Pause(state);
			Assert.AreEqual(new PauseNode(100d), node);
		}

		[Test]
		public static void PauseMinutesSuccess()
		{
			var state = new ParserState("pause { 100 } minutes", 4);

			var node = Compiler.Pause(state);
			Assert.AreEqual(new PauseNode(6000d), node);
		}

		[Test]
		public static void PauseMSuccess()
		{
			var state = new ParserState("pause { 100 } m", 4);

			var node = Compiler.Pause(state);
			Assert.AreEqual(new PauseNode(6000d), node);
		}

		#endregion
	}
}
