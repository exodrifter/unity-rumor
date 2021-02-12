namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Thrown when the parser needs to throw an error with a general error
	/// message.
	/// </summary>
	public class ReasonException : ParserException
	{
		public ReasonException(int index, string message)
			: base(index, message) { }

		public ReasonException(int index, string message, ParserException inner)
			: base(index, message, inner) { }
	}
}
