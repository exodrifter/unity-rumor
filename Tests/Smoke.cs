using Exodrifter.Rumor.Compiler;
using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Engine.Tests;
using Exodrifter.Rumor.Parser;
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
				"  : Alice hangs up.\n" +

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
				.Compile(script);

			double number = 0;
			var rumor = new Engine.Rumor(nodes);
			rumor.Bindings.Bind<double>("add", (x) => number += x);
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
				new Dictionary<string, string>
				{
					{ "_narrator", "Alice hangs up." },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
		}
	}
}
