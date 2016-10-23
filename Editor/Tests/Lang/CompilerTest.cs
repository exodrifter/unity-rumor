using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Lang;
using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Lang
{
	internal class CompilerTest
	{
		[Test]
		public void MultilineStringSpaces()
		{
			var str = "say \"This is a multiline\n      string.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Say).EvaluateText(scope), "This is a multiline string.");
		}

		[Test]
		public void MultilineStringTabs()
		{
			var str = "say \"This is a multiline\n\tstring.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Say).EvaluateText(scope), "This is a multiline string.");
		}

		[Test]
		public void MultilineStringMixed()
		{
			var str = "say \"This is a multiline\n  \t  \tstring.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Say).EvaluateText(scope), "This is a multiline string.");
		}

		[Test]
		public void MultilineStringPreserveSpaces()
		{
			var str = "say \"This  is  a  multiline\n\tstring.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Say).EvaluateText(scope), "This  is  a  multiline string.");
		}

		[Test]
		public void PauseAcceptsFloat()
		{
			var str = "pause 0";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Pause).seconds.Evaluate(scope).AsFloat(), 0f);

			str = "pause .5";
			nodes = new List<Node>(new RumorCompiler().Compile(str));

			scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Pause).seconds.Evaluate(scope).AsFloat(), 0.5f);

			str = "pause 0.5";
			nodes = new List<Node>(new RumorCompiler().Compile(str));

			scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Pause).seconds.Evaluate(scope).AsFloat(), 0.5f);

			str = "pause 1";
			nodes = new List<Node>(new RumorCompiler().Compile(str));

			scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Pause).seconds.Evaluate(scope).AsFloat(), 1f);
		}

		[Test]
		public void PauseAcceptsExpression()
		{
			var str = "pause foo()";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var scope = new Scope();
			scope.Bind("foo", () => { return 1f; });
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Pause).seconds.Evaluate(scope).AsFloat(), 1f);

			str = "pause 8.0 * 3.0";
			nodes = new List<Node>(new RumorCompiler().Compile(str));

			scope = new Scope();
			Assert.AreEqual(nodes.Count, 1);
			Assert.AreEqual((nodes[0] as Pause).seconds.Evaluate(scope).AsFloat(), 24f);
		}
	}
}
