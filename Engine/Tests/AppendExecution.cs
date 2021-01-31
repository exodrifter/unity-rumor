using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class AppendExecution
	{
		[Test]
		public static void AppendSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new AppendDialogNode("Alice", "Hello world!")
						, new AppendDialogNode("Alice", "How are you?")
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
					{ "Alice", "Hello world! How are you?" }
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
