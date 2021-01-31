using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class ChooseExecution
	{
		[Test]
		public static void ChoiceSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AddChoiceNode("choice1", "Hello?")
						, new ChooseNode()
						}
					},
					{ "choice1", new List<Node>()
						{ new SetDialogNode("Alice", "Choice 1!")
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "choice1", "Hello?" },
				},
				rumor.State.GetChoices()
			);

			rumor.Choose("choice1");
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "Alice", "Choice 1!" }
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}
	}
}
