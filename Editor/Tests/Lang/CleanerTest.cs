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

			Assert.AreEqual(tokens.Count, 2);

			Assert.AreEqual(tokens[0].text, "a");
			Assert.AreEqual(tokens[0].row, 0);
			Assert.AreEqual(tokens[0].col, 0);

			Assert.AreEqual(tokens[1].text, "c");
			Assert.AreEqual(tokens[1].row, 0);
			Assert.AreEqual(tokens[1].col, 2);
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

			Assert.AreEqual(tokens.Count, 0);
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

			Assert.AreEqual(tokens.Count, 1);

			Assert.AreEqual(tokens[0].text, "\n");
			Assert.AreEqual(tokens[0].row, 0);
			Assert.AreEqual(tokens[0].col, 0);
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

			Assert.AreEqual(tokens.Count, 3);

			Assert.AreEqual(tokens[0].text, "\n");
			Assert.AreEqual(tokens[0].row, 0);
			Assert.AreEqual(tokens[0].col, 0);

			Assert.AreEqual(tokens[1].text, "\n");
			Assert.AreEqual(tokens[1].row, 1);
			Assert.AreEqual(tokens[1].col, 0);

			Assert.AreEqual(tokens[2].text, "\n");
			Assert.AreEqual(tokens[2].row, 2);
			Assert.AreEqual(tokens[2].col, 0);
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

			Assert.AreEqual(tokens.Count, 0);
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

			Assert.AreEqual(tokens.Count, 1);

			Assert.AreEqual(tokens[0].text, "b");
			Assert.AreEqual(tokens[0].row, 6);
			Assert.AreEqual(tokens[0].col, 0);
		}
	}
}
