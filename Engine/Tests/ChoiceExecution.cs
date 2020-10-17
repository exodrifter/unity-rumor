using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class ChoiceExecution
	{
		[Test]
		public static void ChoiceSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ChoiceNode("choice1", "Hello?")
						, new ChoiceNode("choice2", "Hello??")
						}
					}
				}
			);

			var iter = rumor.Start();
			Assert.IsFalse(iter.MoveNext());
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "choice1", "Hello?" },
					{ "choice2", "Hello??" }
				},
				rumor.State.GetChoices()
			);
		}
	}
}
