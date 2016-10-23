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
	}
}
