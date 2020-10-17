namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		/// <summary>
		/// Returns a parser that returns the current column if the current
		/// parser position is at the same column number as the reference
		/// indentation index.
		/// </summary>
		public static Parser<int> Same
		{
			get
			{
				return state =>
				{
					int current = CalculateIndentFrom(state, state.Index);
					int indent = CalculateIndentFrom(state, state.IndentIndex);

					if (current == indent)
					{
						return current;
					}
					else
					{
						throw new ReasonException(
							state.Index,
							"expected a line indented to column " + indent
						);
					}
				};
			}
		}

		/// <summary>
		/// Returns a parser that returns the current column if the current
		/// parser position is at a same or greater column number than the
		/// column for the reference indentation index.
		/// </summary>
		public static Parser<int> SameOrIndented
		{
			get
			{
				return state =>
				{
					int current = CalculateIndentFrom(state, state.Index);
					int indent = CalculateIndentFrom(state, state.IndentIndex);

					if (current >= indent)
					{
						return current;
					}
					else
					{
						throw new ReasonException(
							state.Index,
							"line indented to column " + indent + " or more"
						);
					}
				};
			}
		}

		/// <summary>
		/// Returns a parser that returns the current column if the current
		/// parser position is at a greater column number than the column for
		/// the reference indentation index.
		/// </summary>
		public static Parser<int> Indented
		{
			get
			{
				return state =>
				{
					int current = CalculateIndentFrom(state, state.Index);
					int indent = CalculateIndentFrom(state, state.IndentIndex);

					if (current > indent)
					{
						return current;
					}
					else
					{
						throw new ReasonException(
							state.Index,
							"line indented to column " + indent + " or more"
						);
					}
				};
			}
		}

		private static int CalculateIndentFrom(ParserState state, int index)
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
			var column = 1;
			for (int i = line.Length - 1; i >= 0; i--)
			{
				var ch = line[i];
				if (ch == '\t')
				{
					column += state.TabSize - (column % state.TabSize);
				}
				else
				{
					column++;
				}
			}

			return column;
		}
	}
}
