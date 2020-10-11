namespace Exodrifter.Rumor.Parser
{
	public sealed class Result<T>
	{
		public State NextState { get; }
		public T Value { get; }

		public Result(State nextState, T value)
		{
			NextState = nextState;
			Value = value;
		}
	}
}
