using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler.Tests
{
	using Rumor = Engine.Rumor;

	public static class AddChoice
	{
		[Test]
		public static void AddChoiceUnlabeledSingleSuccess()
		{
			var state = new ParserState(
				"choice\n" +
				"  > Hello?\n" +
				"  : Hello!",
				4, 0
			);

			var result = Compiler.AddChoice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode(
							"_7GsId23vSk3NFwOJtwQwIGlADow=",
							"Hello?")
						}
					},
					{ "_7GsId23vSk3NFwOJtwQwIGlADow=", new List<Node>()
						{ new SetDialogNode(null, "Hello!")
						}
					},
				},
				result
			);
		}

		[Test]
		public static void AddChoiceUnlabeledMultilineSuccess()
		{
			var state = new ParserState(
				"choice\n" +
				"  > Hello?\n" +
				"  > Anyone there?\n" +
				"  : Hello!",
				4, 0
			);

			var result = Compiler.AddChoice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode(
							"_Mab778K2k2yaPnNxIK+OQQkcp90=",
							"Hello? Anyone there?")
						}
					},
					{ "_Mab778K2k2yaPnNxIK+OQQkcp90=", new List<Node>()
						{ new SetDialogNode(null, "Hello!")
						}
					},
				},
				result
			);
		}

		[Test]
		public static void AddChoiceLabeledSingleSuccess()
		{
			var state = new ParserState(
				"choice [choice1]\n" +
				"  > Hello?\n" +
				"  : Hello!",
				4, 0
			);

			var result = Compiler.AddChoice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode("choice1", "Hello?")
						}
					},
					{ "choice1", new List<Node>()
						{ new SetDialogNode(null, "Hello!")
						}
					},
				},
				result
			);
		}

		[Test]
		public static void AddChoiceLabeledMultilineSuccess()
		{
			var state = new ParserState(
				"choice [choice1]\n" +
				"  > Hello?\n" +
				"  > Anyone there?\n" +
				"  : Hello!",
				4, 0
			);

			var result = Compiler.AddChoice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode("choice1", "Hello? Anyone there?")
						}
					},
					{ "choice1", new List<Node>()
						{ new SetDialogNode(null, "Hello!")
						}
					},
				},
				result
			);
		}
	}
}
