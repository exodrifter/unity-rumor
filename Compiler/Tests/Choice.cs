using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler.Tests
{
	using Rumor = Engine.Rumor;

	public static class Choice
	{
		[Test]
		public static void ChoiceUnlabeledSingleSuccess()
		{
			var state = new ParserState(
				"choice\n" +
				"  > Hello?\n" +
				"  : Hello!",
				4, 0
			);

			var result = Compiler.Choice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ChoiceNode("", "Hello?")
						}
					},
					{ "", new List<Node>()
						{ new SayNode(null, "Hello!")
						}
					},
				},
				result
			);
		}

		[Test]
		public static void ChoiceUnlabeledMultilineSuccess()
		{
			var state = new ParserState(
				"choice\n" +
				"  > Hello?\n" +
				"  > Anyone there?\n" +
				"  : Hello!",
				4, 0
			);

			var result = Compiler.Choice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ChoiceNode("", "Hello? Anyone there?")
						}
					},
					{ "", new List<Node>()
						{ new SayNode(null, "Hello!")
						}
					},
				},
				result
			);
		}
	}
}
