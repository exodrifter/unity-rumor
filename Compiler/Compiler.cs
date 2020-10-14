using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler
{
	using NumberOperator =
		Func<Expression<NumberValue>, Expression<NumberValue>, Expression<NumberValue>>;

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

				var lines = Parse.Block1(
					Parse.AnyChar.Until(Parse.EOL).String(),
					Parse.Indented
				)(ref temp);

				var dialog = new List<string>();
				foreach (var line in lines)
				{
					dialog.Add(line.Trim());
				}

				state = temp;
				return new SayNode(identifier, string.Join(" ", dialog));
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

		/// <summary>
		/// Parses an arithmetic expression.
		/// </summary>
		public static Parser<Expression<NumberValue>> Math() =>
			Parse.ChainL1(NumberLiteral(), AddOrSubtract())
				.Or(NumberLiteral());

		/// <summary>
		/// Parses a number literal.
		/// </summary>
		public static Parser<Expression<NumberValue>> NumberLiteral()
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
		public static Parser<NumberOperator> AddOrSubtract() =>
			Add().Or(Subtract());

		/// <summary>
		/// Parses an addition operator.
		/// </summary>
		public static Parser<NumberOperator> Add()
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
		public static Parser<NumberOperator> Subtract()
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
	}
}
