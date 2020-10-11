using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public class ManyParser<T> : Parser<List<T>>
	{
		private readonly Parser<T> parser;
		private readonly int minimum;

		public ManyParser(Parser<T> parser, int minimum)
		{
			this.parser = parser;
			this.minimum = minimum;
		}

		public override Result<List<T>> Parse(State state)
		{
			var results = new List<T>();

			while (true)
			{
				try
				{
					var result = parser.Parse(state);
					results.Add(result.Value);
					state = result.NextState;
				}
				catch (ParserException exception)
				{
					if (results.Count < minimum)
					{
						var delta = minimum - results.Count;
						throw new ParserException(
							exception.Index,
							"at least " + delta + " more of " +
							string.Join(", ", exception.Expected)
						);
					}
					else
					{
						return Result<List<T>>.Success(state, results);
					}
				}
			}
		}
	}
}
