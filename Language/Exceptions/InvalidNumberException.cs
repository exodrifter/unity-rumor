using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from an invalid number string.
	/// </summary>
	public class InvalidNumberException : LanguageException
	{
		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="position">Where the exception occured.</param>
		/// <param name="builder">The invalid number string.</param>
		public InvalidNumberException(ITextPosition position, StringBuilder builder)
			: base(position, GetMessage(position, builder.ToString()))
		{
		}

		#region Get Message

		private const string message =
			"Found invalid number \"{0}\" at line {1}, column {2} (index {3})";

		/// <summary>
		/// Creates an error message using the current state of a reader.
		/// </summary>
		/// <returns>An error message.</returns>
		private static string GetMessage(ITextPosition position, string str)
		{
			return string.Format(message,
				str,
				position.Line,
				position.Column,
				position.Index
			);
		}

		#endregion
	}
}
