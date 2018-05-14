#if UNITY_EDITOR

using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Language;
using Exodrifter.Rumor.Nodes;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Lang
{
	internal sealed class CompilerTest
	{
		#region Add

		/// <summary>
		/// Checks if add statements with only dialog compile.
		/// </summary>
		[Test]
		public void CompileAddDialog()
		{
			var nodes = Compiler.Compile("add \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add bar");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);
		}

		/// <summary>
		/// Checks if add statements with speaker and dialog compile.
		/// </summary>
		[Test]
		public void CompileAddSpeakerDialog()
		{
			var nodes = Compiler.Compile("add \"foo\" \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add foo \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add foo + bar \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);
		}

		/// <summary>
		/// Checks if add statements compile with a trailing comment.
		/// </summary>
		[Test]
		public void CompileAddComment()
		{
			var nodes = Compiler.Compile("add bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add foo bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);
		}

		/// <summary>
		/// Checks if add statements compile with trailing whitespace.
		/// </summary>
		[Test]
		public void CompileAddWhitespace()
		{
			var nodes = Compiler.Compile("add bar  ");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNull((nodes[0] as Add).speaker);

			nodes = Compiler.Compile("add foo bar\t\t");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Add>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Add).speaker);
		}

		#endregion

		#region Say

		/// <summary>
		/// Checks if say statements with only dialog compile.
		/// </summary>
		[Test]
		public void CompileSayDialog()
		{
			var nodes = Compiler.Compile("say \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say bar");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);
		}

		/// <summary>
		/// Checks if say statements with speaker and dialog compile.
		/// </summary>
		[Test]
		public void CompileSaySpeakerDialog()
		{
			var nodes = Compiler.Compile("say \"foo\" \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say foo \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say foo + bar \"Hello world\"");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);
		}

		/// <summary>
		/// Checks if say statements compile with a trailing comment.
		/// </summary>
		[Test]
		public void CompileSayComment()
		{
			var nodes = Compiler.Compile("say bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say foo bar # comment");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);
		}

		/// <summary>
		/// Checks if say statements compile with trailing whitespace.
		/// </summary>
		[Test]
		public void CompileSayWhitespace()
		{
			var nodes = Compiler.Compile("say bar  ");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNull((nodes[0] as Say).speaker);

			nodes = Compiler.Compile("say foo bar\t\t");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Say>(nodes[0]);
			Assert.IsNotNull((nodes[0] as Say).speaker);
		}

		#endregion

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

		/// <summary>
		/// Checks if 'if', 'elif', and 'else' statements compile.
		/// <summary>
		[Test]
		public void CompileControlFlow()
		{
			var nodes = Compiler.Compile(@"
if foo:
	pause
elif bar:
	pause
else:
	pause
");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Condition>(nodes[0]);

			nodes = Compiler.Compile(@"
if foo:
	pause
else:
	pause
");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Condition>(nodes[0]);

			nodes = Compiler.Compile(@"
if foo:
	pause

else:
	pause
");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Condition>(nodes[0]);

			nodes = Compiler.Compile(@"
if foo:
	pause

elif bar:
	pause

else:
	pause
");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Condition>(nodes[0]);

			nodes = Compiler.Compile(@"
if foo:
# comment
	pause

	
elif bar:
	pause

   # comment
elif car:
	pause

# comment
else:
	pause
");
			Assert.AreEqual(1, nodes.Count);
			Assert.IsAssignableFrom<Condition>(nodes[0]);
		}
	}
}

#endif
