using System.Collections.Generic;
using System.Linq;

namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// The Parser takes a list of tokens and transforms them into logical
	/// lines and logical tokens.
	/// </summary>
	public class Parser
	{
		/// <summary>
		/// A list of ignored tokens.
		/// </summary>
		private readonly List<string> ignored;

		/// <summary>
		/// A list of terminator tokens.
		/// </summary>
		private readonly List<string> terminators;

		/// <summary>
		/// A list of delimiter tokens.
		/// </summary>
		private readonly List<Delimiter> delimiters;

		/// <summary>
		/// Creates a new Parser
		/// </summary>
		private Parser(
			List<string> ignored,
			List<string> terminators,
			List<Delimiter> delimiters)
		{
			this.ignored = new List<string>(ignored);
			this.terminators = new List<string>(terminators);
			this.delimiters = new List<Delimiter>(delimiters);
		}

		public IEnumerable<LogicalLine> Parse(IEnumerable<LogicalToken> tokens)
		{
			var lines = new List<LogicalLine>();

			var tokenBuffer = new List<LogicalToken>();
			var iter = tokens.GetEnumerator();
			while (iter.MoveNext()) {

				// Check if the token should be ignored
				if (ignored.Contains(iter.Current.text)) {
					continue;
				}
				else {
					// Otherwise, build the buffer
					tokenBuffer.Add(iter.Current);
				}

				// Check if a terminator was found
				if (terminators.Contains(iter.Current.text)) {
					lines.Add(new LogicalLine(tokenBuffer));
					tokenBuffer.Clear();
					continue;
				}

				// Check if a delimiter was found
				var delims = delimiters.Where
					(x => x.start == iter.Current.text);
				if (delims.Count() > 0) {
					var start = iter.Current;
					var delim = delims.First();

					var foundEnd = false;
					while (iter.MoveNext()) {
						tokenBuffer.Add(iter.Current);

						if (delim.escapes.Contains(iter.Current.text)) {
							iter.MoveNext();
							tokenBuffer.Add(iter.Current);
						}

						if (iter.Current.text == delim.end) {
							foundEnd = true;
							break;
						}
					}

					if (!foundEnd) {
						throw new CompilerError(start, string.Format(
							"Could not find the closing delimiter for the "
							+ "delimiter set {0}",
							delim));
					}
					continue;
				}
			}

			// Dump the remaining buffer into a logical line before returning
			if (tokenBuffer.Count > 0) {
				lines.Add(new LogicalLine(tokenBuffer));
			}
			return lines;
		}

		public class Factory
		{
			/// <summary>
			/// A list of ignored tokens.
			/// </summary>
			private List<string> ignored;

			/// <summary>
			/// A list of terminator tokens.
			/// </summary>
			private List<string> terminators;

			/// <summary>
			/// A list of delimiter tokens.
			/// </summary>
			private List<Delimiter> delimiters;

			/// <summary>
			/// Creates a new factory to generate parser objects with.
			/// </summary>
			public Factory()
			{
				ignored = new List<string>();
				terminators = new List<string>();
				delimiters = new List<Delimiter>();
			}

			/// <summary>
			/// Instantiates a new parser object.
			/// </summary>
			public Parser Instantiate()
			{
				return new Parser(ignored, terminators, delimiters);
			}

			/// <summary>
			/// Adds a token that should be ignored.
			/// </summary>
			/// <param name="ignore">The token to ignore.</param>
			/// <returns>This object for chaning.</returns>
			public Factory AddIgnore(string ignore)
			{
				ignored.Add(ignore);
				return this;
			}

			/// <summary>
			/// Adds a token that should be treated as a terminator.
			/// </summary>
			/// <param name="terminator">
			/// The token to treat as a terminator.
			/// </param>
			/// <returns>This object for chaning.</returns>
			public Factory AddTerminator(string terminator)
			{
				terminators.Add(terminator);
				return this;
			}

			/// <summary>
			/// Adds a token that should be treated as a delimiter.
			/// </summary>
			/// <param name="start">The start token of the delimiter.</param>
			/// <param name="end">The end token of the delimiter.</param>
			/// <returns>This object for chaning.</returns>
			public Factory AddDelimiter(string start, string end)
			{
				return AddDelimiter(start, end, new List<string>());
			}

			/// <summary>
			/// Adds a token that should be treated as a delimiter.
			/// </summary>
			/// <param name="start">The start token of the delimiter.</param>
			/// <param name="end">The end token of the delimiter.</param>
			/// <param name="escapes">
			/// The tokens to treat as the beginning of an escape sequence.
			/// </param>
			/// <returns>This object for chaning.</returns>
			public Factory AddDelimiter
				(string start, string end, IEnumerable<string> escapes)
			{
				delimiters.Add(new Delimiter(start, end, escapes));
				return this;
			}
		}

		/// <summary>
		/// A Delimiter is a helper class for the Parser to define behaviour
		/// for items such as brackets and quotes.
		/// </summary>
		private class Delimiter
		{
			/// <summary>
			/// The start of the delimiter.
			/// </summary>
			public readonly string start;

			/// <summary>
			/// The end of the delimiter.
			/// </summary>
			public readonly string end;

			/// <summary>
			/// A list of tokens that should be used as escape tokens.
			/// </summary>
			public readonly List<string> escapes;

			/// <summary>
			/// Creates a new Delimiter.
			/// </summary>
			/// <param name="start">
			/// The start of the delimiter.
			/// </param>
			/// <param name="end">
			/// The end of the delimiter.
			/// </param>
			/// <param name="escapes">
			/// A list of tokens that should be used as escape tokens.
			/// </param>
			public Delimiter
				(string start, string end, IEnumerable<string> escapes)
			{
				this.start = start;
				this.end = end;
				this.escapes = new List<string>(escapes);
			}

			public override string ToString()
			{
				return string.Format("[\"{0}\", \"{1}\"]", start, end);
			}
		}
	}
}
