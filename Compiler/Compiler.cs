﻿using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;

namespace Exodrifter.Rumor.Compiler
{
	public static class Compiler
	{
		#region Nodes

		public static Parser<ChooseNode> Choose =>
			Parse.String("choose").Then(new ChooseNode());

		public static Parser<ClearNode> Clear
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						state.IndentIndex = state.Index;

						Parse.String("clear")(state);
						transaction.Commit();

						// Parse the optional clear type
						try
						{
							Parse.Spaces1(state);

							var type =
								Parse.String("all").Then(ClearType.All)
								.Or(Parse.String("choices").Then(ClearType.Choices))
								.Or(Parse.String("dialog").Then(ClearType.Dialog))
								(state);

							transaction.Commit();
							return new ClearNode(type);
						}
						catch (ParserException)
						{
							return new ClearNode(ClearType.All);
						}
					}
				};
			}
		}

		public static Parser<AddNode> Add =>
			Dialog('+', (i, d) => new AddNode(i, d));

		public static Parser<SayNode> Say =>
			Dialog(':', (i, d) => new SayNode(i, d));

		private static Parser<T> Dialog<T>
			(char ch, Func<string, Expression<StringValue>, T> constructor)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					state.IndentIndex = state.Index;

					var identifier = Identifier.Maybe()(state)
						.GetValueOrDefault(null);

					Parse.Spaces(state);
					Parse.Char(ch)(state);
					Parse.Whitespaces(state);

					var dialog = ExpressionCompiler.Text(state);

					transaction.Commit();
					return constructor(identifier, dialog);
				}
			};
		}

		public static Parser<ReturnNode> Return =>
			Parse.String("return").Then(new ReturnNode());

		public static Parser<WaitNode> Wait =>
			Parse.String("wait").Then(new WaitNode());

		#endregion

		public static Parser<string> Identifier =>
			Parse.Alphanumeric
				.Many(1)
				.String()
				.Where(x => !x.StartsWith("_"), "identifier");
	}
}
