using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		#region Chain

		/// <summary>
		/// Returns a new parser that repeats a parser one or more times,
		/// separated by <paramref name="op"/>, and returns the left associative
		/// application of the function returned by the <paramref name="op"/>
		/// on those values.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="value">The parser for the value.</param>
		/// <param name="op">The parser for the operator.</param>
		public static Parser<T> ChainL1<T>
			(this Parser<T> value, Parser<Func<T, T, T>> op)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					// Parse the first value
					var x = value(state);

					while (true)
					{
						// Parse an operator
						Func<T, T, T> fn;
						try
						{
							fn = op(state);
						}
						catch (ParserException)
						{
							break;
						}

						// This part of the parser is not in the try block
						// because if the operator exists, the value must also
						// exist
						var y = value(state);
						x = fn(x, y);
					}

					transaction.CommitIndex();
					return x;
				}
			};
		}

		#endregion

		#region Many

		/// <summary>
		/// Returns a new parser that repeats a parser one or more times.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="parser">The parser to repeat.</param>
		public static Parser<List<T>> Many1<T>(this Parser<T> parser) =>
			Many(parser, 1);

		/// <summary>
		/// Returns a new parser that repeats a parser for a specified minimum
		/// number of successful times.
		/// </summary>
		/// <typeparam name="T">The return type of the parser.</typeparam>
		/// <param name="parser">The parser to repeat.</param>
		/// <param name="minimum">The minimum number of successes.</param>
		public static Parser<List<T>> Many<T>
			(this Parser<T> parser, int minimum = 0)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var results = new List<T>();

					while (true)
					{
						try
						{
							results.Add(parser(state));
						}
						catch (ParserException exception)
						{
							if (results.Count < minimum)
							{
								var delta = minimum - results.Count;
								throw new ReasonException(
									state.Index,
									"expected at least " + delta + " more " +
									"instance(s) of the parser to succeed",
									exception
								);
							}
							else
							{
								transaction.CommitIndex();
								return results;
							}
						}
					}
				}
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
		public static Parser<List<T>> Until<T, U>
			(this Parser<T> parser, Parser<U> until)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var results = new List<T>();

					while (true)
					{
						try
						{
							until(new ParserState(state));
							break;
						}
						catch (ParserException untilException)
						{
							try
							{
								results.Add(parser(state));
							}
							catch (ParserException parserException)
							{
								// If we're at the end of the source file, throw
								// the exception for the until parser instead,
								// since that is more likely to be descriptive
								// of why this parser is failing.
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

					transaction.CommitIndex();
					return results;
				}
			};
		}

		#endregion
	}
}
