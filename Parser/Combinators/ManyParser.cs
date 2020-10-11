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
				var result = parser.Parse(state);
				if (result.IsSuccess)
				{
					results.Add(result.Value);
					state = result.NextState;
					continue;
				}
				else if (results.Count < minimum)
				{
					var delta = minimum - results.Count;
					return Result<List<T>>.Error(
						result.ErrorIndex,
						"at least " + delta + " more of " +
						string.Join(", ", result.Expected)
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
