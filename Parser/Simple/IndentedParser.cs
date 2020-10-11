namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Succeeds when the current indentation level is either equal to or
	/// greater than the indexed indentation level in the state.
	/// </summary>
	public class IndentedParser : Parser<int>
	{
		public IndentedParser() { }

		public override Result<int> Parse(State state)
		{
			var current = CalculateIndentFrom(state, state.Index);
			var indent = CalculateIndentFrom(state, state.IndentIndex);

			if (current >= indent)
			{
				return new Result<int>(state, current);
			}
			else
			{
				throw new ParserException(
					state.Index,
					"indented line"
				);
			}
		}

		private static int CalculateIndentFrom(State state, int index)
		{
			var line = "";
			for (int i = index - 1; i >= 0; i--)
			{
				var ch = state.Source[i];
				if (ch == '\n' || ch == '\r')
				{
					break;
				}
				else
				{
					line += ch;
				}
			}

			// The line is backwards, so to read the line in order we need to
			// read it starting at the end.
			var result = 0;
			for (int i = line.Length - 1; i >= 0; i--)
			{
				var ch = line[i];
				if (ch == ' ')
				{
					result++;
				}
				else if (ch == '\t')
				{
					result += state.TabSize - (result % state.TabSize);
				}
				else
				{
					break;
				}
			}

			return result;
		}
	}
}
