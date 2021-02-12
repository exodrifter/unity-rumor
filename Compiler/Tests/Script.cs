using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler.Tests
{
	using Rumor = Engine.Rumor;

	public static class Script
	{
		[Test]
		public static void ScriptOneSuccess()
		{
			var state = new ParserState("Alice: Hello world!", 4);

			var result = Compiler.Script(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SetDialogNode("Alice", "Hello world!")
						}
					},
				},
				result
			);

			Assert.AreEqual(19, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		[Test]
		public static void ScriptMultipleSuccess()
		{
			var state = new ParserState(
				"Alice: Hello world!\n" +
				"Alice: How are you?\n" +
				"Alice: The weather seems nice today.\n"
				, 4
			);

			var result = Compiler.Script(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SetDialogNode("Alice", "Hello world!")
						, new SetDialogNode("Alice", "How are you?")
						, new SetDialogNode("Alice", "The weather seems nice today.")
						}
					},
				},
				result
			);

			Assert.AreEqual(76, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
