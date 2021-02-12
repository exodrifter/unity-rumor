using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		/// <summary>
		/// Returns a parser that parses the specified character.
		/// </summary>
		/// <param name="ch">The character to parse.</param>
		public static Parser<char> Char(char ch) =>
			Char(ch.Equals, "'" + ch.ToString() + "'");

		/// <summary>
		/// Returns a parser that parses a letter.
		/// </summary>
		public static Parser<char> Letter =>
			Char(char.IsLetter, "letter");

		/// <summary>
		/// Returns a parser that parses a digit.
		/// </summary>
		public static Parser<char> Digit =>
			Char(char.IsDigit, "digit");

		/// <summary>
		/// Returns a parser that parses a letter or number.
		/// </summary>
		public static Parser<char> Alphanumeric =>
			Char(char.IsLetterOrDigit, "alphanumeric character");

		/// <summary>
		/// Returns a parser that parses any character.
		/// </summary>
		public static Parser<char> AnyChar =>
			Char(_ => true, "any character");

		/// <summary>
		/// Returns a parser that parses a space or tab.
		/// </summary>
		public static Parser<char> Space =>
			Char(ch => ch == ' ' || ch == '\t', "space");

		/// <summary>
		/// Returns a parser that parses zero or more spaces and tabs.
		/// </summary>
		public static Parser<string> Spaces =>
			Space.Many().String();

		/// <summary>
		/// Returns a parser that parses one or more spaces and tabs.
		/// </summary>
		public static Parser<string> Spaces1 =>
			Space.Many1().String();

		/// <summary>
		/// Returns a parser that parses a carriage return or newline.
		/// </summary>
		public static Parser<char> NewLine =>
			String("\r\n", "\n").Then('\n');

		/// <summary>
		/// Returns a parser that succeeds if the parser is at the end of the
		/// line or at the end of a file.
		/// </summary>
		public static Parser<Unit> EOL =>
			NewLine.Then(new Unit()).Or(EOF);

		/// <summary>
		/// Returns a parser that succeeds if the parser is at the end of the
		/// file.
		/// </summary>
		public static Parser<Unit> EOF
		{
			get
			{
				return state =>
				{
					if (state.EOF)
					{
						return new Unit();
					}
					else
					{
						throw new ExpectedException(state.Index, "end of file");
					}
				};
			}
		}

		/// <summary>
		/// Returns a parser that parses a whitespace character. This includes
		/// space, tab, carriage return, and newline.
		/// </summary>
		public static Parser<char> Whitespace =>
			Char(char.IsWhiteSpace, "whitespace");

		/// <summary>
		/// Returns a parser that parses zero or more whitespace characters.
		/// </summary>
		public static Parser<string> Whitespaces =>
			Whitespace.Many().String();

		/// <summary>
		/// Returns a parser that parses one or more whitespace characters.
		/// </summary>
		public static Parser<string> Whitespaces1 =>
			Whitespace.Many1().String();

		/// <summary>
		/// Returns a parser that parses any character that exists in the
		/// string.
		/// </summary>
		/// <param name="str">The characters to parse.</param>
		public static Parser<char> Char(string str) =>
			Char(str.ToCharArray());

		/// <summary>
		/// Returns a parser that parses any character that exists in the
		/// array.
		/// </summary>
		/// <param name="str">The characters to parse.</param>
		public static Parser<char> Char(params char[] chs)
		{
			var parsers = new List<Parser<char>>(chs.Length);
			foreach (var ch in chs)
			{
				parsers.Add(Char(ch));
			}
			return Or(parsers.ToArray());
		}

		/// <summary>
		/// Returns a parser that parses any character which satisfies a
		/// predicate.
		/// </summary>
		/// <param name="predicate">The predicate to satisfy.</param>
		/// <param name="expected">
		/// The expected value (used in error messages).
		/// </param>
		public static Parser<char> Char
			(Func<char, bool> predicate, string expected)
		{
			return state =>
			{
				if (state.Source.Length <= state.Index)
				{
					throw new ExpectedException(state.Index, expected);
				}

				var ch = state.Source[state.Index];
				if (predicate(ch))
				{
					state.Index += 1;
					return ch;
				}

				throw new ExpectedException(state.Index, expected);
			};
		}
	}
}
