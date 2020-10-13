using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		public static Parser<T> Or<T>(this Parser<T> first, Parser<T> second)
		{
			return state =>
			{
				try
				{
					return first.DoParse(state);
				}
				catch (ParserException exception1)
				{
					try
					{
						return second.DoParse(state);
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
			};
		}
	}
}
