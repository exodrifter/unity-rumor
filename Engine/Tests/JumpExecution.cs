using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class JumpExecution
	{
		[Test]
		public static void JumpSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new JumpNode("foobar")
						}
					},
					{ "foobar", new List<Node>()
						{ new WaitNode()
						}
					}
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

		[Test]
		public static void JumpFailure()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new JumpNode("foobar")
						}
					}
				}
			);

			var iter = rumor.Start();
			var exception = Assert.Throws<InvalidOperationException>(() =>
				iter.MoveNext()
			);
			Assert.AreEqual(
				"The label \"foobar\" does not exist!",
				exception.Message
			);
		}
	}
}
