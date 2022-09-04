using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class CallExecution
	{
		[Test]
		public static void CallSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new CallNode("foobar")
						, new WaitNode()
						}
					},
					{ "foobar", new List<Node>()
						{ new WaitNode()
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void DropStackSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new CallNode("foobar")
						, new WaitNode()
						}
					},
					{ "foobar", new List<Node>()
						{ new WaitNode()
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);

			rumor.DropStack(1);
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void DropStackNestedSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new CallNode("foobar")
						, new WaitNode()
						}
					},
					{ "foobar", new List<Node>()
						{ new CallNode("baz")
						, new WaitNode()
						}
					},
					{ "baz", new List<Node>()
						{ new WaitNode()
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);

			rumor.DropStack(1);
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void DropStackCountSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new CallNode("foobar")
						, new WaitNode()
						}
					},
					{ "foobar", new List<Node>()
						{ new CallNode("baz")
						, new SetDialogNode("a", "Hello world")
						}
					},
					{ "baz", new List<Node>()
						{ new WaitNode()
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);

			rumor.DropStack(2);
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsTrue(rumor.Executing);
			Assert.AreEqual(rumor.State.GetDialog()["a"], "Hello world");

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void KeepTopStackCountSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new CallNode("foo")
						, new WaitNode()
						}
					},
					{ "foo", new List<Node>()
						{ new CallNode("bar")
						, new WaitNode()
						}
					},
					{ "bar", new List<Node>()
						{ new CallNode("baz")
						, new WaitNode()
						}
					},
					{ "baz", new List<Node>()
						{ new WaitNode()
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);

			rumor.KeepTopStack();
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void CallFailure()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new CallNode("foobar")
						}
					}
				}
			);

			var exception = Assert.Throws<InvalidOperationException>(() =>
				rumor.Start()
			);
			Assert.AreEqual(
				"The label \"foobar\" does not exist!",
				exception.Message
			);
		}
	}
}
