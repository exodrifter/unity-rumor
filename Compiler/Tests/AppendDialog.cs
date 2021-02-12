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
	}
}
