using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public delegate Result<T> Parser<T>(State state);

	public static partial class Parse
	{
		#region Char

		/// <summary>
		/// Returns a parser that parses the specified character.
		/// </summary>
		/// <param name="ch">The character to parse.</param>
		public static Parser<char> Char(char ch)
		{
			return Char((other) => ch == other, ch.ToString());
		}

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
			return state =>
			{
				if (state.Source.Length <= state.Index)
				{
					throw new ParserException(state.Index, expected);
				}

				var ch = state.Source[state.Index];
				if (predicate(ch))
				{
					var newState = state.AddIndex(1);
					return new Result<char>(newState, ch);
				}

				throw new ParserException(state.Index, expected);
			};
		}

		#endregion

		#region Indented

		/// <summary>
		/// Returns a parser that returns the current column or fails if the
		/// current parser position is not at the same indentation level or
		/// more than the reference indentation level.
		/// </summary>
		public static Parser<int> SameOrIndented()
		{
			return state =>
			{
				var current = CalculateIndentFrom(state, state.Index);
				var indent = CalculateIndentFrom(state, state.IndentIndex);

				if (current >= indent)
				{
					return new Result<int>(state, current);
				}
				else
				{
					throw new ParserException(
						state.Index,
						"indented line"
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
			return state =>
			{
				var result = parser(state);
				return new Result<U>(result.NextState, fn(result.Value));
			};
		}

		#endregion

		#region Many

		/// <summary>
		/// Returns a new parser that repeats a parser for a specified minimum
		/// number of successful times.
		/// </summary>
		/// <typeparam name="T">The return type of the parser.</typeparam>
		/// <param name="parser">The parser to repeat.</param>
		/// <param name="minimum">The minimum number of successes.</param>
		public static Parser<List<T>> Many<T>(this Parser<T> parser, int minimum)
		{
			return state =>
			{
				var results = new List<T>();

				while (true)
				{
					try
					{
						var result = parser(state);
						results.Add(result.Value);
						state = result.NextState;
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
							return new Result<List<T>>(state, results);
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
			return state =>
			{
				try
				{
					return parser(state);
				}
				catch (ParserException)
				{
					return new Result<T>(state, default);
				}
			};
		}

		#endregion

		#region Or

		/// <summary>
		/// Returns a parser that returns the first successful result.
		/// </summary>
		/// <typeparam name="T">The type of the parsers to use.</typeparam>
		/// <param name="first">The first parser to try.</param>
		/// <param name="second">The second parser to try.</param>
		public static Parser<T> Or<T>(this Parser<T> first, Parser<T> second)
		{
			return state =>
			{
				try
				{
					return first(state);
				}
				catch (ParserException exception1)
				{
					try
					{
						return second(state);
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

		#endregion

		#region String

		/// <summary>
		/// Parses the specified string.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		public static Parser<string> String(string str)
		{
			return state =>
			{
				if (string.IsNullOrEmpty(str))
				{
					return new Result<string>(state, str);
				}

				if (state.Source.Length <= state.Index + str.Length - 1)
				{
					throw new ParserException(state.Index, str);
				}

				if (state.Source.Substring(state.Index, str.Length) == str)
				{
					var newState = state.AddIndex(str.Length);
					return new Result<string>(newState, str);
				}

				throw new ParserException(state.Index, str);
			};
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
			return state =>
			{
				var result = first(state);
				return second(result.NextState);
			};
		}

		#endregion
	}
}
