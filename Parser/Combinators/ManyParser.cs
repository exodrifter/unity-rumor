using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		public static Parser<List<T>> Many<T>(this Parser<T> parser, int minimum)
		{
			return state =>
			{
				var results = new List<T>();

				while (true)
				{
					try
					{
						var result = parser(state);
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
							return new Result<List<T>>(state, results);
						}
					}
				}
			};
		}
	}
}
