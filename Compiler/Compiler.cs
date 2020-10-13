using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;

namespace Exodrifter.Rumor.Compiler
{
	public static class Compiler
	{
		public static Parser<SayNode> SayNode()
		{
			return (ref State state) =>
			{
				var identifier = Identifier().Maybe()(ref state);
				Parse.Char(':')(ref state);
				var dialog = Parse.AnyChar.Many(0).String()(ref state);

				return new SayNode(identifier, dialog);
			};
		}

		public static Parser<string> Identifier()
		{
			return (ref State state) =>
			{
				return
					Parse.Char(char.IsLetterOrDigit, "alphanumeric character")
					.Many(1).String()
					.Where(x => x.StartsWith("_"), "identifier")
					(ref state);
			};
		}
	}
}
