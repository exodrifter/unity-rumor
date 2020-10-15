using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;

namespace Exodrifter.Rumor.Compiler
{
	// Define type aliases for convenience
	using NumberOperator =
		Func<Expression<NumberValue>, Expression<NumberValue>, Expression<NumberValue>>;
	using BooleanOperator =
		Func<Expression<BooleanValue>, Expression<BooleanValue>, Expression<BooleanValue>>;

	public static class Compiler
	{
		public static Parser<SayNode> SayNode()
		{
			return (ref State state) =>
			{
				var temp = state.SetIndent();

				var identifier = Identifier().Maybe()(ref temp)
					.GetValueOrDefault(null);

				Parse.Spaces(ref temp);
				Parse.Char(':')(ref temp);
				Parse.Whitespaces(ref temp);

				var dialog = Text()(ref temp);

				state = temp;
				return new SayNode(identifier, dialog);
			};
		}

		public static Parser<string> Identifier()
		{
			return (ref State state) =>
			{
				return
					Parse.Alphanumeric.Many(1).String()
					.Where(x => !x.StartsWith("_"), "identifier")
					(ref state);
			};
		}

		#region Logic

		/// <summary>
		/// Parses a logic expression, which will return a
		/// <see cref="BooleanLiteral"/> when evaluated.
		/// </summary>
		public static Parser<Expression<BooleanValue>> Logic() =>
			Parse.ChainL1(AndPieces(), Or());

		// Groups the "and" operators
		private static Parser<Expression<BooleanValue>> AndPieces() =>
			Parse.ChainL1(XorPieces(), And());

		// Groups the "xor" operators
		private static Parser<Expression<BooleanValue>> XorPieces() =>
			Parse.ChainL1(LogicPiece(), Xor());

		private static Parser<Expression<BooleanValue>> LogicPiece() =>
			Parse.Surround('(', ')', Parse.Ref(Logic), Parse.SameOrIndented)
				.Or(NotExpression())
				.Or(BooleanLiteral());

		/// <summary>
		/// Parses a logic or operator.
		/// </summary>
		private static Parser<BooleanOperator> Or()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.String("or", "||")(ref temp);
				state = temp;

				BooleanOperator op = (l, r) => new OrExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a logic and operator.
		/// </summary>
		private static Parser<BooleanOperator> And()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.String("and", "&&")(ref temp);
				state = temp;

				BooleanOperator op = (l, r) => new AndExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a logic xor operator.
		/// </summary>
		private static Parser<BooleanOperator> Xor()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.String("xor", "^")(ref temp);
				state = temp;

				BooleanOperator op = (l, r) => new XorExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a boolean literal.
		/// </summary>
		private static Parser<Expression<BooleanValue>> BooleanLiteral()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				var b = Parse.String("true").Then(true)
						.Or(Parse.String("false").Then(false))
						(ref temp);
				state = temp;

