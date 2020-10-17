namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// A parser is a function that takes an input state and returns a value
	/// which has been determined by reading the contents of the input. It is
	/// required to roll back the state to the original value on failure and
	/// to update the state to a new position on success.
	/// </summary>
	/// <typeparam name="T">The type the parser will return.</typeparam>
	/// <param name="state">The input state.</param>
	public delegate T Parser<T>(ParserState state);
}
