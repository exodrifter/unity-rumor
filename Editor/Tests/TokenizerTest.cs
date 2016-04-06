using Exodrifter.Rumor.Lang;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test
{
	internal class TokenizerTest
	{
		/// <summary>
		/// Checks if the tokenizer works properly when the keyword input has
		/// duplicate entries.
		/// </summary>
		[Test]
		public void TokenizeDuplicate()
		{
			var keywords = new string[]
			{
				"\n", " ", " ", " ", " ",
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = " \r\n   ";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 6);
			Assert.AreEqual(tokens[0], " ");
			Assert.AreEqual(tokens[1], "\r");
			Assert.AreEqual(tokens[2], "\n");
			Assert.AreEqual(tokens[3], " ");
			Assert.AreEqual(tokens[4], " ");
			Assert.AreEqual(tokens[5], " ");
		}

		/// <summary>
		/// Checks if the tokenizer works properly when the keyword input is
		/// in the correct order.
		/// </summary>
		[Test]
		public void TokenizeForward()
		{
			var keywords = new string[]
			{
				"\r\n", "\n", "\r", "\t", " ",
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = " \r\n\r\t";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 4);
			Assert.AreEqual(tokens[0], " ");
			Assert.AreEqual(tokens[1], "\r\n");
			Assert.AreEqual(tokens[2], "\r");
			Assert.AreEqual(tokens[3], "\t");
		}

		/// <summary>
		/// Checks if the tokenizer works properly when the keyword input is
		/// in the reverse order.
		/// </summary>
		[Test]
		public void TokenizeReverse()
		{
			var keywords = new string[]
			{
				" ", "\t", "\r", "\n", "\r\n",
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = " \r\n\r\t";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 4);
			Assert.AreEqual(tokens[0], " ");
			Assert.AreEqual(tokens[1], "\r\n");
			Assert.AreEqual(tokens[2], "\r");
			Assert.AreEqual(tokens[3], "\t");
		}

		/// <summary>
		/// Checks if the tokenizer works properly when a single token is
		/// repeated in the input.
		/// </summary>
		[Test]
		public void TokenizeRepeated()
		{
			var keywords = new string[]
			{
				" ",
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = "    ";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 4);
			Assert.AreEqual(tokens[0], " ");
			Assert.AreEqual(tokens[1], " ");
			Assert.AreEqual(tokens[2], " ");
			Assert.AreEqual(tokens[3], " ");
		}

		/// <summary>
		/// Checks if the tokenizer works properly when a two overlapping
		/// tokens are repeated in the input.
		/// </summary>
		[Test]
		public void TokenizeRepeatedOverlap()
		{
			var keywords = new string[]
			{
				" ", "  "
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = "     ";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 3);
			Assert.AreEqual(tokens[0], "  ");
			Assert.AreEqual(tokens[1], "  ");
			Assert.AreEqual(tokens[2], " ");
		}

		/// <summary>
		/// Checks if the tokenizer works properly with a single token.
		/// </summary>
		[Test]
		public void TokenizeSingle()
		{
			var keywords = new string[]
			{
				" ",
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = "Hello, world!";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 3);
			Assert.AreEqual(tokens[0], "Hello,");
			Assert.AreEqual(tokens[1], " ");
			Assert.AreEqual(tokens[2], "world!");
		}

		/// <summary>
		/// Checks if the tokenizer works properly with a multiple tokens of
		/// the same length.
		/// </summary>
		[Test]
		public void TokenizeMultipleSimple()
		{
			var keywords = new string[]
			{
				" ", ",", "!"
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = "Hello, world!";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 5);
			Assert.AreEqual(tokens[0], "Hello");
			Assert.AreEqual(tokens[1], ",");
			Assert.AreEqual(tokens[2], " ");
			Assert.AreEqual(tokens[3], "world");
			Assert.AreEqual(tokens[4], "!");
		}

		/// <summary>
		/// Checks if the tokenizer works properly with multiple tokens of
		/// varying lengths.
		/// </summary>
		[Test]
		public void TokenizeMultipleVarying()
		{
			var keywords = new string[]
			{
				" ", "->", "float"
			};
			var tokenizer = new Tokenizer(keywords, new List<string>());

			var input = "float a -> float b";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(tokens.Count, 9);
			Assert.AreEqual(tokens[0], "float");
			Assert.AreEqual(tokens[1], " ");
			Assert.AreEqual(tokens[2], "a");
			Assert.AreEqual(tokens[3], " ");
			Assert.AreEqual(tokens[4], "->");
			Assert.AreEqual(tokens[5], " ");
			Assert.AreEqual(tokens[6], "float");
			Assert.AreEqual(tokens[7], " ");
			Assert.AreEqual(tokens[8], "b");
		}
	}
}
