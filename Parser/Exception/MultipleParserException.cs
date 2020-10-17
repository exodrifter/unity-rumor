using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Used to accumulate errors from multiple parsers.
	/// </summary>
	public class MultipleParserException : ParserException
	{
		public readonly ParserException[] exceptions;

		public MultipleParserException(ParserException e1, ParserException e2)
			: base(FirstIndex(e1, e2), MakeMessage(e1, e2))
		{
			exceptions = AllExceptions(e1, e2).ToArray();
		}

		private static int FirstIndex(params ParserException[] exs)
		{
			var min = int.MaxValue;

			var exceptions = AllExceptions(exs);
			foreach (var exception in exceptions)
			{
				if (exception.Index < min)
				{
					min = exception.Index;
				}
			}

			return min;
		}

		private static string MakeMessage(params ParserException[] exs)
		{
			var messages = new List<string>();

			var exceptions = AllExceptions(exs);
			foreach (var exception in exceptions)
			{
				messages.Add(exception.Message);
			}

			return "expected one of the following exceptions to not happen: " +
				string.Join("; ", messages);
		}

		private static List<ParserException> AllExceptions
			(params ParserException[] exceptions)
		{
			var result = new List<ParserException>();

			foreach (var e in exceptions)
			{
				if (e is MultipleParserException)
				{
					var multipleExceptions = e as MultipleParserException;
					result.AddRange(
						AllExceptions(multipleExceptions.exceptions)
					);
				}
				else
				{
					result.Add(e);
				}
			}

			return result;
		}
	}
}
