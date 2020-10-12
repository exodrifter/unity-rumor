using System;

namespace Exodrifter.Rumor.Parser
{
	public class LambdaParser<T> : Parser<T>
	{
		private readonly Func<State, Result<T>> fn;

		public LambdaParser(Func<State, Result<T>> fn)
		{
			this.fn = fn;
		}

		public override Result<T> Parse(State state)
		{
			return fn(state);
		}
	}
}
