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

			var rumor = new Rumor.Engine.Rumor("return");
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual("This is a multiline string.", (nodes[0] as Say).EvaluateText(rumor));
		}

		[Test]
		public void MultilineStringTabs()
		{
			var str = "say \"This is a multiline\n\tstring.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var rumor = new Rumor.Engine.Rumor("return");
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual("This is a multiline string.", (nodes[0] as Say).EvaluateText(rumor));
		}

		[Test]
		public void MultilineStringMixed()
		{
			var str = "say \"This is a multiline\n  \t  \tstring.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var rumor = new Rumor.Engine.Rumor("return");
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual("This is a multiline string.", (nodes[0] as Say).EvaluateText(rumor));
		}

		[Test]
		public void MultilineStringPreserveSpaces()
		{
			var str = "say \"This  is  a  multiline\n\tstring.\"";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var rumor = new Rumor.Engine.Rumor("return");
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual("This  is  a  multiline string.", (nodes[0] as Say).EvaluateText(rumor));
		}

		[Test]
		public void PauseAcceptsFloat()
		{
			var rumor = new Rumor.Engine.Rumor("return");

			var str = "pause 0";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(0f, (nodes[0] as Pause).seconds.Evaluate(rumor).AsFloat());

			str = "pause .5";
			nodes = new List<Node>(new RumorCompiler().Compile(str));
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(0.5f, (nodes[0] as Pause).seconds.Evaluate(rumor).AsFloat());

			str = "pause 0.5";
			nodes = new List<Node>(new RumorCompiler().Compile(str));
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(0.5f, (nodes[0] as Pause).seconds.Evaluate(rumor).AsFloat());

			str = "pause 1";
			nodes = new List<Node>(new RumorCompiler().Compile(str));
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(1f, (nodes[0] as Pause).seconds.Evaluate(rumor).AsFloat());
		}

		[Test]
		public void PauseAcceptsExpression()
		{
			var str = "pause foo()";
			var nodes = new List<Node>(new RumorCompiler().Compile(str));

			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Bind("foo", () => { return 1f; });
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(1f, (nodes[0] as Pause).seconds.Evaluate(rumor).AsFloat());

			str = "pause 8.0 * 3.0";
			nodes = new List<Node>(new RumorCompiler().Compile(str));

			rumor = new Rumor.Engine.Rumor("return");
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(24f, (nodes[0] as Pause).seconds.Evaluate(rumor).AsFloat());
		}
	}
}
