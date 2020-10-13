using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;

namespace Exodrifter.Rumor.Compiler
{
	public static class Compiler
	{
		public static Result<SayNode> SayNode(State state)
		{
			var r1 = Identifier().Maybe().DoParse(state);
			var r2 = Parse.Char(':').DoParse(r1.NextState);
			var r3 = Parse.Char(ch => ch != '\n', "")
				.Many(0)
				.Select(chs => new string(chs.ToArray()))
				.DoParse(r2.NextState);

			return new Result<SayNode>(
				r3.NextState,
				new SayNode(r1.Value, r3.Value)
			);
		}

		public static Parser<string> Identifier()
		{
			return state =>
			{
				var result =
					Parse.Char(char.IsLetterOrDigit, "alphanumeric character")
					.Many(1)
					.Select(chs => new string(chs.ToArray()))
					.DoParse(state);

				if (result.Value.StartsWith("_"))
				{
					throw new ParserException(
						state.Index,
						"any alphanumeric character except for '_'"
					);
				}
				else
				{
					return result;
				}
			};
		}
	}
}
