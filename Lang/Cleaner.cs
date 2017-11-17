using System.Collections.Generic;

namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// The cleaner is responsible for replacing, removing, and converting
	/// tokens into logical tokens.
	/// </summary>
	public class Cleaner
	{
		/// <summary>
		/// True if empty lines should be discarded.
		/// </summary>
		private readonly bool discardEmptyLines;

		/// <summary>
		/// A list of tokens that should be replaced.
		/// </summary>
		private readonly Dictionary<string, string> replaces;

		/// <summary>
		/// A list of tokens that should be removed.
		/// </summary>
		private readonly HashSet<string> removes;

		/// <summary>
		/// A list of tokens that should be replaced with one instance of
		/// itself if it appears in a consecutive sequence of tokens.
		/// </summary>
		private readonly HashSet<string> duplicates;

		/// <summary>
		/// A list of tokens that should be treated as the newline character.
		/// </summary>
		private readonly HashSet<string> newlines;

		/// <summary>
		/// Creates a new Cleaner.
		/// </summary>
		/// <param name="discardEmptyLines">
		/// True if empty lines should be discarded.
		/// </param>
		private Cleaner(
			bool discardEmptyLines,
			Dictionary<string, string> replaces,
			IEnumerable<string> removes,
			IEnumerable<string> duplicates,
			IEnumerable<string> newlines)
		{
			this.discardEmptyLines = discardEmptyLines;
			this.replaces = new Dictionary<string, string>(replaces);
			this.removes = new HashSet<string>(removes);
			this.duplicates = new HashSet<string>(duplicates);
			this.newlines = new HashSet<string>(newlines);
		}

		/// <summary>
		/// Converts a list of tokens into logical tokens.
		/// </summary>
		/// <param name="tokens">The tokens to convert.</param>
		/// <returns>The logical tokens.</returns>
		public IEnumerable<LogicalToken> Clean(IEnumerable<string> tokens)
		{
			var logicalTokens = new List<LogicalToken>();

			int row = 0;
			int col = 0;
			foreach (var str in tokens) {
				string token = str;
				int length = token.Length;

				// Check if this token should be replaced
				if (replaces.ContainsKey(token)) {
					token = replaces[token];
				}

				// Check if this token should be ignored
				if (removes.Contains(token)) {
					col += length;
					continue;
				}

				// Check if this token is duplicated
				if (duplicates.Contains(token) && logicalTokens.Count > 0) {
					var last = logicalTokens[logicalTokens.Count - 1].text;
					if (token == last) {
						if (newlines.Contains(token)) {
							row++;
							col = 0;
						}
						else {
							col += length;
						}
						continue;
					}
				}

				// Check if this token is a newline
				if (newlines.Contains(token)) {

					// Discard this line if it is empty...
					if (discardEmptyLines) {

						// ...and the file is empty
						if (logicalTokens.Count == 0) {
							row++;
							col = 0;
							continue;
						}

						// ...or if the previous token was a newline
						var last = logicalTokens[logicalTokens.Count - 1].text;
						if (newlines.Contains(last)) {
							row++;
							col = 0;
							continue;
						}
					}

					logicalTokens.Add(new LogicalToken(token, row, col));
					row++;
					col = 0;
					continue;
				}

				// Add the token
				logicalTokens.Add(new LogicalToken(token, row, col));
				col += length;
				continue;
			}

			return logicalTokens;
		}

		/// <summary>
		/// A factory for constructing new cleaner object with.
		/// </summary>
		public class Factory
		{
			private bool discardEmptyLines;
			private Dictionary<string, string> replaces;
			private List<string> removes;
			private List<string> duplicates;
			private List<string> newlines;

			/// <summary>
			/// Creates a new factory to generate cleaner objects with.
			/// </summary>
			public Factory()
			{
				this.discardEmptyLines = true;
				this.replaces = new Dictionary<string, string>();
				this.removes = new List<string>();
				this.duplicates = new List<string>();
				this.newlines = new List<string>();
			}

			/// <summary>
			/// Instantiates a new cleaner object.
			/// </summary>
			public Cleaner Instantiate()
			{
				return new Cleaner(
					discardEmptyLines,
					replaces,
					removes,
					duplicates,
					newlines
				);
			}

			/// <summary>
			/// Sets the discard empty space setting.
			/// </summary>
			/// <param name="removeWhitespace">
			/// True if empty lines should be discarded.
			/// </param>
			/// <returns>This object for chaining</returns>
			public Factory SetDiscardEmptyLines(bool discardEmptyLines)
			{
				this.discardEmptyLines = discardEmptyLines;
				return this;
			}

			/// <summary>
			/// Adds a token that should be replaced.
			/// </summary>
			/// <param name="replace">
			/// The token that should be replaced.
			/// </param>
			/// <param name="substitution">
			/// The token that should be substituted.
			/// </param>
			/// <returns>This object for chaining.</returns>
			public Factory AddReplace(string replace, string substitution)
			{
				this.replaces.Add(replace, substitution);
				return this;
			}

			/// <summary>
			/// Adds a token that should be removed.
			/// </summary>
			/// <param name="remove">
			/// The token that should be removed.
			/// </param>
			/// <returns>This object for chaining.</returns>
			public Factory AddRemove(string remove)
			{
				this.removes.Add(remove);
				return this;
			}

			/// <summary>
			/// Adds a token that should be replaced with one instance of
			/// itself if it appears in a consecutive sequence of tokens.
			/// </summary>
			/// <param name="duplicate">
			/// The token that should be replaced.
			/// </param>
			/// <returns>This object for chaining.</returns>
			public Factory AddDuplicate(string duplicate)
			{
				this.duplicates.Add(duplicate);
				return this;
			}

			/// <summary>
			/// Adds a token that should be treated as the newline character.
			/// </summary>
			/// <param name="newline">
			/// The token that should be treated as a newline character.
			/// </param>
			/// <returns>This object for chaining.</returns>
			public Factory AddNewline(string newline)
			{
				this.newlines.Add(newline);
				return this;
			}
		}
	}
}
