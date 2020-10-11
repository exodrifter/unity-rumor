namespace Exodrifter.Rumor.Parser
{
	public sealed class Result<T>
	{
		public T Value { get; }
		public State NextState { get; }

		private Result(State nextState, T value)
		{
			Value = value;
			NextState = nextState;
		}

		private Result(int index, string[] expected)
		{
			Value = default;
			NextState = default;
		}

		public static Result<T> Success(State nextState, T value)
		{
			return new Result<T>(nextState, value);
		}
	}
}
