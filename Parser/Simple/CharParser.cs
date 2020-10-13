using System;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		public static Parser<char> Char(char ch)
		{
			return Char((other) => ch == other, ch.ToString());
		}

		/// <summary>
		/// Creates a new CharParser which succeeds on a predicate check.
		/// </summary>
		/// <param name="predicate">
		/// The predicate to test.
		/// </param>
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
	}
}
