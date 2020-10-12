using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;

namespace Exodrifter.Rumor.Compiler
{
	public static class Compiler
	{
		public static Result<SayNode> SayNode(State state)
		{
			var r1 = new LambdaParser<string>(Identifier).Maybe().Parse(state);
			var r2 = new CharParser(':').Parse(r1.NextState);
			var r3 = new CharParser(ch => ch != '\n', "")
				.Many(0)
				.Fn(chs => new string(chs.ToArray()))
				.Parse(r2.NextState);

			return new Result<SayNode>(
				r3.NextState,
				new SayNode(r1.Value, r3.Value)
			);
		}

		public static Result<string> Identifier(State state)
		{
			var result =
				new CharParser(char.IsLetterOrDigit, "alphanumeric character")
				.Many(1)
				.Fn(chs => new string(chs.ToArray()))
				.Parse(state);
			
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
		}
	}
}
