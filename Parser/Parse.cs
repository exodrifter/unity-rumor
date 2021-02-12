using System;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		/// <summary>
		/// Returns true if the parser would succeed.
		/// </summary>
		/// <typeparam name="T">The return type of the parser.</typeparam>
		/// <param name="parser">The parser to try.</param>
		public static Parser<bool> FollowedBy<T>(Parser<T> parser)
		{
			return state =>
			{
				try
				{
					parser(new ParserState(state));
					return true;
				}
				catch (ParserException)
				{
					return false;
				}
			};
		}

		/// <summary>
		/// Returns a parser that returns the default value if the parser fails.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="parser">The parser to try.</param>
		public static Parser<Maybe<T>> Maybe<T>(this Parser<T> parser)
		{
			return state =>
			{
				try
				{
					return new Maybe<T>(parser(state));
				}
				catch (ParserException)
				{
					return new Maybe<T>();
				}
			};
		}

		/// <summary>
		/// Returns a parser that returns the first successful result.
		/// </summary>
		/// <typeparam name="T">The type of the parsers to use.</typeparam>
		/// <param name="parser">The first parser to try.</param>
		/// <param name="others">The second parser to try.</param>
		public static Parser<T> Or<T>(this Parser<T> first, Parser<T> second)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					try
					{
						var result = first(state);
						transaction.CommitIndex();
						return result;
					}
					catch (ParserException e1)
					{
						try
						{
							var result = second(state);
							transaction.CommitIndex();
							return result;
						}
						catch (ParserException e2)
						{
							throw new MultipleParserException(e1, e2);
						}
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
					result = result.Or(parser);
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
				result = result.Or(other);
			}
			return result;
		}

		/// <summary>
		/// Wraps a raw value in a parser.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="value">The value to wrap in a parser.</param>
		/// <returns>
		/// A parser that doesn't do anything but return the value.
		/// </returns>
		public static Parser<T> Pure<T>(T value)
		{
			return state =>
			{
				return value;
			};
		}

		/// <summary>
		/// Refer to another parser indirectly, which can be used to allow a set
		/// of circular-dependent parsers to be created.
		/// </summary>
		/// <typeparam name="T">The type of the parser to refer to</typeparam>
		/// <param name="parser">
		/// A function that produces the indirectly-referenced parser.
		/// </param>
		public static Parser<T> Ref<T>(Func<Parser<T>> parser)
		{
			return state =>
			{
				return parser()(state);
			};
		}

		/// <summary>
		/// Returns a new parser that returns the result of running an arbitrary
		/// function over the result of this parser.
		/// </summary>
		/// <typeparam name="U">The new type of the result.</typeparam>
		/// <param name="fn">The function to use over the result.</param>
		public static Parser<U> Select<T, U>
			(this Parser<T> parser, Func<T, U> fn)
		{
			return state =>
			{
				return fn(parser(state));
			};
		}

		/// <summary>
		/// Returns a parser which runs the first parser, discards the result,
		/// then returns a different value.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <typeparam name="U">The type of the value to return.</typeparam>
		/// <param name="first">The parser.</param>
		/// <param name="second">The value to return.</param>
		public static Parser<U> Then<T, U>(this Parser<T> first, U value) =>
			Then(first, Pure(value));

		/// <summary>
		/// Returns a parser which runs the first parser, discards the result,
		/// then runs the second parser.
		/// </summary>
		/// <typeparam name="T">The type to discard.</typeparam>
		/// <typeparam name="U">The type to return.</typeparam>
		/// <param name="first">The parser to discard the result of.</param>
		/// <param name="second">The parser to return the result of.</param>
		public static Parser<U> Then<T, U>
			(this Parser<T> first, Parser<U> second)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					first(state);
					var result = second(state);

					transaction.CommitIndex();
					return result;
				}
			};
		}
	}
}
