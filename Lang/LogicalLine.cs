using System.Collections.Generic;

namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// A logical line is a sequence of tokens that belong to the same line.
	/// </summary>
	public class LogicalLine
	{
		/// <summary>
		/// The list of tokens in this logical line.
		/// </summary>
		public readonly List<LogicalToken> tokens;

		/// <summary>
		/// Creates a new logical line.
		/// </summary>
		/// <param name="tokens">
		/// The tokens in this logical line.
		/// </param>
		public LogicalLine(IEnumerable<LogicalToken> tokens)
		{
			this.tokens = new List<LogicalToken>(tokens);
		}

		public override string ToString()
		{
			var str = "";
			foreach (var token in tokens) {
				str += token.text;
			}
			return str;
		}
	}
}