using System;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from an unknown command.
	/// </summary>
	public class UnknownCommandException : LanguageException
	{
		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="position">Where the exception occured.</param>
		/// <param name="command">The unknown command.</param>
		public UnknownCommandException(ITextPosition position, string command)
			: base(position, GetMessage(position, command))
		{
		}

		#region Get Message

		private const string message =
			"Found unknown command \"{0}\" at line {1}, column {2} (index {3})";

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
