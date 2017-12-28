using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from a read operation.
	/// </summary>
	public class ReadException : LanguageException
	{
		#region Constructor

		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="reader">The reader to create an exception for.</param>
		/// <param name="expected">The expected characters.</param>
		public ReadException(Reader reader, params char[] expected)
			: base(reader, GetMessage(reader, expected))
		{
		}

		/// <summary>
		/// Creates a new exception.
		/// </summary>
		/// <param name="reader">
		/// The reader to create an exception for.
		/// </param>
		/// <param name="expected">
		/// A string describing what was expected.
		/// </param>
		public ReadException(Reader reader, string expected)
			: base(reader, GetMessage(reader, expected))
		{
		}

		#endregion

		#region Get Message

		private const string message =
			"Expected {0}; got {1} instead at line {2}, column {3} (pos {4})";

		/// <summary>
		/// Creates an error message using the current state of a reader.
		/// </summary>
		/// <returns>An error message.</returns>
		private static string GetMessage(Reader reader, params char[] expected)
		{
			return string.Format(message,
				DescribeChar(expected),
				GetCurrentChar(reader),
				reader.Line,
				reader.Column,
				reader.Index
			);
		}

		/// <summary>
		/// Creates an error message using the current state of a reader.
		/// </summary>
		/// <returns>An error message.</returns>
		private static string GetMessage(Reader reader, string expected)
		{
			return string.Format(message,
				expected,
				GetCurrentChar(reader),
				reader.Line,
				reader.Column,
				reader.Index
			);
		}

		#endregion

		#region Util

		/// <summary>
		/// Returns the current character in the reader or the string "EOF" if
		/// the reader is at the end of the file.
		/// </summary>
		/// <returns>A string representing the current character.</returns>
		private static string GetCurrentChar(Reader reader)
		{
			if (reader.EOF)
			{
				return "EOF";
			}

			return DescribeChar(reader.Script[reader.Index]);
		}

		#endregion
	}
}
