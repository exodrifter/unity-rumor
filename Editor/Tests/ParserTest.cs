using Exodrifter.Rumor.Lang;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Exodrifter.Rumor.Test
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

			Assert.AreEqual(lines.Count, 4);
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

			Assert.AreEqual(lines.Count, 1);
			Assert.AreEqual(lines[0].tokens.Count, tokens.Count);
		}
	}
}
