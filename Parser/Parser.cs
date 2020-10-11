using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public abstract class Parser<T>
	{
		public abstract Result<T> Parse(State state);

		public Parser<List<T>> Many(int minimum)
		{
			return new ManyParser<T>(this, minimum);
		}

		public Parser<T> Or(Parser<T> other)
		{
			return new OrParser<T>(this, other);
		}

		public Parser<U> Then<U>(Parser<U> other)
		{
			return new ThenParser<T, U>(this, other);
		}
	}
}