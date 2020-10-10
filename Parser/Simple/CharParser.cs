namespace Exodrifter.Rumor.Parser
{
	public class CharParser : Parser<char>
	{
		private readonly char ch;

		public CharParser(char ch)
		{
			this.ch = ch;
		}

		public override Result<char> Parse(State state)
		{
			if (state.Source.Length <= state.Index)
			{
				return Result<char>.Error(state.Index, ch.ToString());
			}

			if (state.Source[state.Index] == ch)
			{
				var newState = state.AddIndex(1);
				return Result<char>.Success(newState, ch);
			}

			return Result<char>.Error(state.Index, ch.ToString());
		}
	}
}
