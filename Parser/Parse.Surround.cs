namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		#region Surround

		/// <summary>
		/// Returns a parser that parses delimiters with padded whitespace
		/// around another parser.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="before">The beginning delimiter.</param>
		/// <param name="after">The ending delimiter.</param>
		/// <param name="parser">The parser between the delimiters.</param>
		public static Parser<T> Surround<T>
			(char before, char after, Parser<T> parser) =>
			Surround(Char(before), Char(after), parser);

		/// <summary>
		/// Returns a parser that parses delimiters with padded whitespace
		/// around another parser.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="before">The beginning delimiter.</param>
		/// <param name="after">The ending delimiter.</param>
		/// <param name="parser">The parser between the delimiters.</param>
		public static Parser<T> Surround<T>
			(string before, string after, Parser<T> parser) =>
			Surround(String(before), String(after), parser);

		/// <summary>
		/// Returns a parser that parses delimiters with padded whitespace
		/// around another parser.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <typeparam name="U">The type of the beginning delimiter.</typeparam>
		/// <typeparam name="V">The type of the ending delimiter.</typeparam>
		/// <param name="before">The parser for the beginning delimiter.</param>
		/// <param name="after">The parser for the ending delimiter.</param>
		/// <param name="parser">The parser between the delimiters.</param>
		public static Parser<T> Surround<T, U, V>
			(Parser<U> before, Parser<V> after, Parser<T> parser)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					before(state);
					Whitespaces(state);

					var result = parser(state);

					Whitespaces(state);
					after(state);

					transaction.CommitIndex();
					return result;
				}
			};
		}

		/// <summary>
		/// Returns a parser that parses delimiters around another parser,
		/// allowing for padded whitespace that keeps the content in the same
		/// block.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="before">The beginning delimiter.</param>
		/// <param name="after">The ending delimiter.</param>
		/// <param name="parser">The parser between the delimiters.</param>
		/// <param name="indentType">The indentation parser.</param>
		public static Parser<T> SurroundBlock<T>
			(char before, char after, Parser<T> parser,
			Parser<int> indentType) =>
			SurroundBlock(Char(before), Char(after), parser, indentType);

		/// <summary>
		/// Returns a parser that parses delimiters around another parser,
		/// allowing for padded whitespace that keeps the content in the same
		/// block.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <param name="before">The beginning delimiter.</param>
		/// <param name="after">The ending delimiter.</param>
		/// <param name="parser">The parser between the delimiters.</param>
		/// <param name="indentType">The indentation parser.</param>
		public static Parser<T> SurroundBlock<T>
			(string before, string after, Parser<T> parser,
			Parser<int> indentType) =>
			SurroundBlock(String(before), String(after), parser, indentType);

		/// <summary>
		/// Returns a parser that parses delimiters around another parser,
		/// allowing for whitespace that keeps the content in the same block.
		/// </summary>
		/// <typeparam name="T">The type of the parser.</typeparam>
		/// <typeparam name="U">The type of the beginning delimiter.</typeparam>
		/// <typeparam name="V">The type of the ending delimiter.</typeparam>
		/// <param name="before">The parser for the beginning delimiter.</param>
		/// <param name="after">The parser for the ending delimiter.</param>
		/// <param name="parser">The parser between the delimiters.</param>
		/// <param name="indentType">The indentation parser.</param>
		public static Parser<T> SurroundBlock<T, U, V>
			(Parser<U> before, Parser<V> after, Parser<T> parser,
			Parser<int> indentType)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					before(state);
					Whitespaces(state);
					indentType(state);

					var result = parser(state);

					Whitespaces(state);
					indentType(state);
					after(state);

					transaction.CommitIndex();
					return result;
				}
			};
		}

		#endregion
	}
}
