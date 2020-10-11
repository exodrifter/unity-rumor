using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public class OrParser<T> : Parser<T>
	{
		private readonly Parser<T> first;
		private readonly Parser<T> second;

		public OrParser(Parser<T> first, Parser<T> second)
		{
			this.first = first;
			this.second = second;
		}

		public override Result<T> Parse(State state)
		{
			try
			{
				return first.Parse(state);
			}
			catch (ParserException exception1)
			{
				try
				{
					return second.Parse(state);
				}
				catch (ParserException exception2)
				{
					var expected = new List<string>(
						exception1.Expected.Length + exception2.Expected.Length
					);
					expected.AddRange(exception1.Expected);
					expected.AddRange(exception2.Expected);
					throw new ParserException(state.Index, expected.ToArray());
				}
			}
		}
	}
}
