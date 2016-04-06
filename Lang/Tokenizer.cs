using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
		IEnumerable<string> keywords;

		/// <summary>
		/// The regex matches to treat as tokens.
		/// </summary>
		IEnumerable<string> regex;

		/// <summary>
		/// Creates a new tokenizer. Any sequence of characters that is not
		/// one of the keywords will be automatically treated as a token.
		/// </summary>
		/// <param name="keywords">The keywords to treat as tokens.</param>
		/// <param name="regex">The regex matches to treat as tokens.</param>
		public Tokenizer(IEnumerable<string> keywords, IEnumerable<string> regex)
		{
			this.keywords = keywords.OrderByDescending(w => w.Length);
			this.regex = regex;
		}

		/// <summary>
		/// Converts the input string into a series of tokens.
		/// </summary>
		/// <param name="input">The input to convert into tokens.</param>
		/// <returns>The tokens.</returns>
		public IEnumerable<string> Tokenize(string input)
		{
			var tokenBuffer = new List<string>() { input };

			// Tokenize based on regex
			foreach (var r in regex) {

				var currentBuffer = new List<string>();
				var reg = new Regex(r);

				// Try to split each item in our token buffer
				foreach (var token in tokenBuffer) {
					currentBuffer.AddRange(reg.Split(token));
				}

				// Update the token buffer
				tokenBuffer = currentBuffer;
			}

			// Tokenize based on keywords
			foreach (var keyword in keywords) {

				var currentBuffer = new List<string>();
				var separator = new string[] { keyword };
				var splitOption = StringSplitOptions.None;

				// For each item in our token buffer...
				foreach (var token in tokenBuffer) {

					// If the item is a keyword, ignore it
					if (keywords.Contains(token)) {
						currentBuffer.Add(token);
						continue;
					}

					// If the item is a regex match, ignore it
					bool regexMatched = false;
					foreach (var r in regex) {
						if (new Regex(r).IsMatch(token)) {
							currentBuffer.Add(token);
							regexMatched = true;
							break;
						}
					}
					if (regexMatched) {
						continue;
					}

					// Otherwise, try to split it into pieces
					string[] parts = token.Split(separator, splitOption);
					foreach (var part in parts) {
						if (part != "") {
							currentBuffer.Add(part);
						}
						currentBuffer.Add(keyword);
					}
					currentBuffer.RemoveAt(currentBuffer.Count - 1);
				}

				// Update the token buffer
				tokenBuffer = currentBuffer;
			}

			return tokenBuffer;
		}
	}
}
