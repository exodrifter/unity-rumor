using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class SayExecution
	{
		[Test]
		public static void SaySuccess()
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
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "Alice", "Hello world!" }
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(
				new Dictionary<string, string>()
				{
					{ "Alice", "How are you?" }
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
