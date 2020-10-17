using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		/// <summary>
		/// Converts a parser that returns a character into a parser that
		/// returns a string.
		/// </summary>
		/// <param name="parser">The parser to convert.</param>
		public static Parser<string> String(this Parser<char> parser) =>
			parser.Select(x => x.ToString());

		/// <summary>
		/// Converts a parser that returns a list of characters into a parser
		/// that returns a string.
		/// </summary>
		/// <param name="parser">The parser to convert.</param>
		public static Parser<string> String(this Parser<List<char>> parser)
		{
			return parser.Select(chs => new string(chs.ToArray()));
		}

		/// <summary>
		/// Parses the specified string.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		public static Parser<string> String(string str)
		{
			return state =>
			{
				if (string.IsNullOrEmpty(str))
				{
					return str;
				}

				if (state.Source.Length <= state.Index + str.Length - 1)
				{
					throw new ExpectedException(state.Index, "\"" + str + "\"");
				}

				if (state.Source.Substring(state.Index, str.Length) == str)
				{
					state.Index += str.Length;
					return str;
				}

				throw new ExpectedException(state.Index, "\"" + str + "\"");
			};
		}

		/// <summary>
		/// Parses any one of the specified strings.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		public static Parser<string> String(params string[] strs)
		{
			var parsers = new List<Parser<string>>(strs.Length);
			foreach (var str in strs)
			{
				parsers.Add(String(str));
			}
			return Or(parsers.ToArray());
		}
	}
}
