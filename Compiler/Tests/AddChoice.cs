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
				4
			);

			var result = Compiler.AddChoice(state);

			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode(
							"_6UneA59ka0+xivIVzV0HSu2W08o=",
							"Hello?")
						}
					},
					{ "_6UneA59ka0+xivIVzV0HSu2W08o=", new List<Node>()
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
				"    Anyone there?\n" +
				"  : Hello!",
				4
			);

			var result = Compiler.AddChoice(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode(
							"_SnGOWgMY+B2jxdb0cDqRSaV2by8=",
							"Hello? Anyone there?")
						}
					},
					{ "_SnGOWgMY+B2jxdb0cDqRSaV2by8=", new List<Node>()
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
				4, new RumorParserState()
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
				"    Anyone there?\n" +
				"  : Hello!",
				4, new RumorParserState()
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

		[Test]
		public static void AddChoiceEmptySingleSuccess()
		{
			var state = new ParserState(
				"choice\n" +
				"  > Hello?",
				4
			);

			var result = Compiler.AddChoice(state);

			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode(
							"_aw5hXzryzc4KSwOXy9KhEqu8QFM=",
							"Hello?")
						}
					},
					{ "_aw5hXzryzc4KSwOXy9KhEqu8QFM=", new List<Node>() {}
					},
				},
				result
			);
		}
	}
}
