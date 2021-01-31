using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class ClearExecution
	{
		[Test]
		public static void ClearDialogSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new SetDialogNode("Alice", "Hello world!")
						, new ClearNode(ClearType.Dialog)
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
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
			Assert.AreEqual(
				new Dictionary<string, string>() { },
				rumor.State.GetDialog()
			);
		}
	}
}
