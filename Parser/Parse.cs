using System;

namespace Exodrifter.Rumor.Parser
{
	public delegate Result<T> Parser<T>(State state);

	public static partial class Parse
	{
		#region Char

		/// <summary>
		/// Returns a parser which parses the specified character.
		/// </summary>
		/// <param name="ch">The character to parse.</param>
		/// <returns>A parser.</returns>
		public static Parser<char> Char(char ch)
		{
			return Char((other) => ch == other, ch.ToString());
		}

		/// <summary>
		/// Creates a parser which parses any character that satisfies a
		/// predicate.
		/// </summary>
		/// <param name="predicate">
		/// The predicate to satisfy.
		/// </param>
		/// <param name="expected">
		/// The expected value (used in error messages).
		/// </param>
		/// <returns>A parser.</returns>
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

		#region LINQ-like

		/// <summary>
		/// Runs an arbitrary function over the result of the parser.
		/// </summary>
		/// <typeparam name="U">
		/// The new type of the result.
		/// </typeparam>
		/// <param name="fn">
		/// The function to use over the result.
		/// </param>
		/// <returns>
		/// A new parser which represents the result of running an arbitrary
		/// function over the result of this parser.
		/// </returns>
		public static Parser<U> Select<T, U>(this Parser<T> parser, Func<T, U> fn)
		{
			return state =>
			{
				var result = parser(state);
				return new Result<U>(result.NextState, fn(result.Value));
			};
		}

		#endregion

		#region Maybe

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
	}
}
