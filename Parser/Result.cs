namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Represents a successful result from a parser.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the value which was parsed.
	/// </typeparam>
	public sealed class Result<T>
	{
		/// <summary>
		/// The state to use for the next parser after this one.
		/// </summary>
		public State NextState { get; }

		/// <summary>
		/// The value that was successfully parsed.
		/// </summary>
		public T Value { get; }

		/// <summary>
		/// Creates a new successful parse result.
		/// </summary>
		/// <param name="nextState">
		/// The state to use for the next parser after this one.
		/// </param>
		/// <param name="value">
		/// The value that was successfully parsed.
		/// </param>
		public Result(State nextState, T value)
		{
			NextState = nextState;
			Value = value;
		}
	}
}
