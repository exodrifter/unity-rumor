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
			var state = new ParserState("Alice: Hello world!", 4, 0);

			var result = Compiler.Script(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SayNode("Alice", "Hello world!")
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
			var state = new ParserState(@"
				Alice: Hello world!
				Alice: How are you?
				Alice: The weather seems nice today.
				"
				, 4, 0
			);

			var result = Compiler.Script(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SayNode("Alice", "Hello world!")
						, new SayNode("Alice", "How are you?")
						, new SayNode("Alice", "The weather seems nice today.")
						}
					},
				},
				result
			);

			Assert.AreEqual(92, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
