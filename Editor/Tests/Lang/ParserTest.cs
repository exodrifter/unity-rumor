using Exodrifter.Rumor.Lang;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Exodrifter.Rumor.Test.Lang
{
	internal class ParserTest
	{
		[Test]
		public void Terminator()
		{
			var parser = new Parser.Factory()
				.AddTerminator(";\n")
				.Instantiate();

			var tokens = new List<string>()
			{
				"Hello", ";\n",
				",", ";\n",
				" ", ";\n",
				"world!", ";\n",
			};

			var lTokens = tokens.Select(x => new LogicalToken(x));
			var lines = new List<LogicalLine>(parser.Parse(lTokens));

			Assert.AreEqual(4, lines.Count);
		}

		[Test]
		public void TerminatorAndDelimiter()
		{
			var parser = new Parser.Factory()
				.AddTerminator("\n")
				.AddDelimiter("\"", "\"")
				.Instantiate();

			var tokens = new List<string>()
			{
				"\"", "Hello", ",", "\n", "world", "!", "\"",
				" ", "said", " ", "Alice", ".", "\n",
			};

			var lTokens = tokens.Select(x => new LogicalToken(x));
			var lines = new List<LogicalLine>(parser.Parse(lTokens));

			Assert.AreEqual(1, lines.Count);
			Assert.AreEqual(tokens.Count, lines[0].tokens.Count);
		}
	}
}
