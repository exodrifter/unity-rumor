using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from a missing close parenthesis for
	/// an existing open parenthesis.
	/// </summary>
	public class OpenParenthesisException : LanguageException
	{
		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="position">Where the exception occured.</param>
		public OpenParenthesisException(ITextPosition position)
			: base(position, GetMessage(position))
		{
		}

		#region Get Message

		private const string message =
			"Open parenthesis has no matching close parenthesis at line {0}, "
			+ "column {1} (index {2})";

		/// <summary>
		/// Creates an error message using the current state of a reader.
		/// </summary>
		/// <returns>An error message.</returns>
		private static string GetMessage(ITextPosition position)
		{
			return string.Format(message,
				position.Line,
				position.Column,
				position.Index
			);
		}

		#endregion
	}
}
