using Exodrifter.Rumor.Compiler;
using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Tests
{
	public static class Smoke
	{
		[Test]
		public static void AppendDialogLineSuccess()
		{
			string script =
				"Alice: Hello? Are you there?\n" +
				"Eve: Yes, yes, just... one moment.\n" +
				"waited = false\n" +
				"add(4)\n" +
				": The line briefly emits a sharp static. Then, silence.\n" +

				"choice [hangup]\n" +
				"  > Hang up.\n" +
				"  : Alice hangs up.\n" +

				"choice [wait]\n" +
				"  > Wait.\n" +
				"  waited = true\n" +
				"  : Alice waits\n" +

				"choose\n" +

				"if waited\n" +
				"  Eve: There we go. Sorry about that.\n" +
				"  Alice: No problem.\n" +
				"  add(4)\n";

			var userState = new RumorParserState();
			userState.LinkAction("add", ValueType.Number);

			var state = new ParserState(script, 2, userState);
			var nodes = Compiler.Compiler.Script(state);

			double number = 0;
			var rumor = new Engine.Rumor(nodes);
			rumor.Bindings.Bind<double>("add", (x) => number += x);
			var iter = rumor.Start();

			iter.MoveNext();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "Alice", "Hello? Are you there?" },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			iter.MoveNext();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "Eve", "Yes, yes, just... one moment." },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			iter.MoveNext();

			Assert.AreEqual(4, number);
			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "The line briefly emits a sharp static. Then, silence." },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			iter.MoveNext();

			rumor.Choose("hangup");
			iter.MoveNext();

			Assert.AreEqual(
				new Dictionary<string, string>
				{
					{ "_narrator", "Alice hangs up." },
				},
				rumor.State.GetDialog()
			);

			rumor.Advance();
			Assert.IsFalse(iter.MoveNext());
		}
	}
}
