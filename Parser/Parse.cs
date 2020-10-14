using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public delegate T Parser<T>(ref State state);

	public static partial class Parse
	{
		#region Char

		/// <summary>
		/// Returns a parser that parses the specified character.
		/// </summary>
		/// <param name="ch">The character to parse.</param>
		public static Parser<char> Char(char ch) =>
			Char(ch.Equals, ch.ToString());

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
		public static Parser<char> Spaces =>
			Char(ch => ch == ' ' || ch == '\t', "space");

		/// <summary>
		/// Returns a parser that parses a carriage return or newline.
		/// </summary>
		public static Parser<char> NewLine =>
			Char(ch => ch == '\r' || ch == '\n', "newline");

		/// <summary>
		/// Returns a parser that parses a whitespace character. This includes
		/// space, tab, carriage return, and newline.
		/// </summary>
		public static Parser<char> Whitespace =>
			Char(char.IsWhiteSpace, "whitespace");

		/// <summary>
		/// Returns a parser that parses any character which satisfies a
		/// predicate.
		/// </summary>
		/// <param name="predicate">The predicate to satisfy.</param>
		/// <param name="expected">
		/// The expected value (used in error messages).
		/// </param>
		public static Parser<char> Char(Func<char, bool> predicate, string expected)
		{
			return (ref State state) =>
			{
				if (state.Source.Length <= state.Index)
				{
					throw new ParserException(state.Index, expected);
				}

				var ch = state.Source[state.Index];
				if (predicate(ch))
				{
					state = state.AddIndex(1);
					return ch;
				}

				throw new ParserException(state.Index, expected);
			};
		}

		#endregion

		#region Indented

		/// <summary>
		/// Returns a parser that returns the current column if the current
		/// parser position is less than the reference indentation level.
		/// </summary>
		public static Parser<int> Unindented()
		{
			return (ref State state) =>
			{
				int current = CalculateIndentFrom(state, state.Index);
				int reference = CalculateIndentFrom(state, state.IndentIndex);

				if (current == reference)
				{
					return current;
				}
				else
				{
					throw new ParserException(
						state.Index,
						"line indented less than column " + reference
					);
				}
			};
		}

		/// <summary>
		/// Returns a parser that returns the current column if the current
		/// parser position is the same as the reference indentation level.
		/// </summary>
		public static Parser<int> Same()
		{
			return (ref State state) =>
			{
				int current = CalculateIndentFrom(state, state.Index);
				int reference = CalculateIndentFrom(state, state.IndentIndex);

				if (current == reference)
				{
					return current;
				}
				else
				{
					throw new ParserException(
						state.Index,
						"line indented to column " + reference
					);
				}
			};
		}

		/// <summary>
		/// Returns a parser that returns the current column if the current
		/// parser position is at the same indentation level or more than the
		/// reference indentation level.
		/// </summary>
		public static Parser<int> SameOrIndented()
		{
			return (ref State state) =>
			{
				int current = CalculateIndentFrom(state, state.Index);
				int reference = CalculateIndentFrom(state, state.IndentIndex);

				if (current >= reference)
				{
					return current;
				}
				else
				{
					throw new ParserException(
						state.Index,
						"line indented to column " + reference + " or more"
					);
				}
			};
		}

		private static int CalculateIndentFrom(State state, int index)
		{
			var line = "";
			for (int i = index - 1; i >= 0; i--)
			{
				var ch = state.Source[i];
				if (ch == '\n' || ch == '\r')
				{
					break;
				}
				else
				{
					line += ch;
				}
			}

			// The line is backwards, so to read the line in order we need to
			// read it starting at the end.
			var column = 1;
			for (int i = line.Length - 1; i >= 0; i--)
			{
				var ch = line[i];
				if (ch == ' ')
				{
					column++;
				}
				else if (ch == '\t')
				{
					column += state.TabSize - (column % state.TabSize);
				}
				else
				{
					break;
				}
			}

			return column;
		}

		#endregion

		#region LINQ-like

		/// <summary>
		/// Returns a new parser that returns the result of running an arbitrary
		/// function over the result of this parser.
		/// </summary>
		/// <typeparam name="U">The new type of the result.</typeparam>
		/// <param name="fn">The function to use over the result.</param>
		public static Parser<U> Select<T, U>(this Parser<T> parser, Func<T, U> fn)
		{
			return (ref State state) =>
			{
				return fn(parser(ref state));
			};
		}

		/// <summary>
		/// Returns a new parser that returns the result of another parser only
		/// if its result satisfies some predicate.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="parser">The parser to check the result of.</param>
		/// <param name="predicate">The predicate to satisfy.</param>
		/// <param name="expected">
		/// The expected value (used in error messages).
		/// </param>
		public static Parser<T> Where<T>(this Parser<T> parser, Func<T, bool> predicate, string expected)
		{
			return (ref State state) =>
			{
				var temp = state;
				var result = parser(ref temp);
				if (predicate(result))
				{
					state = temp;
					return result;
				}
				else
				{
					throw new ParserException(state.Index, expected);
				}
			};
		}

		#endregion

		#region Many

		public static Parser<List<T>> Many<T>(this Parser<T> parser) =>
			Many(parser, 0);

		public static Parser<List<T>> Many1<T>(this Parser<T> parser) =>
			Many(parser, 1);

		/// <summary>
		/// Returns a new parser that repeats a parser for a specified minimum
		/// number of successful times.
		/// </summary>
		/// <typeparam name="T">The return type of the parser.</typeparam>
		/// <param name="parser">The parser to repeat.</param>
		/// <param name="minimum">The minimum number of successes.</param>
		public static Parser<List<T>> Many<T>(this Parser<T> parser, int minimum)
		{
			return (ref State state) =>
			{
				var results = new List<T>();

				while (true)
				{
					try
					{
						var temp = state;
						var result = parser(ref temp);
						state = temp;
						results.Add(result);
					}
					catch (ParserException exception)
					{
						if (results.Count < minimum)
						{
							var delta = minimum - results.Count;
							throw new ParserException(
								exception.Index,
								"at least " + delta + " more of " +
								string.Join(", ", exception.Expected)
							);
						}
						else
						{
							return results;
						}
					}
				}
			};
		}

		#endregion

		#region Maybe

		/// <summary>
		/// Returns a parser that returns the default value if the parser fails.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="parser">The parser to try.</param>
		public static Parser<T> Maybe<T>(this Parser<T> parser)
		{
			return (ref State state) =>
			{
				try
				{
					return parser(ref state);
				}
				catch (ParserException)
				{
					return default;
				}
			};
		}

		#endregion

		#region Or

		/// <summary>
		/// Returns a parser that returns the first successful result.
		/// </summary>
		/// <typeparam name="T">The type of the parsers to use.</typeparam>
		/// <param name="parser">The first parser to try.</param>
		/// <param name="others">The second parser to try.</param>
		public static Parser<T> Or<T>(this Parser<T> first, Parser<T> second)
		{
			return (ref State state) =>
			{
				try
				{
					var temp1 = state;
					var result = first(ref temp1);
					state = temp1;
					return result;
				}
				catch (ParserException exception1)
				{
					try
					{
						var temp2 = state;
						var result = second(ref temp2);
						state = temp2;
						return result;
					}
					catch (ParserException exception2)
					{
						var expected = new List<string>(
							exception1.Expected.Length + exception2.Expected.Length
						);
						expected.AddRange(exception1.Expected);
						expected.AddRange(exception2.Expected);
						throw new ParserException(state.Index, expected.ToArray());
					}
				}
			};
		}

		/// <summary>
		/// Returns the first parser that returns a successful result.
		/// </summary>
		/// <typeparam name="T">The type of the parsers to use.</typeparam>
		/// <param name="parsers">The parsers to try.</param>
		public static Parser<T> Or<T>(params Parser<T>[] parsers)
		{
			Parser<T> result = null;
			foreach (var parser in parsers)
			{
				if (result != null)
				{
					result.Or(parser);
				}
				else
				{
					result = parser;
				}
			}
			return result;
		}

		/// <summary>
		/// Returns a parser that returns the first successful result.
		/// </summary>
		/// <typeparam name="T">The type of the parsers to use.</typeparam>
		/// <param name="parser">The first parser to try.</param>
		/// <param name="others">The other parsers to try.</param>
		public static Parser<T> Or<T>(this Parser<T> parser, params Parser<T>[] others)
		{
			Parser<T> result = parser;
			foreach (var other in others)
			{
				result.Or(other);
			}
			return result;
		}

		#endregion

		#region String

		/// <summary>
		/// Converts a parser that returns a list of characters into a parser
		/// that returns a string.
		/// </summary>
		/// <param name="parser">The parser to convert.</param>
		public static Parser<string> String(this Parser<List<char>> parser)
		{
			return parser.Select(chs => new string(chs.ToArray()));
		}

		/// <summary>
		/// Parses the specified string.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		public static Parser<string> String(string str)
		{
			return (ref State state) =>
			{
				if (string.IsNullOrEmpty(str))
				{
					return str;
				}

				if (state.Source.Length <= state.Index + str.Length - 1)
				{
					throw new ParserException(state.Index, str);
				}

				if (state.Source.Substring(state.Index, str.Length) == str)
				{
					state = state.AddIndex(str.Length);
					return str;
				}

				throw new ParserException(state.Index, str);
			};
		}

		/// <summary>
		/// Parses any one of the specified strings.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		public static Parser<string> String(params string[] strs)
		{
			Parser<string> result = null;
			foreach (var str in strs)
			{
				if (result != null)
				{
					result.Or(String(str));
				}
				else
				{
					result = String(str);
				}
			}
			return result;
		}

		#endregion

		#region Then

		/// <summary>
		/// Returns a parser which runs the first parser, discards the result,
		/// then runs the second parser.
		/// </summary>
		/// <typeparam name="T">The type to discard.</typeparam>
		/// <typeparam name="U">The type to return.</typeparam>
		/// <param name="first">The parser to discard the result of.</param>
		/// <param name="second">The parser to return the result of.</param>
		public static Parser<U> Then<T, U>(this Parser<T> first, Parser<U> second)
		{
			return (ref State state) =>
			{
				var result = first(ref state);
				return second(ref state);
			};
		}

		#endregion

		#region Until

		/// <summary>
		/// Runs a parser until the result of another parser is successful.
		/// </summary>
		/// <typeparam name="T">The type of the parser to run.</typeparam>
		/// <typeparam name="U">The type of the parser to check.</typeparam>
		/// <param name="parser">The parser to run.</param>
		/// <param name="until">If this parser succeeds, stop.</param>
		public static Parser<List<T>> Until<T, U>(this Parser<T> parser, Parser<U> until)
		{
			return (ref State state) =>
			{
				var results = new List<T>();
				while (true)
				{
					try
					{
						var temp = state;
						until(ref temp);
						break;
					}
					catch (ParserException untilException)
					{
						try
						{
							results.Add(parser(ref state));
						}
						catch (ParserException parserException)
						{
							// If we're at the end of the source file, throw
							// the parsing exception for the until parser
							// instead, since that is more likely descriptive
							// of the content that is missing.
							if (state.EOF)
							{
								throw untilException;
							}
							else
							{
								throw parserException;
							}
						}
					}
				}
				return results;
			};
		}

		#endregion
	}
}
