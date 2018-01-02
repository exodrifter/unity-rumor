using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from an invalid number string.
	/// </summary>
	public class UnknownOpException : LanguageException
	{
		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="position">Where the exception occured.</param>
		/// <param name="op">The invalid operator.</param>
		public UnknownOpException(ITextPosition position, string op)
			: base(position, GetMessage(position, op))
		{
		}

		#region Get Message

		private const string message =
			"Found invalid operator \"{0}\" at line {1}, column {2}"
			+ " (index {3})";

		/// <summary>
		/// Creates an error message using the current state of a reader.
		/// </summary>
		/// <returns>An error message.</returns>
		private static string GetMessage(ITextPosition position, string op)
		{
			return string.Format(message,
				op,
				position.Line,
				position.Column,
				position.Index
			);
		}

		#endregion
	}
}
