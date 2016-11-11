using Exodrifter.Rumor.Lang;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Lang
{
	public class CleanerTest
	{
		/// <summary>
		/// Checks if the cleaner properly handles removed tokens.
		/// </summary>
		[Test]
		public void CleanRemoves()
		{
			var cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(true)
				.AddRemove("b")
				.Instantiate();

			var input = new List<string>()
			{
				"a",
				"b",
				"c",
			};

			var tokens = new List<LogicalToken>(cleaner.Clean(input));

			Assert.AreEqual(2, tokens.Count);

			Assert.AreEqual("a", tokens[0].text);
			Assert.AreEqual(0, tokens[0].row);
			Assert.AreEqual(0, tokens[0].col);

			Assert.AreEqual("c", tokens[1].text);
			Assert.AreEqual(0, tokens[1].row);
			Assert.AreEqual(2, tokens[1].col);
		}

		/// <summary>
		/// Checks if the cleaner properly handles the case where all tokens
		/// are removed.
		/// </summary>
		[Test]
		public void CleanRemovesAll()
		{
			var cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(true)
				.AddRemove("a")
				.Instantiate();

			var input = new List<string>()
			{
				"a",
				"a",
				"a",
			};

			var tokens = new List<LogicalToken>(cleaner.Clean(input));

			Assert.AreEqual(0, tokens.Count);
		}

		/// <summary>
		/// Checks if the cleaner properly handles duplicate tokens.
		/// </summary>
		[Test]
		public void CleanDuplicates()
		{
			var cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(true)
				.AddDuplicate("\n")
				.Instantiate();

			var input = new List<string>()
			{
				"\n",
				"\n",
				"\n",
			};

			var tokens = new List<LogicalToken>(cleaner.Clean(input));

			Assert.AreEqual(1, tokens.Count);

			Assert.AreEqual("\n", tokens[0].text);
			Assert.AreEqual(0, tokens[0].row);
			Assert.AreEqual(0, tokens[0].col);
		}

		/// <summary>
		/// Checks if the cleaner properly handles newline tokens.
		/// </summary>
		[Test]
		public void CleanNewlines()
		{
			var cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(false)
				.AddNewline("\n")
				.Instantiate();

			var input = new List<string>()
			{
				"\n",
				"\n",
				"\n",
			};

			var tokens = new List<LogicalToken>(cleaner.Clean(input));

			Assert.AreEqual(3, tokens.Count);

			Assert.AreEqual("\n", tokens[0].text);
			Assert.AreEqual(0, tokens[0].row);
			Assert.AreEqual(0, tokens[0].col);

			Assert.AreEqual("\n", tokens[1].text);
			Assert.AreEqual(1, tokens[1].row);
			Assert.AreEqual(0, tokens[1].col);

			Assert.AreEqual("\n", tokens[2].text);
			Assert.AreEqual(2, tokens[2].row);
			Assert.AreEqual(0, tokens[2].col);
		}

		/// <summary>
		/// Checks if the cleaner properly handles duplicates and newlines.
		/// </summary>
		[Test]
		public void CleanDuplicateAndNewline()
		{
			var cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(true)
				.AddDuplicate("\n")
				.AddNewline("\n")
				.Instantiate();

			var input = new List<string>()
			{
				"\n",
				"\n",
				"\n",
			};

			var tokens = new List<LogicalToken>(cleaner.Clean(input));

			Assert.AreEqual(0, tokens.Count);
		}

		/// <summary>
		/// Checks if the cleaner properly handles all directives at once.
		/// </summary>
		[Test]
		public void CleanMulti()
		{
			var cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(true)
				.AddReplace("\r", "\n")
				.AddRemove("a")
				.AddDuplicate("\n")
				.AddNewline("\n")
				.Instantiate();

			var input = new List<string>()
			{
				"a", "\r", "\r", "\n",
				"a", "a", "\r", "\r", "\n",
				"b",
			};

			var tokens = new List<LogicalToken>(cleaner.Clean(input));

			Assert.AreEqual(1, tokens.Count);

			Assert.AreEqual("b", tokens[0].text);
			Assert.AreEqual(6, tokens[0].row);
			Assert.AreEqual(0, tokens[0].col);
		}
	}
}
