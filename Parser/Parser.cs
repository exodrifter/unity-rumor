using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public abstract class Parser<T>
	{
		public abstract Result<T> Parse(State state);

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
		public Parser<U> Fn<U>(Func<T, U> fn)
		{
			return new LambdaParser<U>(state => {
				var result = Parse(state);
				return new Result<U>(result.NextState, fn(result.Value));
			});
		}

		public Parser<List<T>> Many(int minimum)
		{
			return new ManyParser<T>(this, minimum);
		}

		public Parser<T> Maybe()
		{
			return new LambdaParser<T>(state => {
				try
				{
					return Parse(state);
				}
				catch (ParserException)
				{
					return new Result<T>(state, default);
				}
			});
		}

		public Parser<T> Or(Parser<T> other)
		{
			return new OrParser<T>(this, other);
		}

		public Parser<U> Then<U>(Parser<U> other)
		{
			return new ThenParser<T, U>(this, other);
		}
	}
}
