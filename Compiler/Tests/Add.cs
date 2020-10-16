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
			var state = new State("+ Hello world!", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddSpeakerLineSuccess()
		{
			var state = new State("alice+ Hello world!", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void AddMultiLineSuccess()
		{
			var state = new State("+ Hello\n  world!", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddSpeakerMultiLineSuccess()
		{
			var state = new State("alice+ Hello\n  world!", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void AddNextMultiLineSuccess()
		{
			var state = new State("+\n Hello\n  world!", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddSpeakerNextMultiLineSuccess()
		{
			var state = new State("alice+\n Hello\n  world!", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode("alice", "Hello world!"), node);
		}

		#endregion

		#region Boolean Substitution

		[Test]
		public static void AddBooleanSubstitutionSuccess()
		{
			var state = new State("+ That's { true or false }.", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "That's true."), node);
		}

		[Test]
		public static void AddBooleanSubstitutionMultiLineSuccess()
		{
			var state = new State("+ That's { true\n or\n false }.", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "That's true."), node);
		}

		[Test]
		public static void AddBooleanSubstitutionComplexSuccess()
		{
			var state = new State(
				"+ That's { (true and false) or true }.", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "That's true."), node);
		}

		#endregion

		#region Math Substitution

		[Test]
		public static void AddMathSubstitutionSuccess()
		{
			var state = new State("+ {1+2} berries please.", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "3 berries please."), node);
		}

		[Test]
		public static void AddMathSubstitutionMultiLineSuccess()
		{
			var state = new State("+ {\n 1\n +\n 2} berries please.", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "3 berries please."), node);
		}

		[Test]
		public static void AddMathSubstitutionComplexSuccess()
		{
			var state = new State("+ {(1+2) * 5} berries please.", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "15 berries please."), node);
		}

		#endregion

		#region Quote Substitution

		[Test]
		public static void AddQuoteSubstitutionSuccess()
		{
			var state = new State("+ Hello {\"world!\"}", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "Hello world!"), node);
		}

		[Test]
		public static void AddQuoteSubstitutionMultiLineSuccess()
		{
			var state = new State("+ Hello {\n \"world!\"\n }", 4, 0);

			var node = Compiler.Add(state);
			Assert.AreEqual(new AddNode(null, "Hello world!"), node);
		}

		#endregion
	}
}
