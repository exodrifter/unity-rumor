using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler
{
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

		public static Parser<Expression<BooleanValue>> Logic() =>
			Parse.ChainL1(AndPieces(), Or());

		private static Parser<Expression<BooleanValue>> AndPieces() =>
			Parse.ChainL1(XorPieces(), And());

		private static Parser<Expression<BooleanValue>> XorPieces() =>
			Parse.ChainL1(LogicPiece(), Xor());

		// Either logic expressions surrounded by parenthesis or boolean literals
		private static Parser<Expression<BooleanValue>> LogicPiece() =>
			Parse.Parenthesis(
					'(', ')', Parse.Ref(() => Logic()), Parse.SameOrIndented
				).Or(BooleanLiteral());

		/// <summary>
		/// Parses a number literal.
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

				return new LiteralExpression<BooleanValue>(new BooleanValue(b));
			};
		}

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

		#endregion

		#region Text

		public static Parser<Expression<StringValue>> Text()
		{
			return (ref State state) =>
			{
				var temp = state;

				var lines = Parse.Block1(
					TextLine(),
					Parse.Indented
				)(ref temp);

				Expression<StringValue> dialog = null;
				foreach (var line in lines)
				{
					if (dialog != null)
					{
						var s = new StringValue(" ");
						dialog = new ConcatExpression(
							new ConcatExpression(dialog, s),
							line
						);
					}
					else
					{
						dialog = line;
					}
				}

				state = temp;

				return dialog.Simplify();
			};
		}

		private static Parser<Expression<StringValue>> TextLine()
		{
			return (ref State state) =>
			{
				var temp = state;

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

				if (Parse.FollowedBy(Parse.EOL)(ref temp))
				{
					state = temp;
					return new LiteralExpression<StringValue>(
						new StringValue(s + rest)
					);
				}
				else
				{
					var substitution =
						Parse.Parenthesis('{', '}', Math())(ref temp);

					var remaining = TextLine()(ref temp);

					state = temp;
					return new ConcatExpression(
						new ConcatExpression(
							new StringValue(s + rest),
							new ToStringExpression<NumberValue>(substitution)
						),
						remaining
					);
				}
			};
		}

		#endregion

		#region Math

		/// <summary>
		/// Parses an arithmetic expression.
		/// </summary>
		public static Parser<Expression<NumberValue>> Math() =>
			Parse.ChainL1(MultipliedPieces(), AddOrSubtract());

		// Groups the multiplication and division operators
		private static Parser<Expression<NumberValue>> MultipliedPieces() =>
			Parse.ChainL1(MathPiece(), MultiplyOrDivide());

		// Either math expressions surrounded by parenthesis or number literals
		private static Parser<Expression<NumberValue>> MathPiece() =>
			Parse.Parenthesis(
					'(', ')', Parse.Ref(() => Math()), Parse.SameOrIndented
				).Or(NumberLiteral());

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

				return new LiteralExpression<NumberValue>(new NumberValue(num));
			};
		}

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
		/// Parses an addition or subtraction operator.
		/// </summary>
		private static Parser<NumberOperator> MultiplyOrDivide() =>
			Multiply().Or(Divide());

		/// <summary>
		/// Parses an addition operator.
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
		/// Parses a subtraction operator.
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

		#endregion
	}
}
