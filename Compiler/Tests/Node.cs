using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Node
	{
		#region Say

		[Test]
		public static void SayLineSuccess()
		{
			var state = new State(": Hello world!", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SaySpeakerLineSuccess()
		{
			var state = new State("alice: Hello world!", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SayMultiLineSuccess()
		{
			var state = new State(": Hello\n  world!", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SaySpeakerMultiLineSuccess()
		{
			var state = new State("alice: Hello\n  world!", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SayNextMultiLineSuccess()
		{
			var state = new State(":\n Hello\n  world!", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode(null, "Hello world!"), node);
		}

		[Test]
		public static void SaySpeakerNextMultiLineSuccess()
		{
			var state = new State("alice:\n Hello\n  world!", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode("alice", "Hello world!"), node);
		}

		[Test]
		public static void SaySubstitutionSuccess()
		{
			var state = new State(": {1+2} berries please.", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode(null, "3 berries please."), node);
		}

		[Test]
		public static void SaySubstitutionWhitespaceSuccess()
		{
			var state = new State(": {\n 1\n +\n 2} berries please.", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode(null, "3 berries please."), node);
		}

		[Test]
		public static void SaySubstitutionComplexMathSuccess()
		{
			var state = new State(": {(1+2) * 5} berries please.", 4, 0);

			var node = Compiler.SayNode()(ref state);
			Assert.AreEqual(new SayNode(null, "15 berries please."), node);
		}

		#endregion
	}
}
