using System;

namespace Exodrifter.Rumor.Parser
{
	public delegate Result<T> Parser<T>(State state);

	public static partial class Parse
	{
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
	}
}
