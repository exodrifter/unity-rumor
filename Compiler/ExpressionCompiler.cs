using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;

namespace Exodrifter.Rumor.Compiler
{
	// Define type aliases for convenience
	using NumberOp =
		Func<Expression<NumberValue>, Expression<NumberValue>, Expression<NumberValue>>;
	using BooleanOp =
		Func<Expression<BooleanValue>, Expression<BooleanValue>, Expression<BooleanValue>>;

	public static class ExpressionCompiler
	{
		#region Logic

		/// <summary>
		/// Parses a logic expression, which will return a
		/// <see cref="BooleanLiteral"/> when evaluated.
		/// </summary>
		public static Parser<Expression<BooleanValue>> Logic =>
			Parse.ChainL1(AndPieces, Or);

		// Groups the "and" operators
		private static Parser<Expression<BooleanValue>> AndPieces =>
			Parse.ChainL1(XorPieces, And);

		// Groups the "xor" operators
		private static Parser<Expression<BooleanValue>> XorPieces =>
			Parse.ChainL1(LogicPiece, Xor);

		private static Parser<Expression<BooleanValue>> LogicPiece =>
			Parse.SurroundBlock('(', ')',
				Parse.Ref(() => Logic),
				Parse.SameOrIndented
			).Or(NotExpression)
			.Or(BooleanLiteral);

		/// <summary>
		/// Parses a logic or operator.
		/// </summary>
		private static Parser<BooleanOp> Or
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.String("or", "||")(state);

						transaction.Commit();
						BooleanOp op = (l, r) => new OrExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a logic and operator.
		/// </summary>
		private static Parser<BooleanOp> And
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.String("and", "&&")(state);

						transaction.Commit();
						BooleanOp op = (l, r) => new AndExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a logic xor operator.
		/// </summary>
		private static Parser<BooleanOp> Xor
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.String("xor", "^")(state);

						transaction.Commit();
						BooleanOp op = (l, r) => new XorExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a boolean literal.
		/// </summary>
		private static Parser<Expression<BooleanValue>> BooleanLiteral
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

						transaction.Commit();
						return new BooleanLiteral(b);
					}
				};
			}
		}

		/// <summary>
		/// Parses a logic not operator and the logic expression associated
		/// with it.
		/// </summary>
		private static Parser<Expression<BooleanValue>> NotExpression
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
						var logic = Logic(state);

						transaction.Commit();
						return new NotExpression(logic);
					}
				};
			}
		}

		#endregion

		#region Math

		/// <summary>
		/// Parses an arithmetic expression, which will return a
		/// <see cref="NumberValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression<NumberValue>> Math =>
			Parse.ChainL1(MultipliedPieces, AddOrSubtract);

		// Groups the multiplication and division operators
		private static Parser<Expression<NumberValue>> MultipliedPieces =>
			Parse.ChainL1(MathPiece, MultiplyOrDivide);

		private static Parser<Expression<NumberValue>> MathPiece =>
			Parse.SurroundBlock('(', ')',
				Parse.Ref(() => Math),
				Parse.SameOrIndented
			).Or(NumberLiteral);

		/// <summary>
		/// Parses an addition or subtraction operator.
		/// </summary>
		private static Parser<NumberOp> AddOrSubtract =>
			Add.Or(Subtract);

		/// <summary>
		/// Parses an addition operator.
		/// </summary>
		private static Parser<NumberOp> Add
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.Char('+')(state);

						transaction.Commit();
						NumberOp op = (l, r) => new AddExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a subtraction operator.
		/// </summary>
		private static Parser<NumberOp> Subtract
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.Char('-')(state);

						transaction.Commit();
						NumberOp op = (l, r) => new SubtractExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a multiplication or division operator.
		/// </summary>
		private static Parser<NumberOp> MultiplyOrDivide =>
			Multiply.Or(Divide);

		/// <summary>
		/// Parses an multiplication operator.
		/// </summary>
		private static Parser<NumberOp> Multiply
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.Char('*')(state);

						transaction.Commit();
						NumberOp op = (l, r) => new MultiplyExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a division operator.
		/// </summary>
		private static Parser<NumberOp> Divide
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						Parse.Whitespaces(state);
						Parse.SameOrIndented(state);
						Parse.Char('/')(state);

						transaction.Commit();
						NumberOp op = (l, r) => new DivideExpression(l, r);
						return op;
					}
				};
			}
		}

		/// <summary>
		/// Parses a number literal.
		/// </summary>
		private static Parser<Expression<NumberValue>> NumberLiteral
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

						transaction.Commit();
						return new NumberLiteral(num);
					}
				};
			}
		}

		#endregion

		#region Substitution

		/// <summary>
		/// Parses a substitution. Shared by the <see cref="Text"/> and
		/// <see cref="Quote"/> parsers.
		/// </summary>
		private static Parser<Expression<StringValue>> Substitution =>
			Parse.Surround('{', '}',
				Math.Select(x => (Expression<StringValue>)
					new ToStringExpression<NumberValue>(x))
				.Or(Logic.Select(x => (Expression<StringValue>)
					new ToStringExpression<BooleanValue>(x)))
				.Or(Quote)
			);

		#endregion

		#region Text

		/// <summary>
		/// Parses a text expression, or a block of unquoted strings, which will
		/// return a <see cref="StringValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression<StringValue>> Text
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						// Parse each line of the text
						var lines = Parse.Block1(
							TextLine,
							Parse.Indented
						)(state);

						// Combine each line of the text into a single expression
						Expression<StringValue> result = null;
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

						transaction.Commit();
						return result.Simplify();
					}
				};
			}
		}

		/// <summary>
		/// Parses a single line of a text block.
		/// </summary>
		private static Parser<Expression<StringValue>> TextLine
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
							transaction.Commit();
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

							transaction.Commit();
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
		/// Parses a quote expression, or a quoted string, which will return a
		/// <see cref="StringValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression<StringValue>> Quote =>
			Parse.Surround('\"', '\"', QuoteInternal);

		private static Parser<Expression<StringValue>> QuoteInternal
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
							.Or(Parse.Pure<Expression<StringValue>>(null))
							(state);

						transaction.Commit();
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

		private static Parser<Expression<StringValue>> EscapeSequence
		{
			get
			{
				return Parse.String("\\n").Then("\n")
					.Or(Parse.String("\\r").Then("\r"))
					.Or(Parse.String("\\{").Then("{"))
					.Or(Parse.String("\\\"").Then("\""))
					.Or(Parse.String("\\\\").Then("\\"))
					.Select(str =>
						(Expression<StringValue>)new StringLiteral(str)
					);
			}
		}

		private static Parser<Expression<StringValue>> SubstitutionQuote
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var sub = Substitution(state);
						var rest = QuoteInternal(state);

						transaction.Commit();
						return new ConcatExpression(sub, rest);
					}
				};
			}
			
		}

		#endregion
	}
}
