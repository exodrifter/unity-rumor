using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Add
	{
		#region Node

		[Test]
		public static void AddLineSuccess()
		{
			var state = new ParserState("+ Hello world!", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddSpeakerLineSuccess()
		{
			var state = new ParserState("alice+ Hello world!", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void AddMultiLineSuccess()
		{
			var state = new ParserState("+ Hello\n  world!", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddSpeakerMultiLineSuccess()
		{
			var state = new ParserState("alice+ Hello\n  world!", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void AddNextMultiLineSuccess()
		{
			var state = new ParserState("+\n Hello\n  world!", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddSpeakerNextMultiLineSuccess()
		{
			var state = new ParserState("alice+\n Hello\n  world!", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode("alice", "Hello world!"), node);
		}

		#endregion

		#region Boolean Substitution

		[Test]
		public static void AddBooleanSubstitutionSuccess()
		{
			var state = new ParserState("+ That's { true or false }.", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "That's true."), node);
		}

		[Test]
		public static void AddBooleanSubstitutionMultiLineSuccess()
		{
			var state = new ParserState("+ That's { true\n or\n false }.", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "That's true."), node);
		}

		[Test]
		public static void AddBooleanSubstitutionComplexSuccess()
		{
			var state = new ParserState(
				"+ That's { (true and false) or true }.", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "That's true."), node);
		}

		#endregion

		#region Math Substitution

		[Test]
		public static void AddMathSubstitutionSuccess()
		{
			var state = new ParserState("+ {1+2} berries please.", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "3 berries please."), node);
		}

		[Test]
		public static void AddMathSubstitutionMultiLineSuccess()
		{
			var state = new ParserState("+ {\n 1\n +\n 2} berries please.", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "3 berries please."), node);
		}

		[Test]
		public static void AddMathSubstitutionComplexSuccess()
		{
			var state = new ParserState("+ {(1+2) * 5} berries please.", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "15 berries please."), node);
		}

		#endregion

		#region Quote Substitution

		[Test]
		public static void AddQuoteSubstitutionSuccess()
		{
			var state = new ParserState("+ Hello {\"world!\"}", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddQuoteSubstitutionMultiLineSuccess()
		{
			var state = new ParserState("+ Hello {\n \"world!\"\n }", 4, 0);

			var node = Compiler.Append(state);
			Assert.AreEqual(new AppendNode(null, "Hello world!"), node);
		}

		#endregion
	}
}
