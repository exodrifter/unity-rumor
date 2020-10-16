using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler
{
	public static class Compiler
	{
		#region Block

		public static Parser<Dictionary<string, List<Node>>> Block
		{
			get
			{
				return state =>
				{
					Parse.Whitespaces(state);
					state.IndentIndex = state.Index;

					var blocks = Parse.Many(NodeBlock.Or(LabelBlock))(state);

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

					return result;
				};
			}
		}

		private static Parser<Dictionary<string, List<Node>>> NodeBlock
		{
			get
			{
				return state =>
				{
					var nodes = Parse.Block(Node, Parse.Same)(state);

					var result = new Dictionary<string, List<Node>>();
					result.Add("_main", nodes);
					return result;
				};
			}
		}

		private static Parser<Dictionary<string, List<Node>>> LabelBlock
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

						// Consume at least one newline
						Parse.Spaces.Until(Parse.EOL)(state);
						Parse.NewLine(state);
						Parse.Whitespaces(state);

						Parse.Indented(state);
						var result = NodeBlock(state);

						// Move the node block to the identifier
						result[identifier] = result[null];
						result.Remove(null);

						transaction.Commit();
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

						transaction.Commit();
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

		public static Parser<string> IdentifierLabel =>
			Parse.Surround('[', ']', Identifier);
	}
}
