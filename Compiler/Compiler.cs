using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Exodrifter.Rumor.Compiler
{
	using Rumor = Engine.Rumor;

	public static partial class Compiler
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
							AddChoice
							.Or(Label)
							.Or(If)
							.Or(
								Node.Select(x =>
									new Dictionary<string, List<Node>>()
										{ { Rumor.MainIdentifier, new List<Node>() { x } } }
								)
							),
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

		#region Choice

		public static Parser<Dictionary<string, List<Node>>> AddChoice
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("choice")(state);
						var maybeIdentifier = Parse
							.Spaces1
							.Then(IdentifierLabel)
							.Maybe()(state);

						// Consume the rest of the whitespace on this line
						Parse.Spaces.Until(Parse.EOL)(state);
						Parse.EOL(state);

						// Parse the choice text
						Parse.Whitespaces(state);
						Parse.Indented(state);

						state.IndentIndex = state.Index;
						Parse.Char('>')(state);
						Parse.Whitespaces(state);
						var text = Text(state);

						// Consume the rest of the whitespace on this line
						Parse.Spaces.Until(Parse.EOL)(state);

						// Parse an optional indented block on the next line
						var result = Parse.EOL
							.Then(Parse.Whitespaces)
							.Then(Parse.Same)
							.Then(Script)
							.Maybe()(state)
							.GetValueOrDefault(
								new Dictionary<string, List<Node>>()
								{
									{ Rumor.MainIdentifier, new List<Node>() }
								}
							);

						// Move the main block to the identifier for this label
						var textToHash = text.Simplify().GetHashCode().ToString();
						var identifier = maybeIdentifier
							.GetValueOrDefault("_" + Sha1Hash(textToHash));
						result[identifier] = result[Rumor.MainIdentifier];

						// Add the choice as the only node in the main block
						result[Rumor.MainIdentifier] = new List<Node>() {
							new AddChoiceNode(identifier, text)
						};

						transaction.CommitIndex();
						return result;
					}
				};
			}
		}

		#endregion

		#region Control Flow

		public static Parser<Dictionary<string, List<Node>>> If
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("if")(state);
						Parse.Spaces1(state);
						var comparison = ComparisonBlock(state).Simplify();

						// Consume the rest of the whitespace on this line
						Parse.Spaces.Until(Parse.EOL)(state);
						Parse.EOL(state);

						// Parse an indented block
						Parse.Whitespaces(state);
						Parse.Indented(state);
						var result = Script(state);
						if (result[Rumor.MainIdentifier].Count == 0)
						{
							throw new ReasonException(
								state.Index,
								"if statements must be followed by a non-empty block"
							);
						}
						transaction.CommitIndex();

						// Try to parse the next control flow statement
						ControlNode next;
						try
						{
							Parse.Whitespaces(state);
							var nextResult = Elif.Or(Else)(state);

							next = nextResult.Item1;

							// Combine the scripts
							foreach (var item in nextResult.Item2)
							{
								result[item.Key] = item.Value;
							}
						}
						catch (ParserException)
						{
							transaction.Rollback();
							next = null;
						}

						// Move the main block to the control node
						var block = result[Rumor.MainIdentifier];
						var node = new ControlNode(comparison, block, next);
						result.Remove(Rumor.MainIdentifier);

						transaction.CommitIndex();
						result[Rumor.MainIdentifier] = new List<Node>() { node };
						return result;
					}
				};
			}
		}

		public static Parser<Tuple<ControlNode, Dictionary<string, List<Node>>>> Elif
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("elif")(state);
						Parse.Spaces1(state);
						var comparison = ComparisonBlock(state).Simplify();

						// Consume the rest of the whitespace on this line
						Parse.Spaces.Until(Parse.EOL)(state);
						Parse.EOL(state);

						// Parse an indented block
						Parse.Whitespaces(state);
						Parse.Indented(state);
						var result = Script(state);
						if (result[Rumor.MainIdentifier].Count == 0)
						{
							throw new ReasonException(
								state.Index,
								"elif statements must be followed by a non-empty block"
							);
						}
						transaction.CommitIndex();

						// Try to parse the next control flow statement
						ControlNode next;
						try
						{
							Parse.Whitespaces(state);
							var nextResult = Elif.Or(Else)(state);

							next = nextResult.Item1;

							// Combine the scripts
							foreach (var item in nextResult.Item2)
							{
								result[item.Key] = item.Value;
							}
						}
						catch (ParserException)
						{
							transaction.Rollback();
							next = null;
						}

						// Move the main block to the control node
						var block = result[Rumor.MainIdentifier];
						var node = new ControlNode(comparison, block, next);
						result.Remove(Rumor.MainIdentifier);

						transaction.CommitIndex();
						return new Tuple<ControlNode, Dictionary<string, List<Node>>>(
							node,
							result
						);
					}
				};
			}
		}

		public static Parser<Tuple<ControlNode, Dictionary<string, List<Node>>>> Else
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("else")(state);

						// Consume the rest of the whitespace on this line
						Parse.Spaces.Until(Parse.EOL)(state);
						Parse.EOL(state);

						// Parse an indented block
						Parse.Whitespaces(state);
						Parse.Indented(state);
						var result = Script(state);
						if (result[Rumor.MainIdentifier].Count == 0)
						{
							throw new ReasonException(
								state.Index,
								"else statements must be followed by a non-empty block"
							);
						}

						// Move the main block to the control node
						var block = result[Rumor.MainIdentifier];
						var node = new ControlNode(null, block, null);
						result.Remove(Rumor.MainIdentifier);

						transaction.CommitIndex();
						return new Tuple<ControlNode, Dictionary<string, List<Node>>>(
							node,
							result
						);
					}
				};
			}
		}

		#endregion

		#region Nodes

		public static Parser<Node> Node =>
			SetDialog.Select(x => (Node)x)
			.Or(AppendDialog.Select(x => (Node)x))
			.Or(SetVariableLogic.Select(x => (Node)x))
			.Or(SetVariableMath.Select(x => (Node)x))
			.Or(SetVariableText.Select(x => (Node)x))
			.Or(BindingAction().Select(x => (Node)x))
			.Or(Clear.Select(x => (Node)x))
			.Or(Choose.Select(x => (Node)x))
			.Or(Jump.Select(x => (Node)x))
			.Or(Call.Select(x => (Node)x))
			.Or(Wait.Select(x => (Node)x))
			.Or(Pause.Select(x => (Node)x))
			.Or(Return.Select(x => (Node)x));

		public static Parser<ChooseNode> Choose
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("choose")(state);

						var spaces = Parse.Spaces(state);
						if (Parse.FollowedBy(Parse.EOL)(state))
						{
							transaction.CommitIndex();
							return new ChooseNode();
						}

						if (spaces.Length == 0)
						{
							throw new ReasonException(
								state.Index,
								"Expected space after 'choose'"
							);
						}

						Parse.String("in")(state);
						Parse.Spaces1(state);
						var time = Timespan(state);

						Parse.Spaces1(state);
						Parse.String("or jump")(state);
						Parse.Spaces1(state);
						var jump = Identifier(state);

						Parse.Spaces(state);
						transaction.CommitIndex();
						return new ChooseNode(time, jump);
					}
				};
			}
		}

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

						// Check if we might have defined an optional clear type
						try
						{
							Parse.Spaces1(state);
						}
						catch (ParserException)
						{
							return new ClearNode(ClearType.All);
						}

						// Parse the optional clear type
						if (!Parse.FollowedBy(Parse.EOL)(state))
						{
							var type =
								Parse.String("all").Then(ClearType.All)
								.Or(Parse.String("choices").Then(ClearType.Choices))
								.Or(Parse.String("dialog").Then(ClearType.Dialog))
								(state);

							transaction.CommitIndex();
							return new ClearNode(type);
						}
						else
						{
							return new ClearNode(ClearType.All);
						}
					}
				};
			}
		}

		public static Parser<AppendDialogNode> AppendDialog =>
			Dialog('+', (i, d) => new AppendDialogNode(i, d));

		public static Parser<SetDialogNode> SetDialog =>
			Dialog(':', (i, d) => new SetDialogNode(i, d));

		private static Parser<T> Dialog<T>
			(char ch, Func<string, Expression, T> constructor)
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

					var dialog = Text(state);

					transaction.CommitIndex();
					return constructor(identifier, dialog);
				}
			};
		}

		public static Parser<CallNode> Call
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.String("call")(state);
						Parse.Spaces1(state);
						var identifier = Identifier(state);

						transaction.CommitIndex();
						return new CallNode(identifier);
					}
				};
			}
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

						var time = Timespan(state);

						transaction.CommitIndex();
						return new PauseNode(time);
					}
				};
			}
		}

		public static Parser<ReturnNode> Return =>
			Parse.String("return").Then(new ReturnNode());

		public static Parser<SetVariableNode> SetVariableLogic
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var errorIndex = state.Index;
						state.IndentIndex = state.Index;

						var id = Variable(Engine.ValueType.Boolean)(state);
						Parse.Spaces(state);

						Parse.String("=")(state);
						Parse.Spaces(state);

						var expression = Compiler.LogicBlock(state).Simplify();

						transaction.CommitIndex();
						return new SetVariableNode(id, expression);
					}
				};
			}
		}

		public static Parser<SetVariableNode> SetVariableMath
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var errorIndex = state.Index;
						state.IndentIndex = state.Index;

						var id = Variable(Engine.ValueType.Number)(state);
						Parse.Spaces(state);

						Parse.String("=")(state);
						Parse.Spaces(state);

						var expression = Compiler.MathBlock(state).Simplify();

						transaction.CommitIndex();
						return new SetVariableNode(id, expression);
					}
				};
			}
		}

		public static Parser<SetVariableNode> SetVariableText
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						state.IndentIndex = state.Index;

						var id = Variable(Engine.ValueType.String)(state);
						Parse.Spaces(state);

						Parse.String("=")(state);
						Parse.Spaces(state);

						var expression = Compiler.QuoteBlock(state).Simplify();

						transaction.CommitIndex();
						return new SetVariableNode(id, expression);
					}
				};
			}
		}

		public static Parser<string> Variable(Engine.ValueType type)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var errorIndex = state.Index;
					var id = Identifier(state);

					var userState = (RumorParserState)state.UserState;
					userState.LinkVariable(errorIndex, id, type);

					transaction.CommitIndex();
					return id;
				}
			};
		}

		public static Parser<WaitNode> Wait =>
			Parse.String("wait").Then(new WaitNode());

		#endregion

		#region Identifier

		public static Parser<string> Identifier
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var c = Parse.Letter(state);
						var cs = Parse.Alphanumeric.Or(Parse.Char('_'))
							.Many().String()(state);

						transaction.CommitIndex();
						return c + cs;
					}
				};
			}
		}

		public static Parser<string> IdentifierLabel
		{
			get
			{
				return state =>
				{
					var errorIndex = state.Index;

					var id = Parse.Surround('[', ']', Identifier)(state);

					var userState = (RumorParserState)state.UserState;
					if (userState.UsedIdentifiers.Contains(id))
					{
						throw new ReasonException(errorIndex,
							"the identifier \"" + id + "\" has already been " +
							"used!"
						);
					}

					userState.UsedIdentifiers.Add(id);
					return id;
				};
			}
		}

		private static string Sha1Hash(string rawData)
		{
			using (var sha1Hasher = SHA1.Create())
			{
				var bytes = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(rawData));
				return Convert.ToBase64String(bytes);
			}
		}

		#endregion

		#region Binding

		public static Parser<BindingActionNode> BindingAction() =>
			BindingAction0()
				.Or(BindingAction1())
				.Or(BindingAction2())
				.Or(BindingAction3())
				.Or(BindingAction4());

		private static Parser<BindingActionNode> BindingAction0()
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var errorIndex = state.Index;
					var id = Identifier(state);

					var userState = (RumorParserState)state.UserState;
					if (!userState.ContainsBindingHint(BindingType.Action, id, 0))
					{
						throw new ReasonException(errorIndex,
							"Tried to reference unlinked action \"" + id + "\" " +
							"with zero parameters!"
						);
					}

					Parse.Spaces(state);
					Parse.String("(")(state);

					Parse.Spaces(state);
					Parse.String(")")(state);

					transaction.CommitIndex();
					return new BindingActionNode(id);
				}
			};
		}

		private static Parser<BindingActionNode> BindingAction1()
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var errorIndex = state.Index;
					var id = Identifier(state);

					var userState = (RumorParserState)state.UserState;
					if (!userState.ContainsBindingHint(BindingType.Action, id, 1))
					{
						throw new ReasonException(errorIndex,
							"Tried to reference unlinked action \"" + id + "\" " +
							"with one parameter!"
						);
					}
					var hint = (BindingActionHint1)
						userState.GetBindingHint(BindingType.Action, id, 1);

					Parse.Spaces(state);
					Parse.String("(")(state);

					Parse.Spaces(state);
					var p1 = Param(hint.t1)(state);

					Parse.Spaces(state);
					Parse.String(")")(state);

					transaction.CommitIndex();
					return new BindingActionNode(id, p1);
				}
			};
		}

		private static Parser<BindingActionNode> BindingAction2()
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var errorIndex = state.Index;
					var id = Identifier(state);

					var userState = (RumorParserState)state.UserState;
					if (!userState.ContainsBindingHint(BindingType.Action, id, 2))
					{
						throw new ReasonException(errorIndex,
							"Tried to reference unlinked action \"" + id + "\" " +
							"with two parameters!"
						);
					}
					var hint = (BindingActionHint2)
						userState.GetBindingHint(BindingType.Action, id, 2);

					Parse.Spaces(state);
					Parse.String("(")(state);

					Parse.Spaces(state);
					var p1 = Param(hint.t1)(state);

					Parse.Spaces(state);
					Parse.String(",")(state);

					Parse.Spaces(state);
					var p2 = Param(hint.t2)(state);

					Parse.Spaces(state);
					Parse.String(")")(state);

					transaction.CommitIndex();
					return new BindingActionNode(id, p1, p2);
				}
			};
		}

		private static Parser<BindingActionNode> BindingAction3()
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var errorIndex = state.Index;
					var id = Identifier(state);

					var userState = (RumorParserState)state.UserState;
					if (!userState.ContainsBindingHint(BindingType.Action, id, 3))
					{
						throw new ReasonException(errorIndex,
							"Tried to reference unlinked action \"" + id + "\" " +
							"with three parameters!"
						);
					}
					var hint = (BindingActionHint3)
						userState.GetBindingHint(BindingType.Action, id, 3);

					Parse.Spaces(state);
					Parse.String("(")(state);

					Parse.Spaces(state);
					var p1 = Param(hint.t1)(state);

					Parse.Spaces(state);
					Parse.String(",")(state);

					Parse.Spaces(state);
					var p2 = Param(hint.t2)(state);

					Parse.Spaces(state);
					Parse.String(",")(state);

					Parse.Spaces(state);
					var p3 = Param(hint.t3)(state);

					Parse.Spaces(state);
					Parse.String(")")(state);

					transaction.CommitIndex();
					return new BindingActionNode(id, p1, p2, p3);
				}
			};
		}

		private static Parser<BindingActionNode> BindingAction4()
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					var errorIndex = state.Index;
					var id = Identifier(state);

					var userState = (RumorParserState)state.UserState;
					if (!userState.ContainsBindingHint(BindingType.Action, id, 4))
					{
						throw new ReasonException(errorIndex,
							"Tried to reference unlinked action \"" + id + "\" " +
							"with four parameters!"
						);
					}
					var hint = (BindingActionHint4)
						userState.GetBindingHint(BindingType.Action, id, 4);

					Parse.Spaces(state);
					Parse.String("(")(state);

					Parse.Spaces(state);
					var p1 = Param(hint.t1)(state);

					Parse.Spaces(state);
					Parse.String(",")(state);

					Parse.Spaces(state);
					var p2 = Param(hint.t2)(state);

					Parse.Spaces(state);
					Parse.String(",")(state);

					Parse.Spaces(state);
					var p3 = Param(hint.t3)(state);

					Parse.Spaces(state);
					Parse.String(",")(state);

					Parse.Spaces(state);
					var p4 = Param(hint.t4)(state);

					Parse.Spaces(state);
					Parse.String(")")(state);

					transaction.CommitIndex();
					return new BindingActionNode(id, p1, p2, p3, p4);
				}
			};
		}

		private static Parser<Expression> Param(Engine.ValueType t)
		{
			return state =>
			{
				switch (t)
				{
					case Engine.ValueType.Boolean:
						return Logic(state);

					case Engine.ValueType.Number:
						return Math(state);

					case Engine.ValueType.String:
						return Quote(state);

					default:
						throw new InvalidOperationException(
							"Unknown value type!"
						);
				}
			};
		}

		#endregion

		private static Parser<Expression> Timespan
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var number = Compiler.MathBlock(state);
						Parse.Spaces(state);

						var scale = Parse
							.String("milliseconds", "millisecond", "ms").Then(0.001d)
							.Or(Parse.String("seconds", "second", "s").Then(1d))
							.Or(Parse.String("minutes", "minute", "m").Then(60d))
							(state);

						transaction.CommitIndex();

						return new MultiplyExpression(
							number,
							new NumberLiteral(scale)
						).Simplify();
					}
				};
			}
		}
	}
}
