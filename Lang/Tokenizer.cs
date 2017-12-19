using System.Collections.Generic;

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
			var tokenBuffer = new List<string>();

			string current = "";
			for (int pos = 0; pos < input.Length;)
			{
				bool foundToken = false;

				// Find the longest token starting at this position
				string token = null;
				for (int len = longestKeywordLength; len >= 0; --len)
				{
					if (pos + len > input.Length)
					{
						continue;
					}

					token = input.Substring(pos, len);
					if (keywordsHashset.Contains(token))
					{
						foundToken = true;
						break;
					}
				}

				// Add the token
				if (foundToken)
				{
					// Add non-token string before this token
					if (!string.IsNullOrEmpty(current))
					{
						tokenBuffer.Add(current);
						current = "";
					}

					tokenBuffer.Add(token);
					pos += token.Length;
				}
				// This character is not part of a non-token string
				else
				{
					current += input[pos];
					pos++;
				}
			}

			// Add remaining non-token string
			if (!string.IsNullOrEmpty(current))
			{
				tokenBuffer.Add(current);
			}
			return tokenBuffer;
		}
	}
}
