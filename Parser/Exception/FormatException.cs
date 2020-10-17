
namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Thrown when the parser fails to convert a type to the correct format.
	/// </summary>
	public class FormatException : ParserException
	{
		public FormatException(int index, string type, string suggestion)
			: base(index, MakeMessage(type, suggestion)) { }

		private static string MakeMessage(string type, string suggestion)
		{
			return "failed to parse the format for a " + type + "; "
				+ suggestion;
		}
	}
}
