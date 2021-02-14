using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class AutoAdvanceExecution
	{
		[Test]
		public static void AutoAdvance0()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SetDialogNode("Alice", "Hello world!")
						, new SetDialogNode("Alice", "How are you?")
						}
					}
				}
			);

			rumor.Start();
			rumor.SetAutoAdvance(0);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "Alice", "How are you?" }
				},
				rumor.State.GetDialog()
			);

			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void AutoAdvanceNormal()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SetDialogNode("Alice", "Hello world!")
						, new SetDialogNode("Alice", "How are you?")
						}
					}
				}
			);

			rumor.Start();
			rumor.SetAutoAdvance(1);
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "Alice", "Hello world!" }
				},
				rumor.State.GetDialog()
			);

			rumor.Update(1);
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "Alice", "How are you?" }
				},
				rumor.State.GetDialog()
			);

			rumor.Update(1);
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}
	}
}
