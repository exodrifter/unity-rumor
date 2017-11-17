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
			int length = 1;
			for (int pos = 1; pos <= input.Length; ++pos)
			{
				bool foundToken = false;
				for (int l = length; l > 0; --l)
				{
					var token = input.Substring(pos - l, l);
					if (keywordsHashset.Contains(token))
					{
						var pre = current + input.Substring(pos - length, length - l);
						if (!string.IsNullOrEmpty(pre))
						{
							tokenBuffer.Add(pre);
						}

						tokenBuffer.Add(token);
						current = "";
						foundToken = true;
						break;
					}
				}

				if (foundToken)
				{
					length = 1;
				}
				else
				{
					if (length == longestKeywordLength)
					{
						current += input[pos - longestKeywordLength];
					}
					else
					{
						length++;
					}
				}
			}

			return tokenBuffer;
		}
	}
}
