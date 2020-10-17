using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Say
	{
		#region Node

		[Test]
		public static void SayLineSuccess()
		{
			var state = new ParserState(": Hello world!", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SaySpeakerLineSuccess()
		{
			var state = new ParserState("alice: Hello world!", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SayMultiLineSuccess()
		{
			var state = new ParserState(": Hello\n  world!", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SaySpeakerMultiLineSuccess()
		{
			var state = new ParserState("alice: Hello\n  world!", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SayNextMultiLineSuccess()
		{
			var state = new ParserState(":\n Hello\n  world!", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SaySpeakerNextMultiLineSuccess()
		{
			var state = new ParserState("alice:\n Hello\n  world!", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode("alice", "Hello world!"), node);
		}

		#endregion

		#region Boolean Substitution

		[Test]
		public static void SayBooleanSubstitutionSuccess()
		{
			var state = new ParserState(": That's { true or false }.", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "That's true."), node);
		}

		[Test]
		public static void SayBooleanSubstitutionMultiLineSuccess()
		{
			var state = new ParserState(": That's { true\n or\n false }.", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "That's true."), node);
		}

		[Test]
		public static void SayBooleanSubstitutionComplexSuccess()
		{
			var state = new ParserState(
				": That's { (true and false) or true }.", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "That's true."), node);
		}

		#endregion

		#region Math Substitution

		[Test]
		public static void SayMathSubstitutionSuccess()
		{
			var state = new ParserState(": {1+2} berries please.", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "3 berries please."), node);
		}

		[Test]
		public static void SayMathSubstitutionMultiLineSuccess()
		{
			var state = new ParserState(": {\n 1\n +\n 2} berries please.", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "3 berries please."), node);
		}

		[Test]
		public static void SayMathSubstitutionComplexSuccess()
		{
			var state = new ParserState(": {(1+2) * 5} berries please.", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "15 berries please."), node);
		}

		#endregion

		#region Quote Substitution

		[Test]
		public static void SayQuoteSubstitutionSuccess()
		{
			var state = new ParserState(": Hello {\"world!\"}", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SayQuoteSubstitutionMultiLineSuccess()
		{
			var state = new ParserState(": Hello {\n \"world!\"\n }", 4, 0);

			var node = Compiler.Say(state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		#endregion
	}
}
