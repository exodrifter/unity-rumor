#if UNITY_EDITOR

using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Language;
using Exodrifter.Rumor.Nodes;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Lang
{
	internal sealed class CompilerTest
	{
		/// <summary>
		/// Checks if add statements compile.
		/// </summary>
		[Test]
		public void CompileAdd()
		{
			var nodes = Compiler.Compile("add bar");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add \"foo\" \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add foo + bar \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add foo bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);
		}

		/// <summary>
		/// Checks if say statements compile.
		/// </summary>
		[Test]
		public void CompileSay()
		{
			var nodes = Compiler.Compile("say bar");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say \"foo\" \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say foo + bar \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say foo bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);
		}

		/// <summary>
		/// Checks if '$' statements compile.
		/// <summary>
		[Test]
		public void CompileStatement()
		{
			var nodes = Compiler.Compile("$ foo + bar");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Statement>(nodes[0]);
			Assert.IsAssignableFrom<AddExpression>(
				(nodes[0] as Statement).expression);

			nodes = Compiler.Compile("$ 1 + 2 * 3 + 4");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Statement>(nodes[0]);
			Assert.IsAssignableFrom<MultiplyExpression>(
				(nodes[0] as Statement).expression);
		}
	}
}

#endif
