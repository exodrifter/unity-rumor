using System;

namespace Exodrifter.Rumor.Parser
{
	public class CharParser : Parser<char>
	{
		private readonly Func<char, bool> predicate;
		private readonly string expected;

		public CharParser(char ch)
			: this((other) => ch == other, ch.ToString()) { }

		/// <summary>
		/// Creates a new CharParser which succeeds on a predicate check.
		/// </summary>
		/// <param name="predicate">
		/// The predicate to test.
		/// </param>
		/// <param name="expected">
		/// The expected value (used in error messages).
		/// </param>
		public CharParser(Func<char, bool> predicate, string expected)
		{
			this.predicate = predicate;
			this.expected = expected;
		}

		public override Result<char> Parse(State state)
		{
			if (state.Source.Length <= state.Index)
			{
				throw new ParserException(state.Index, expected);
			}

			var ch = state.Source[state.Index];
			if (predicate(ch))
			{
				var newState = state.AddIndex(1);
				return Result<char>.Success(newState, ch);
			}

			throw new ParserException(state.Index, expected);
		}
	}
}
