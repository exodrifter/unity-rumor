using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		/// <summary>
		/// Parses an indented block of one or more occurrences of
		/// <paramref name="line"/>.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="line">The parser to use.</param>
		/// <param name="indentType">The indentation parser.</param>
		/// <param name="minimum">
		/// The minimum number of times the parser must be successful.
		/// </param>
		public static Parser<List<T>> Block1<T>
			(Parser<T> line, Parser<int> indentType) =>
			PrefixBlock<Unit, T>(null, line, indentType, 1);

		/// <summary>
		/// Parses an indented block of zero or more occurrences of
		/// <paramref name="line"/>.
		///
		/// This parser assumes a few things:
		/// * The caller sets the indentation reference index.
		/// * The line parser does not necessarily consume leading whitespace.
		/// * The line parser may NOT consume the newline character at the end
		///   of a line.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="line">The parser to use for each line.</param>
		/// <param name="indentType">The indentation parser.</param>
		/// <param name="minimum">
		/// The minimum number of times the parser must be successful.
		/// </param>
		public static Parser<List<T>> Block<T>
			(Parser<T> line, Parser<int> indentType, int minimum = 0) =>
			PrefixBlock<Unit, T>(null, line, indentType, minimum);

		/// <summary>
		/// Parses an indented block of one or more occurrences of
		/// <paramref name="line"/> that are prefixed with
		/// <paramref name="prefix"/>.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="prefix">
		/// The prefix parser to use at the beginning of each line, or null for
		/// no prefix parsing.
		/// </param>
		/// <param name="line">The parser to use.</param>
		/// <param name="indentType">The indentation parser.</param>
		/// <param name="minimum">
		/// The minimum number of times the parser must be successful.
		/// </param>
		public static Parser<List<U>> PrefixBlock1<T, U>
			(Parser<T> prefix, Parser<U> line, Parser<int> indentType) =>
			PrefixBlock(prefix, line, indentType, 1);

		/// <summary>
		/// Parses an indented block of zero or more occurrences of
		/// <paramref name="line"/> where each line is prefixed with
		/// <paramref name="prefix"/>.
		///
		/// This parser assumes a few things:
		/// * The caller sets the indentation reference index.
		/// * The line parser does not necessarily consume leading whitespace.
		/// * The line parser may NOT consume the newline character at the end
		///   of a line.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="prefix">
		/// The prefix parser to use at the beginning of each line, or null for
		/// no prefix parsing.
		/// </param>
		/// <param name="line">The parser to use for each line.</param>
		/// <param name="indentType">The indentation parser.</param>
		/// <param name="minimum">
		/// The minimum number of times the parser must be successful.
		/// </param>
		public static Parser<List<U>> PrefixBlock<T, U>
			(Parser<T> prefix, Parser<U> line, Parser<int> indentType,
			int minimum = 0)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var results = new List<U>();

					Action checkMinimum = () =>
					{
						if (results.Count < minimum)
						{
							var delta = minimum - results.Count;
							throw new ReasonException(
								state.Index,
								"expected at least " + delta + " more " +
								"line(s) for this block"
							);
						}
					};

					// Consume leading whitespace for this block
					Whitespaces(state);
					indentType(state);

					while (true)
					{
						// Parse the prefix
						try
						{
							prefix?.Invoke(state);
						}
						catch (ParserException)
						{
							checkMinimum();
							return results;
						}

						// Parse a line
						results.Add(line(state));

						// Consume the rest of the spaces on this line
						if (!FollowedBy(EOL)(state))
						{
							Space.Until(EOL)(state);
						}
						transaction.CommitIndex();

						// If this is the end of the file, stop
						if (FollowedBy(EOF)(state))
						{
							checkMinimum();
							return results;
						}

						// Check if the block continues
						try
						{
							NewLine
								.Then(Whitespaces)
								.Then(indentType)(state);

							// If this is the end of the file, stop
							if (FollowedBy(EOF)(state))
							{
								checkMinimum();
								return results;
							}
						}
						catch (ParserException)
						{
							checkMinimum();
							return results;
						}
					}
				}
			};
		}
	}
}
