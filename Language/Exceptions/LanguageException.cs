using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes an exception that occurs from any language-related operation.
	/// </summary>
	public abstract class LanguageException : Exception
	{
		#region Properties

		/// <summary>
		/// The line number the parse exception occured.
		/// </summary>
		public int Line
		{
			get { return line; }
		}
		private readonly int line;

		/// <summary>
		/// The column number the parse exception occured.
		/// </summary>
		public int Column
		{
			get { return column; }
		}
		private readonly int column;

		/// <summary>
		/// The position in the string where the parse exception occured.
		/// </summary>
		public int Index
		{
			get { return index; }
		}
		private readonly int index;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new language exception.
		/// </summary>
		/// <param name="reader">The reader to create an exception for.</param>
		/// <param name="message">The error message.</param>
		protected LanguageException(ITextPosition position, string message)
			: base(message)
		{
			this.line = position.Line;
			this.column = position.Column;
			this.index = position.Index;
		}

		#endregion

		#region Util

		/// <summary>
		/// Returns a human-readable string that describes a string.
		/// <summary>
		protected static string DescribeString(string str)
		{
			var builder = new StringBuilder();

			builder.Append("\"");
			foreach (var ch in str)
			{
				builder.Append(EscapeChar(ch));
			}
			builder.Append("\"");

			return builder.ToString();
		}

		/// <summary>
		/// Returns a human-readable string that describes an array of
		/// characters that compose a string.
		/// <summary>
		protected static string DescribeChar(string str)
		{
			return DescribeChar(str.ToCharArray());
		}

		/// <summary>
		/// Returns a human-readable string that describes a list of characters.
		/// <summary>
		/// <returns>A string representing the characters.</returns>
		protected static string DescribeChar(params char[] chs)
		{
			var builder = new StringBuilder();

			if (chs.Length == 0)
			{
				return "a character";
			}

			for (int i = 0; i < chs.Length; ++i)
			{
				if (i != 0)
				{
					builder.Append(", ");
				}
				if (i == chs.Length - 1 && chs.Length > 1)
				{
					builder.Append("or ");
				}

				builder.Append("'");
				builder.Append(EscapeChar(chs[i]));
				builder.Append("'");
			}

			return builder.ToString();
		}

		/// <summary>
		/// Escapes characters as if they were in a C# string.
		/// </summary>
		/// <returns>A string representing the character.</returns>
		private static string EscapeChar(char ch)
		{
			switch(ch)
			{
				default:
					return ch.ToString();

				case '\0':
					return "\\0";
				case '\n':
					return "\\n";
				case '\t':
					return "\\t";
				case '\"':
					return "\\\"";
			}
		}

		#endregion
	}
}
