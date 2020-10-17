using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class PauseExecution
	{
		[Test]
		public static void PauseSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new PauseNode(1d)
						}
					},
				}
			);

			var iter = rumor.Start();
			Assert.IsTrue(iter.MoveNext());
			Assert.IsTrue(rumor.Executing);

			rumor.Update(0.5f);
			Assert.IsTrue(iter.MoveNext());
			Assert.IsTrue(rumor.Executing);

			rumor.Update(0.5f);
			Assert.IsFalse(iter.MoveNext());
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}
	}
}
