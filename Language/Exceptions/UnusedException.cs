using System;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from unused characters.
	/// </summary>
	public class UnusedException : LanguageException
	{
		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="position">Where the exception occured.</param>
		/// <param name="unused">The unused characters.</param>
		public UnusedException(ITextPosition position, string unused)
			: base(position, GetMessage(position, unused))
		{
		}

		#region Get Message

		private const string message =
			"Found unused characters {0} at line {1}, column {2} (index {3})";

		/// <summary>
		/// Creates an error message using the current state of a reader.
		/// </summary>
		/// <returns>An error message.</returns>
		private static string GetMessage(ITextPosition position, string unused)
		{
			return string.Format(message,
				DescribeString(unused),
				position.Line,
				position.Column,
				position.Index
			);
		}

		#endregion
	}
}
