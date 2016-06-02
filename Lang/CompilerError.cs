using System;

namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// Contains information for a compiler error.
	/// </summary>
	public class CompilerError : Exception
	{
		private readonly string message;

		/// <summary>
		/// The error message.
		/// </summary>
		public override string Message
		{
			get { return message; }
		}

		/// <summary>
		/// Creates a new compiler error about the specified line.
		/// </summary>
		/// <param name="line">
		/// The line where the compiler error occured.
		/// </param>
		/// <param name="message">
		/// The error message.
		/// </param>
		public CompilerError(LogicalLine line, string message)
		{
			string row = "?";
			if (line.tokens.Count > 0 && line.tokens[0].row != null) {
				row = "" + line.tokens[0].row;
			}

			this.message = "on line " + row + ": " + message;
		}

		/// <summary>
		/// Creates a new compile rerror about the specified token.
		/// </summary>
		/// <param name="token">
		/// The token where the compiler error occured.
		/// </param>
		/// <param name="message">
		/// The error message.
		/// </param>
		public CompilerError(LogicalToken token, string message)
		{
			string row = "?";
			string col = "?";
			if (token.row != null) {
				row = "" + token.row;
			}
			if (token.col != null) {
				col = "" + token.col;
			}

			this.message = "at (" + row + "," + col + "): " + message;
		}
	}
}
