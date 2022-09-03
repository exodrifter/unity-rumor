using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class AppendDialog
	{
		#region Node

		[Test]
		public static void AppendDialogLineSuccess()
		{
			var state = new ParserState("+ Hello world!", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AppendDialogSpeakerLineSuccess()
		{
			var state = new ParserState("alice+ Hello world!", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void AppendDialogMultiLineSuccess()
		{
			var state = new ParserState("+ Hello\n  world!", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AppendDialogSpeakerMultiLineSuccess()
		{
			var state = new ParserState("alice+ Hello\n  world!", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void AppendDialogNextMultiLineSuccess()
		{
			var state = new ParserState("+\n Hello\n  world!", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AppendDialogSpeakerNextMultiLineSuccess()
		{
			var state = new ParserState("alice+\n Hello\n  world!", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode("alice", "Hello world!"), node);
		}

		#endregion

		#region Boolean Substitution

		[Test]
		public static void AppendDialogBooleanSubstitutionSuccess()
		{
			var state = new ParserState("+ That's { true or false }.", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "That's true."), node);
		}

		[Test]
		public static void AppendDialogBooleanSubstitutionMultiLineSuccess()
		{
			var state = new ParserState("+ That's { true\n or\n false }.", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "That's true."), node);
		}

		[Test]
		public static void AppendDialogBooleanSubstitutionComplexSuccess()
		{
			var state = new ParserState(
				"+ That's { (true and false) or true }.", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "That's true."), node);
		}

		#endregion

		#region Math Substitution

		[Test]
		public static void AppendDialogMathSubstitutionSuccess()
		{
			var state = new ParserState("+ {1+2} berries please.", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "3 berries please."), node);
		}

		[Test]
		public static void AppendDialogMathSubstitutionMultiLineSuccess()
		{
			var state = new ParserState("+ {\n 1\n +\n 2} berries please.", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "3 berries please."), node);
		}

		[Test]
		public static void AppendDialogMathSubstitutionComplexSuccess()
		{
			var state = new ParserState("+ {(1+2) * 5} berries please.", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "15 berries please."), node);
		}

		#endregion

		#region Quote Substitution

		[Test]
		public static void AppendDialogQuoteSubstitutionSuccess()
		{
			var state = new ParserState("+ Hello {\"world!\"}", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AppendDialogQuoteSubstitutionMultiLineSuccess()
		{
			var state = new ParserState("+ Hello {\n \"world!\"\n }", 4);

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(new AppendDialogNode(null, "Hello world!"), node);
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

			var state = new ParserState("+ { foobar() } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int>("foobar", () => { return 2; });

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(node, new AppendDialogNode(null,
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

			var state = new ParserState("+ { foobar(3) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int>("foobar", (i) => { return i; });

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(node, new AppendDialogNode(null,
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

			var state = new ParserState("+ { foobar(3, 4) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int>("foobar", (i, j) => { return i + j; });

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(node, new AppendDialogNode(null,
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

			var state = new ParserState("+ { foobar(3, 4, 5) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int, int>("foobar", (i, j, k) => { return i + j + k; });

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(node, new AppendDialogNode(null,
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

			var state = new ParserState("+ { foobar(3, 4, 5, 6) } berries please.", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int, int, int>("foobar", (i, j, k, l) => { return i + j + k + l; });

			var node = Compiler.AppendDialog(state);
			Assert.AreEqual(node, new AppendDialogNode(null,
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