				return new BooleanLiteral(b);
			};
		}

		/// <summary>
		/// Parses a logic not operator and the logic expression associated
		/// with it.
		/// </summary>
		private static Parser<Expression<BooleanValue>> NotExpression()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.String("not", "!")(ref temp);
				var logic = Parse.Ref(() => Logic())(ref temp);
				state = temp;

				return new NotExpression(logic);
			};
		}

		#endregion

		#region Math

		/// <summary>
		/// Parses an arithmetic expression, which will return a
		/// <see cref="NumberValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression<NumberValue>> Math() =>
			Parse.ChainL1(MultipliedPieces(), AddOrSubtract());

		// Groups the multiplication and division operators
		private static Parser<Expression<NumberValue>> MultipliedPieces() =>
			Parse.ChainL1(MathPiece(), MultiplyOrDivide());

		private static Parser<Expression<NumberValue>> MathPiece() =>
			Parse.Surround('(', ')', Parse.Ref(Math), Parse.SameOrIndented)
				.Or(NumberLiteral());

		/// <summary>
		/// Parses an addition or subtraction operator.
		/// </summary>
		private static Parser<NumberOperator> AddOrSubtract() =>
			Add().Or(Subtract());

		/// <summary>
		/// Parses an addition operator.
		/// </summary>
		private static Parser<NumberOperator> Add()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.Char('+')(ref temp);
				state = temp;

				NumberOperator op = (l, r) => new AddExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a subtraction operator.
		/// </summary>
		private static Parser<NumberOperator> Subtract()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.Char('-')(ref temp);
				state = temp;

				NumberOperator op = (l, r) => new SubtractExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a multiplication or division operator.
		/// </summary>
		private static Parser<NumberOperator> MultiplyOrDivide() =>
			Multiply().Or(Divide());

		/// <summary>
		/// Parses an multiplication operator.
		/// </summary>
		private static Parser<NumberOperator> Multiply()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.Char('*')(ref temp);
				state = temp;

				NumberOperator op = (l, r) => new MultiplyExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a division operator.
		/// </summary>
		private static Parser<NumberOperator> Divide()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				Parse.Char('/')(ref temp);
				state = temp;

				NumberOperator op = (l, r) => new DivideExpression(l, r);
				return op;
			};
		}

		/// <summary>
		/// Parses a number literal.
		/// </summary>
		private static Parser<Expression<NumberValue>> NumberLiteral()
		{
			return (ref State state) =>
			{
				var temp = state;
				Parse.Whitespaces(ref temp);
				Parse.SameOrIndented(ref temp);
				var num = Parse.Double(ref temp);
				state = temp;

				return new NumberLiteral(num);
			};
		}

		#endregion

		#region Substitution

		/// <summary>
		/// Parses a substitution. Shared by the <see cref="Text"/> and
		/// <see cref="Quote"/> parsers.
		/// </summary>
		private static Parser<Expression<StringValue>> Substitution =>
			Parse.Surround('{', '}',
				Math().Select(x => (Expression<StringValue>)
					new ToStringExpression<NumberValue>(x))
				.Or(Logic().Select(x => (Expression<StringValue>)
					new ToStringExpression<BooleanValue>(x)))
				.Or(Quote())
			);

		#endregion

		#region Text

		/// <summary>
		/// Parses a text expression, or a block of unquoted strings, which will
		/// return a <see cref="StringValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression<StringValue>> Text()
		{
			return (ref State state) =>
			{
				var temp = state;

				// Parse each line of the text
				var lines = Parse.Block1(
					TextLine(),
					Parse.Indented
				)(ref temp);

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

				state = temp;
				return result.Simplify();
			};
		}

		/// <summary>
		/// Parses a single line of a text block.
		/// </summary>
		private static Parser<Expression<StringValue>> TextLine()
		{
			return (ref State state) =>
			{
				var temp = state;

				// If there is at least one space, prepend the resulting string
				// with a space. This is to maintain the space between words
				// across different lines.
				var s = "";
				if (Parse.FollowedBy(Parse.Spaces1)(ref temp))
				{
					Parse.Spaces1(ref temp);
					s = " ";
				}

				var rest =
					Parse.AnyChar
					.Until(Parse.EOL.Or(Parse.Char('{').Then(new Unit())))
					.String()(ref temp);

				// If we're at the end of the line, there is nothing left to
				// parse.
				if (Parse.FollowedBy(Parse.EOL)(ref temp))
				{
					state = temp;
					return new StringLiteral(s + rest);
				}

				// Otherwise, we've found a substitution that needs to be
				// parsed.
				else
				{
					var substitution = Substitution(ref temp);

					// Parse the rest of the line (which may contain another
					// substitution)
					var remaining = TextLine()(ref temp);

					state = temp;
					return new ConcatExpression(
						new ConcatExpression(
							new StringLiteral(s + rest),
							substitution
						),
						remaining
					);
				}
			};
		}

		#endregion

		#region Quote

		/// <summary>
		/// Parses a quote expression, or a quoted string, which will return a
		/// <see cref="StringValue"/> when evaluated.
		/// </summary>
		public static Parser<Expression<StringValue>> Quote()
		{
			return Parse.Surround('\"', '\"', QuoteInternal());
		}

		private static Parser<Expression<StringValue>> QuoteInternal()
		{
			return (ref State state) =>
			{
				var start = Parse.AnyChar
					.Until(Parse.Char('\\', '{', '\"'))
					.String()
					.Select(str => new StringLiteral(str))
					(ref state);

				var rest = EscapeSequence()
					.Or(SubstitutionQuote())
					.Or(Parse.Pure<Expression<StringValue>>(null))
					(ref state);

				if (rest != null)
				{
					return new ConcatExpression(start, rest);
				}
				else
				{
					return start;
				}
			};
		}

		private static Parser<Expression<StringValue>> EscapeSequence()
		{
			return Parse.String("\\n").Then("\n")
				.Or(Parse.String("\\r").Then("\r"))
				.Or(Parse.String("\\{").Then("{"))
				.Or(Parse.String("\\\"").Then("\""))
				.Or(Parse.String("\\\\").Then("\\"))
				.Select(str => (Expression<StringValue>)new StringLiteral(str));
		}

		private static Parser<Expression<StringValue>> SubstitutionQuote()
		{
			return (ref State state) =>
			{
				var sub = Substitution(ref state);
				var rest = Parse.Ref(QuoteInternal)(ref state);
				return new ConcatExpression(sub, rest);
			};
		}

		#endregion
	}
}
