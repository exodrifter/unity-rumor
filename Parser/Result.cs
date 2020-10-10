namespace Exodrifter.Rumor.Parser
{
	public sealed class Result<T>
	{
		public T Value { get; }
		public State NextState { get; }

		public string[] Expected { get; }
		public int ErrorIndex { get; }

		public bool IsSuccess { get; }

		private Result(State nextState, T value)
		{
			IsSuccess = true;

			Value = value;
			NextState = nextState;

			Expected = default;
			ErrorIndex = default;
		}

		private Result(int index, string[] expected)
		{
			IsSuccess = false;

			Value = default;
			NextState = default;

			Expected = expected;
			ErrorIndex = index;
		}

		public static Result<T> Success(State nextState, T value)
		{
			return new Result<T>(nextState, value);
		}

		public static Result<T> Error(int index, params string[] expected)
		{
			
			return new Result<T>(index, expected);
		}
	}
}
