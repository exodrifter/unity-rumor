using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class SetDialog
	{
		#region Node

		[Test]
		public static void SetDialogLineSuccess()
		{
			var state = new ParserState(": Hello world!", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SetDialogSpeakerLineSuccess()
		{
			var state = new ParserState("alice: Hello world!", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SetDialogMultiLineSuccess()
		{
			var state = new ParserState(": Hello\n  world!", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SetDialogSpeakerMultiLineSuccess()
		{
			var state = new ParserState("alice: Hello\n  world!", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SetDialogNextMultiLineSuccess()
		{
			var state = new ParserState(":\n Hello\n  world!", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SetDialogSpeakerNextMultiLineSuccess()
		{
			var state = new ParserState("alice:\n Hello\n  world!", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode("alice", "Hello world!"), node);
		}

		#endregion

		#region Boolean Substitution

		[Test]
		public static void SetDialogBooleanSubstitutionSuccess()
		{
			var state = new ParserState(": That's { true or false }.", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "That's true."), node);
		}

		[Test]
		public static void SetDialogBooleanSubstitutionMultiLineSuccess()
		{
			var state = new ParserState(": That's { true\n or\n false }.", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "That's true."), node);
		}

		[Test]
		public static void SetDialogBooleanSubstitutionComplexSuccess()
		{
			var state = new ParserState(
				": That's { (true and false) or true }.", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "That's true."), node);
		}

		#endregion

		#region Math Substitution

		[Test]
		public static void SetDialogMathSubstitutionSuccess()
		{
			var state = new ParserState(": {1+2} berries please.", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "3 berries please."), node);
		}

		[Test]
		public static void SetDialogMathSubstitutionMultiLineSuccess()
		{
			var state = new ParserState(": {\n 1\n +\n 2} berries please.", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "3 berries please."), node);
		}

		[Test]
		public static void SetDialogMathSubstitutionComplexSuccess()
		{
			var state = new ParserState(": {(1+2) * 5} berries please.", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "15 berries please."), node);
		}

		#endregion

		#region Quote Substitution

		[Test]
		public static void SetDialogQuoteSubstitutionSuccess()
		{
			var state = new ParserState(": Hello {\"world!\"}", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SetDialogQuoteSubstitutionMultiLineSuccess()
		{
			var state = new ParserState(": Hello {\n \"world!\"\n }", 4);

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(new SetDialogNode(null, "Hello world!"), node);
		}

		#endregion

		#region Function Substitution

		[Test]
		public static void Function0Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number
			);

			var state = new ParserState(": { foobar() } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int>("foobar", () => { return 2; });

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(node, new SetDialogNode(null,
				new ConcatExpression(
					new ToStringExpression(new NumberFunction("foobar")),
					new StringLiteral(" berries please.")
				)
			));
		}

		[Test]
		public static void Function1Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number
			);

			var state = new ParserState(": { foobar(3) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int>("foobar", (i) => { return i; });

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(node, new SetDialogNode(null,
				new ConcatExpression(
					new ToStringExpression(new NumberFunction("foobar",
						new NumberLiteral(3)
					)),
					new StringLiteral(" berries please.")
				)
			));
		}

		[Test]
		public static void Function2Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number,
				ValueType.Number
			);

			var state = new ParserState(": { foobar(3, 4) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int>("foobar", (i, j) => { return i + j; });

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(node, new SetDialogNode(null,
				new ConcatExpression(
					new ToStringExpression(new NumberFunction("foobar",
						new NumberLiteral(3),
						new NumberLiteral(4)
					)),
					new StringLiteral(" berries please.")
				)
			));
		}

		[Test]
		public static void Function3Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number,
				ValueType.Number,
				ValueType.Number
			);

			var state = new ParserState(": { foobar(3, 4, 5) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int, int>("foobar", (i, j, k) => { return i + j + k; });

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(node, new SetDialogNode(null,
				new ConcatExpression(
					new ToStringExpression(new NumberFunction("foobar",
						new NumberLiteral(3),
						new NumberLiteral(4),
						new NumberLiteral(5)
					)),
					new StringLiteral(" berries please.")
				)
			));
		}

		[Test]
		public static void Function4Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number,
				ValueType.Number,
				ValueType.Number,
				ValueType.Number
			);

			var state = new ParserState(": { foobar(3, 4, 5, 6) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int, int, int>("foobar", (i, j, k, l) => { return i + j + k + l; });

			var node = Compiler.SetDialog(state);
			Assert.AreEqual(node, new SetDialogNode(null,
				new ConcatExpression(
					new ToStringExpression(new NumberFunction("foobar",
						new NumberLiteral(3),
						new NumberLiteral(4),
						new NumberLiteral(5),
						new NumberLiteral(6)
					)),
					new StringLiteral(" berries please.")
				)
			));
		}

		#endregion
	}
}
