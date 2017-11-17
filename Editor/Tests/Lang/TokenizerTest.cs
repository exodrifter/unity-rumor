﻿using Exodrifter.Rumor.Lang;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Lang
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
			var tokenizer = new Tokenizer(keywords);

			var input = " \r\n   ";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(6, tokens.Count);
			Assert.AreEqual(" ", tokens[0]);
			Assert.AreEqual("\r", tokens[1]);
			Assert.AreEqual("\n", tokens[2]);
			Assert.AreEqual(" ", tokens[3]);
			Assert.AreEqual(" ", tokens[4]);
			Assert.AreEqual(" ", tokens[5]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = " \r\n\r\t";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(4, tokens.Count);
			Assert.AreEqual(" ", tokens[0]);
			Assert.AreEqual("\r\n", tokens[1]);
			Assert.AreEqual("\r", tokens[2]);
			Assert.AreEqual("\t", tokens[3]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = " \r\n\r\t";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(4, tokens.Count);
			Assert.AreEqual(" ", tokens[0]);
			Assert.AreEqual("\r\n", tokens[1]);
			Assert.AreEqual("\r", tokens[2]);
			Assert.AreEqual("\t", tokens[3]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = "    ";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(4, tokens.Count);
			Assert.AreEqual(" ", tokens[0]);
			Assert.AreEqual(" ", tokens[1]);
			Assert.AreEqual(" ", tokens[2]);
			Assert.AreEqual(" ", tokens[3]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = "     ";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("  ", tokens[0]);
			Assert.AreEqual("  ", tokens[1]);
			Assert.AreEqual(" ", tokens[2]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = "Hello, world!";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("Hello,", tokens[0]);
			Assert.AreEqual(" ", tokens[1]);
			Assert.AreEqual("world!", tokens[2]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = "Hello, world!";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(5, tokens.Count);
			Assert.AreEqual("Hello", tokens[0]);
			Assert.AreEqual(",", tokens[1]);
			Assert.AreEqual(" ", tokens[2]);
			Assert.AreEqual("world", tokens[3]);
			Assert.AreEqual("!", tokens[4]);
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
			var tokenizer = new Tokenizer(keywords);

			var input = "float a -> float b";
			var tokens = new List<string>(tokenizer.Tokenize(input));

			Assert.AreEqual(9, tokens.Count);
			Assert.AreEqual("float", tokens[0]);
			Assert.AreEqual(" ", tokens[1]);
			Assert.AreEqual("a", tokens[2]);
			Assert.AreEqual(" ", tokens[3]);
			Assert.AreEqual("->", tokens[4]);
			Assert.AreEqual(" ", tokens[5]);
			Assert.AreEqual("float", tokens[6]);
			Assert.AreEqual(" ", tokens[7]);
			Assert.AreEqual("b", tokens[8]);
		}
	}
}
