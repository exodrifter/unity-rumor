namespace Exodrifter.Rumor.Parser
{
	public class StringParser : Parser<string>
	{
		private string str;

		public StringParser(string str)
		{
			this.str = str;
		}

		public override Result<string> Parse(State state)
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
		}
	}
}
