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
	}
}
