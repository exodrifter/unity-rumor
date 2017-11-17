using System;
using System.Collections.Generic;
using System.Linq;

namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// The tokenizer takes an input string and converts it into a series of
	/// tokens.
	/// 
	/// The Tokenizer is forbidden from dropping or changing information.
	/// This means that every character that appears in the input will appear
	/// again in one of the tokens.
	/// </summary>
	public class Tokenizer
	{
		/// <summary>
		/// The keywords to treat as tokens.
		/// </summary>
		List<string> keywords;
		HashSet<string> keywordsHashset;
		int longestKeywordLength;

		/// <summary>
		/// Creates a new tokenizer. Any sequence of characters that is not
		/// one of the keywords will be automatically treated as a token.
		/// </summary>
		/// <param name="keywords">The keywords to treat as tokens.</param>
		/// <param name="regex">The regex matches to treat as tokens.</param>
		public Tokenizer(IEnumerable<string> keywords)
		{
			this.keywords = new List<string>(keywords.OrderByDescending(w => w.Length));
			keywordsHashset = new HashSet<string>(keywords);

			longestKeywordLength = 0;
			foreach (var keyword in keywords)
			{
				if (keyword.Length > longestKeywordLength)
				{
					longestKeywordLength = keyword.Length;
				}
			}
		}

		/// <summary>
		/// Converts the input string into a series of tokens.
		/// </summary>
		/// <param name="input">The input to convert into tokens.</param>
		/// <returns>The tokens.</returns>
		public IEnumerable<string> Tokenize(string input)
		{
			var tokenBuffer = new LinkedList<string>();
			tokenBuffer.AddLast(input);

			foreach (var keyword in keywords) {

				var currentBuffer = new LinkedList<string>();
				var separator = new string[] { keyword };
				var splitOption = StringSplitOptions.None;

				// For each item in our token buffer...
				foreach (var token in tokenBuffer) {

					// If the item is a keyword, ignore it
					if (token.Length <= longestKeywordLength && keywordsHashset.Contains(token)) {
						currentBuffer.AddLast(token);
						continue;
					}

					// Otherwise, try to split it into pieces
					string[] parts = token.Split(separator, splitOption);
					foreach (var part in parts) {
						if (part != "") {
							currentBuffer.AddLast(part);
						}
						currentBuffer.AddLast(keyword);
					}
					currentBuffer.RemoveLast();
				}

				// Update the token buffer
				tokenBuffer = currentBuffer;
			}

			return tokenBuffer;
		}
	}
}
