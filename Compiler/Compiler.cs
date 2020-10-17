using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler
{
	using Rumor = Engine.Rumor;

	public static class Compiler
	{
		public static Parser<Dictionary<string, List<Node>>> Script
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						// Find the indentation level
						Parse.Whitespaces(state);
						state.IndentIndex = state.Index;

						var blocks = Parse.Block(
							Node.Select(x =>
								new Dictionary<string, List<Node>>()
									{ { Rumor.MainIdentifier, new List<Node>() { x } } }
							)
							.Or(Label),
							Parse.Same
						)(state);

						var result = new Dictionary<string, List<Node>>();
						foreach (var block in blocks)
						{
							foreach (var key in block.Keys)
							{
								if (result.ContainsKey(key))
								{
									result[key].AddRange(block[key]);
								}
								else
								{
									result[key] = block[key];
								}
							}
						}

						transaction.CommitIndex();
						return result;
					}
				};
			}
		}

		#region Label

		public static Parser<Dictionary<string, List<Node>>> Label
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("label")(state);
						Parse.Spaces1(state);
						var identifier = IdentifierLabel(state);

						// Consume the rest of the whitespace on this line
						Parse.Spaces.Until(Parse.EOL)(state);
						Parse.EOL(state);

						// Parse an indented block
						Parse.Whitespaces(state);
						Parse.Indented(state);
						var result = Script(state);

						// Move the main block to the identifier for this label
						result[identifier] = result[Rumor.MainIdentifier];
						result.Remove(Rumor.MainIdentifier);

						transaction.CommitIndex();
						return result;
					}
				};
			}
		}

		#endregion

		#region Nodes

		public static Parser<Node> Node =>
			Choose.Select(x => (Node)x)
			.Or(Clear.Select(x => (Node)x))
			.Or(Add.Select(x => (Node)x))
			.Or(Say.Select(x => (Node)x))
			.Or(Jump.Select(x => (Node)x))
			.Or(Pause.Select(x => (Node)x))
			.Or(Return.Select(x => (Node)x))
			.Or(Wait.Select(x => (Node)x));

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
						Parse.String("clear")(state);
						transaction.CommitIndex();

						// Parse the optional clear type
						try
						{
							Parse.Spaces1(state);

							var type =
								Parse.String("all").Then(ClearType.All)
								.Or(Parse.String("choices").Then(ClearType.Choices))
								.Or(Parse.String("dialog").Then(ClearType.Dialog))
								(state);

							transaction.CommitIndex();
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

					transaction.CommitIndex();
					return constructor(identifier, dialog);
				}
			};
		}

		public static Parser<JumpNode> Jump
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("jump")(state);
						Parse.Spaces1(state);
						var identifier = Identifier(state);

						transaction.CommitIndex();
						return new JumpNode(identifier);
					}
				};
			}
		}

		public static Parser<PauseNode> Pause
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("pause")(state);
						Parse.Spaces1(state);

						var number = ExpressionCompiler.Math(state);
						Parse.Spaces(state);

						var scale = Parse
							.String("ms", "milliseconds").Then(0.001d)
							.Or(Parse.String("s", "seconds").Then(1d))
							.Or(Parse.String("m", "minutes").Then(60d))
							(state);

						var time = new MultiplyExpression(
							number,
							new NumberLiteral(scale)
						).Simplify();

						return new PauseNode(time);
					}
				};
			}
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

		public static Parser<string> IdentifierLabel
		{
			get
			{
				return state =>
				{
					var errorIndex = state.Index;

					var id = Parse.Surround('[', ']', Identifier)(state);

					if (state.UsedIdentifiers.Contains(id))
					{
						throw new ParserException(errorIndex, "identifier");
					}
					else
					{
						state.UsedIdentifiers.Add(id);
						return id;
					}
				};
			}
		}
	}
}
