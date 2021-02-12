using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;

namespace Exodrifter.Rumor.Compiler
{
	// Define type aliases for convenience
	using Op = Func<Expression, Expression, Expression>;

	public static partial class Compiler
	{
		/// <summary>
		/// Helper for constructing operator parsers that can have leading
		/// whitespace.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="value">The value to return.</param>
		/// <param name="ops">The strings which represent the operator.</param>
		private static Parser<T> Op<T>(T value, params string[] ops)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					Parse.Whitespaces(state);
					Parse.SameOrIndented(state);
					Parse.String(ops)(state);

					transaction.CommitIndex();
					return value;
				}
			};
		}

		#region Comparison

		/// <summary>
		/// Parses a comparsion block.
		/// </summary>
		private static Parser<Expression> ComparisonBlock =>
			Parse.SurroundBlock('{', '}', Comparison, Parse.SameOrIndented);

		public static Parser<Expression> Comparison =>
			BooleanComparison
			.Or(VariableComparison)
			.Or(NumberComparison)
			.Or(StringComparison)
			.Or(BooleanVariable);

		private static Parser<Expression> VariableComparison
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var errorIndex = state.Index;

						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var l = Identifier(state);

						var op = ComparisonOps()(state);

						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var r = Identifier(state);

						var userState = (RumorParserState)state.UserState;
						userState.LinkVariables(errorIndex, l, r);

						transaction.CommitIndex();
						return op(
							new VariableExpression(l),
							new VariableExpression(r)
						);
					}
				};
			}
		}

		private static Parser<Expression> BooleanComparison
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var l = Logic.Or(ComparisonParenthesis)(state);
						var op = ComparisonOps()(state);
						var r = Logic.Or(ComparisonParenthesis)(state);

						transaction.CommitIndex();
						return op(l, r);
					}
				};
			}
		}

		private static Parser<Expression> NumberComparison
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var l = Math(state);
						var op = ComparisonOps()
							.Or(Ops())
							(state);
						var r = Math(state);

						transaction.CommitIndex();
						return op(l, r);
					}
				};
			}
		}

		private static Parser<Expression> StringComparison
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var l = QuoteLiteral(state);
						var op = ComparisonOps()(state);
						var r = QuoteLiteral(state);

						transaction.CommitIndex();
						return op(l, r);
					}
				};
			}
		}

		/// <summary>
		/// Parses a comparison operator, which will return a
		/// <see cref="BooleanValue"/> when evaluated.
		/// </summary>
		private static Parser<Op> ComparisonOps() =>
			IsNot().Or(Is());

		/// <summary>
		/// Parses a logic is comparison operator.
		/// </summary>
		private static Parser<Op> Is() =>
			Op<Op>((l, r) => new IsExpression(l, r), "is", "==");

		/// <summary>
		/// Parses a not equal comparison operator.
		/// </summary>
		private static Parser<Op> IsNot() =>
			Op<Op>((l, r) => new IsNotExpression(l, r), "is not", "!=");

		/// <summary>
		/// Parses a comparison operator that can only be used with numbers,
		/// which will return a <see cref="BooleanValue"/> when evaluated.
		/// </summary>
		private static Parser<Op> Ops() =>
			GreaterThanOrEqual
				.Or(GreaterThan)
				.Or(LessThanOrEqual)
				.Or(LessThan);

		/// <summary>
		/// Parses a greater than comparison operator.
		/// </summary>
		private static Parser<Op> GreaterThan =>
			Op<Op>((l, r) => new GreaterThanExpression(l, r), ">");

		/// <summary>
		/// Parses a greater than comparison operator.
		/// </summary>
		private static Parser<Op> LessThan =>
			Op<Op>((l, r) => new LessThanExpression(l, r), "<");

		/// <summary>
		/// Parses a greater than comparison operator.
		/// </summary>
		private static Parser<Op> GreaterThanOrEqual =>
			Op<Op>((l, r) => new GreaterThanOrEqualExpression(l, r), ">=");

		/// <summary>
		/// Parses a greater than comparison operator.
		/// </summary>
		private static Parser<Op> LessThanOrEqual =>
			Op<Op>((l, r) => new LessThanOrEqualExpression(l, r), "<=");

		/// <summary>
		/// Parses a comparison expression wrapped in parenthesis.
		/// </summary>
		private static Parser<Expression> ComparisonParenthesis
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var logic = Parse.SurroundBlock('(', ')',
							Comparison, Parse.SameOrIndented
						)(state);

						transaction.CommitIndex();
						return logic;
					}
				};
			}
		}

		#endregion

		#region Logic

		/// <summary>
		/// Parses a logic block.
		/// </summary>
		private static Parser<Expression> LogicBlock =>
			Parse.SurroundBlock('{', '}', Logic, Parse.SameOrIndented);

		/// <summary>
		/// Parses a logic expression, which will return a
		/// <see cref="BooleanValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression> Logic =>
			Parse.ChainL1(AndPieces, Or);

		// Groups the "and" operators
		private static Parser<Expression> AndPieces =>
			Parse.ChainL1(XorPieces, And);

		// Groups the "xor" operators
		private static Parser<Expression> XorPieces =>
			Parse.ChainL1(LogicPiece, Xor);

		private static Parser<Expression> LogicPiece =>
			LogicParenthesis
				.Or(NotExpression)
				.Or(BooleanLiteral)
				.Or(BooleanVariable);

		/// <summary>
		/// Parses a logic or operator.
		/// </summary>
		private static Parser<Op> Or =>
			Op<Op>((l, r) => new OrExpression(l, r), "or", "||");

		/// <summary>
		/// Parses a logic and operator.
		/// </summary>
		private static Parser<Op> And =>
			Op<Op>((l, r) => new AndExpression(l, r), "and", "&&");

		/// <summary>
		/// Parses a logic xor operator.
		/// </summary>
		private static Parser<Op> Xor =>
			Op<Op>((l, r) => new XorExpression(l, r), "xor", "^");

		/// <summary>
		/// Parses a boolean literal.
		/// </summary>
		private static Parser<Expression> BooleanLiteral
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var b = Parse.String("true").Then(true)
								.Or(Parse.String("false").Then(false))
								(state);

						transaction.CommitIndex();
						return new BooleanLiteral(b);
					}
				};
			}
		}

		/// <summary>
		/// Parses a boolean variable.
		/// </summary>
		private static Parser<Expression> BooleanVariable
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var name = Variable(Engine.ValueType.Boolean)(state);

						transaction.CommitIndex();
						return new BooleanVariable(name);
					}
				};
			}
		}

		/// <summary>
		/// Parses a logic not operator and the logic expression associated
		/// with it.
		/// </summary>
		private static Parser<Expression> NotExpression
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.String("not", "!")(state);
						var logic = Logic.Or(Comparison)(state);

						transaction.CommitIndex();
						return new NotExpression(logic);
					}
				};
			}
		}

		/// <summary>
		/// Parses a logic expression wrapped in parenthesis.
		/// </summary>
		private static Parser<Expression> LogicParenthesis
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var logic = Parse.SurroundBlock('(', ')',
							Logic, Parse.SameOrIndented
						)(state);

						transaction.CommitIndex();
						return logic;
					}
				};
			}
		}

		#endregion

		#region Math

		/// <summary>
		/// Parses a math block.
		/// </summary>
		private static Parser<Expression> MathBlock =>
			Parse.SurroundBlock('{', '}', Math, Parse.SameOrIndented);

		/// <summary>
		/// Parses an arithmetic expression, which will return a
		/// <see cref="NumberValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression> Math =>
			Parse.ChainL1(MultipliedPieces, AddOrSubtract);

		// Groups the multiplication and division operators
		private static Parser<Expression> MultipliedPieces =>
			Parse.ChainL1(MathPiece, MultiplyOrDivide);

		private static Parser<Expression> MathPiece =>
			MathParenthesis.Or(NumberLiteral).Or(NumberVariable);

		/// <summary>
		/// Parses an addition or subtraction operator.
		/// </summary>
		private static Parser<Op> AddOrSubtract =>
			Add.Or(Subtract);

		/// <summary>
		/// Parses an addition operator.
		/// </summary>
		private static Parser<Op> Add =>
			Op<Op>((l, r) => new AddExpression(l, r), "+");

		/// <summary>
		/// Parses a subtraction operator.
		/// </summary>
		private static Parser<Op> Subtract =>
			Op<Op>((l, r) => new SubtractExpression(l, r), "-");

		/// <summary>
		/// Parses a multiplication or division operator.
		/// </summary>
		private static Parser<Op> MultiplyOrDivide =>
			Multiply.Or(Divide);

		/// <summary>
		/// Parses an multiplication operator.
		/// </summary>
		private static Parser<Op> Multiply =>
			Op<Op>((l, r) => new MultiplyExpression(l, r), "*");

		/// <summary>
		/// Parses a division operator.
		/// </summary>
		private static Parser<Op> Divide =>
			Op<Op>((l, r) => new DivideExpression(l, r), "/");

		/// <summary>
		/// Parses a number literal.
		/// </summary>
		private static Parser<Expression> NumberLiteral
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var num = Parse.Double(state);

						transaction.CommitIndex();
						return new NumberLiteral(num);
					}
				};
			}
		}

		/// <summary>
		/// Parses a number variable.
		/// </summary>
		private static Parser<Expression> NumberVariable
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var name = Variable(Engine.ValueType.Number)(state);

						transaction.CommitIndex();
						return new NumberVariable(name);
					}
				};
			}
		}

		/// <summary>
		/// Parses a math expression wrapped in parenthesis.
		/// </summary>
		private static Parser<Expression> MathParenthesis
		{
			get
			{
				return state =>
				{
					Parse.Whitespaces(state);
					Parse.SameOrIndented(state);
					var math = Parse.SurroundBlock('(', ')',
						Math, Parse.SameOrIndented
					)(state);

					return math;
				};
			}
		}

		#endregion

		#region Substitution

		/// <summary>
		/// Parses a substitution. Shared by the <see cref="Text"/> and
		/// <see cref="Quote"/> parsers.
		/// </summary>
		private static Parser<Expression> Substitution =>
			Parse.SurroundBlock('{', '}',
				Logic.Select(x => (Expression)
					new ToStringExpression(x)),
				Parse.SameOrIndented
			).Or(Parse.SurroundBlock('{', '}',
				Math.Select(x => (Expression)
					new ToStringExpression(x)),
				Parse.SameOrIndented
			)).Or(Parse.SurroundBlock('{', '}',
				Quote,
				Parse.SameOrIndented
			));

		#endregion

		#region Text

		/// <summary>
		/// Parses a text expression, or a block of unquoted strings, which will
		/// return a <see cref="StringValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression> Text =>
			PrefixText<Unit>(null);

		/// <summary>
		/// Parses a text expression, or a block of unquoted strings, that are
		/// prefixed with <paramref name="prefix"/> which will return a
		/// <see cref="StringValue"/> when evaluated.
		/// </summary>
		/// <param name="prefix">
		/// The prefix parser to use at the beginning of each line, or null for
		/// no prefix parsing.
		/// </param>
		public static Parser<Expression> PrefixText<T>
			(Parser<T> prefix)
		{
			return state =>
			{
				using (var transaction = new Transaction(state))
				{
					// Parse each line of the text
					var lines = Parse.PrefixBlock1(
						prefix,
						TextLine,
						Parse.Indented
					)(state);

					// Combine each line of the text into a single expression
					Expression result = null;
					foreach (var line in lines)
					{
						if (result != null)
						{
							var s = new StringValue(" ");
							result = new ConcatExpression(
								new ConcatExpression(result, new StringLiteral(s)),
								line
							);
						}
						else
						{
							result = line;
						}
					}

					transaction.CommitIndex();
					return result.Simplify();
				}
			};
		}

		/// <summary>
		/// Parses a single line of a text block.
		/// </summary>
		private static Parser<Expression> TextLine
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						// If there is at least one space, prepend the resulting
						// string with a space. This is to maintain the space
						// between words across different lines.
						var s = "";
						if (Parse.FollowedBy(Parse.Spaces1)(state))
						{
							Parse.Spaces1(state);
							s = " ";
						}

						var rest =
							Parse.AnyChar
							.Until(Parse.EOL.Or(Parse.Char('{').Then(new Unit())))
							.String()(state);

						// If we're at the end of the line, there is nothing
						// left to parse.
						if (Parse.FollowedBy(Parse.EOL)(state))
						{
							transaction.CommitIndex();
							return new StringLiteral(s + rest);
						}

						// Otherwise, we've found a substitution that needs to
						// be parsed.
						else
						{
							var substitution = Substitution(state);

							// Parse the rest of the line (which may contain
							// another substitution)
							var remaining = TextLine(state);

							transaction.CommitIndex();
							return new ConcatExpression(
								new ConcatExpression(
									new StringLiteral(s + rest),
									substitution
								),
								remaining
							);
						}
					}
				};
			}
		}

		#endregion

		#region Quote

		/// <summary>
		/// Parses a text block.
		/// </summary>
		private static Parser<Expression> QuoteBlock =>
			Parse.SurroundBlock('{', '}', Quote, Parse.SameOrIndented);

		/// <summary>
		/// Parses a quote expression, or a quoted string, which will return a
		/// <see cref="StringValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression> Quote =>
			Parse.Surround('\"', '\"', QuoteInternal).Or(StringVariable);

		private static Parser<Expression> QuoteLiteral
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var quote = Quote(state);

						transaction.CommitIndex();
						return quote;
					}
				};
			}
		}

		/// <summary>
		/// Parses a string variable.
		/// </summary>
		private static Parser<Expression> StringVariable
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						var name = Variable(Engine.ValueType.String)(state);

						transaction.CommitIndex();
						return new StringVariable(name);
					}
				};
			}
		}

		private static Parser<Expression> QuoteInternal
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var start = Parse.AnyChar
							.Until(Parse.Char('\\', '{', '\"'))
							.String()
							.Select(str => new StringLiteral(str))
							(state);

						var rest = EscapeSequence
							.Or(SubstitutionQuote)
							.Or(Parse.Pure<Expression>(null))
							(state);

						transaction.CommitIndex();
						if (rest != null)
						{
							return new ConcatExpression(start, rest);
						}
						else
						{
							return start;
						}
					}
				};
			}
		}

		private static Parser<Expression> EscapeSequence
		{
			get
			{
				return Parse.String("\\n").Then("\n")
					.Or(Parse.String("\\r").Then("\r"))
					.Or(Parse.String("\\{").Then("{"))
					.Or(Parse.String("\\\"").Then("\""))
					.Or(Parse.String("\\\\").Then("\\"))
					.Select(str =>
						(Expression)new StringLiteral(str)
					);
			}
		}

		private static Parser<Expression> SubstitutionQuote
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var sub = Substitution(state);
						var rest = QuoteInternal(state);

						transaction.CommitIndex();
						return new ConcatExpression(sub, rest);
					}
				};
			}

		}

		#endregion
	}
}
