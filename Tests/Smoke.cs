using Exodrifter.Rumor.Compiler;
using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Engine.Tests;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Tests
{
	public static class Smoke
	{
		[Test]
		public static void SmokeTest1()
		{
			string script =
				"Alice: Hello? Are you there?\n" +
				"Eve: Yes, yes, just... one moment.\n" +
				"add(4)\n" +
				": The line briefly emits a sharp static. Then, silence.\n" +

				"choice [hangup]\n" +
				"  > Hang up.\n" +
				"  : Alice hangs up after { subtract(4, 2) } seconds.\n" +

				"choice [wait]\n" +
				"  > Wait.\n" +
				"  waited = { true }\n" +
				"  : Alice waits\n" +

				"choose\n" +

				"if { waited }\n" +
				"  Eve: There we go. Sorry about that.\n" +
				"  Alice: No problem.\n" +
				"  add(4)\n";

			var nodes = new RumorCompiler()
				.SetTabSize(2)
				.LinkAction("add", ValueType.Number)
				.LinkFunction("subtract", ValueType.Number, ValueType.Number, ValueType.Number)
				.Compile(script);

			double number = 0;
			var rumor = new Engine.Rumor(nodes);
			rumor.Bindings.Bind<double>("add", (x) => number += x);
			rumor.Bindings.Bind<int, int, int>("subtract", (l, r) => { return l - r; });
			rumor.Start();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "Alice", "Hello? Are you there?" },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "Eve", "Yes, yes, just... one moment." },
				},
				rumor.State.GetDialog()
			);

			// Simulate saving and loading the game
			rumor = SerializationUtil.Reserialize(rumor);
			rumor.Bindings.Bind<double>("add", (x) => number += x);
			rumor.Bindings.Bind<int, int, int>("subtract", (l, r) => { return l - r; });

			rumor.Advance();

			Assert.AreEqual(4, number);
			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "The line briefly emits a sharp static. Then, silence." },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			rumor.Choose("hangup");

			Assert.AreEqual(
				"Alice hangs up after 2 seconds.",
				rumor.State.GetDialog()["_narrator"]
			);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
		}
	
		[Test]
		public static void SmokeTest2()
		{
			string script =
				"x = { 0 }\n" +
				"if { true is true }\n" +
				"  x = { x + 1 }\n" +
				"  : Hello!\n" +
				": In the middle!\n" +
				"if { true is true }\n" +
				"  x = { x + 1 }\n" +
				"  : Hello!\n" +
				": That's it!\n";

			var nodes = new RumorCompiler()
				.SetTabSize(2)
				.Compile(script);

			var rumor = new Engine.Rumor(nodes);
			rumor.Start();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "Hello!" },
				},
				rumor.State.GetDialog()
			);
			rumor.Advance();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "In the middle!" },
				},
				rumor.State.GetDialog()
			);
			rumor.Advance();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "Hello!" },
				},
				rumor.State.GetDialog()
			);
			rumor.Advance();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "That's it!" },
				},
				rumor.State.GetDialog()
			);
			rumor.Advance();

			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(new NumberValue(2), rumor.Scope.Get("x"));
		}
	}
}
