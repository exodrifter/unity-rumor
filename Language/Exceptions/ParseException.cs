using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes miscellaneous exceptions that occur from parser logic.
	/// </summary>
	public class ParseException : LanguageException
	{
		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="position">Where the exception occured.</param>
		/// <param name="str">The message.</param>
		public ParseException(ITextPosition position, string str)
			: base(position, GetMessage(position, str))
		{
		}

		#region Get Message

		private const string message =
			"{0} at line {1}, column {2} (index {3})";

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
