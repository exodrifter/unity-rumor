using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class WaitExecution
	{
		[Test]
		public static void WaitSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new WaitNode()
						}
					},
				}
			);

			var iter = rumor.Start();
			Assert.IsTrue(iter.MoveNext());
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsFalse(iter.MoveNext());
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}
	}
}
