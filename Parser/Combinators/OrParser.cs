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
			var result1 = first.Parse(state);
			if (result1.IsSuccess)
			{
				return result1;
			}
			else
			{
				var result2 = second.Parse(state);
				if (result2.IsSuccess)
				{
					return result2;
				}
				else
				{
					var expected = new List<string>(
						result1.Expected.Length + result2.Expected.Length
					);
					expected.AddRange(result1.Expected);
					expected.AddRange(result2.Expected);
					return Result<T>.Error(state.Index, expected.ToArray());
				}
			}
		}
	}
}
