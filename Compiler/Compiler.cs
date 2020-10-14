using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler
{
	public static class Compiler
	{
		public static Parser<SayNode> SayNode()
		{
			return (ref State state) =>
			{
				var temp = state.SetIndent();

				var identifier = Identifier().Maybe()(ref temp)
					.GetValueOrDefault(null);

				Parse.Spaces(ref temp);
				Parse.Char(':')(ref temp);
				Parse.Whitespaces(ref temp);

				var lines = Parse.Block1(
					Parse.AnyChar.Until(Parse.NewLine).String(),
					Parse.Indented
				)(ref temp);

				var dialog = new List<string>();
				foreach (var line in lines)
				{
					dialog.Add(line.Trim());
				}

				state = temp;
				return new SayNode(identifier, string.Join(" ", dialog));
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
