using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Arithmetic
	{
		#region Literal

		[Test]
		public static void IntegerSuccess()
		{
			var state = new ParserState("01234567890", 4);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1234567890d), n.Evaluate(new RumorScope()));
		}

		[Test]
		public static void PositiveIntegerSuccess()
		{
			var state = new ParserState("+01234567890", 4);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1234567890d), n.Evaluate(new RumorScope()));
		}

		[Test]
		public static void NegativeIntegerSuccess()
		{
			var state = new ParserState("-01234567890", 4);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-1234567890d), n.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DecimalSuccess()
		{
			var state = new ParserState("01234567890.123456789", 4);

			var n = Compiler.Math(state);
			Assert.AreEqual
				(new NumberValue(1234567890.123456789d), n.Evaluate(new RumorScope()));
		}

		[Test]
		public static void PositiveDecimalSuccess()
		{
			var state = new ParserState("+01234567890.123456789", 4);

			var n = Compiler.Math(state);
			Assert.AreEqual
				(new NumberValue(1234567890.123456789d), n.Evaluate(new RumorScope()));
		}

		[Test]
		public static void NegativeDecimalSuccess()
		{
			var state = new ParserState("-01234567890.123456789", 4);

			var n = Compiler.Math(state);
			Assert.AreEqual
				(new NumberValue(-1234567890.123456789d), n.Evaluate(new RumorScope()));
		}

		[Test]
		public static void VariableSuccess()
		{
			var state = new ParserState("foobar", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", 1234);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1234), n.Evaluate(scope));
		}

		#endregion

		#region Addition

		[Test]
		public static void AdditionSuccess()
		{
			var state = new ParserState("1+2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(3d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AdditionVariableSuccess()
		{
			var state = new ParserState("foobar + 2", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", 1234);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1236), n.Evaluate(scope));
		}

		[Test]
		public static void AdditionPlusSignSuccess()
		{
			var state = new ParserState("1++2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(3d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AdditionMinusSignSuccess()
		{
			var state = new ParserState("1+-2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-1d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AdditionWhitespaceSuccess()
		{
			var state = new ParserState("1  +   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(3d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AdditionMultilineSuccess()
		{
			var state = new ParserState("1\n   +\n   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(3d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AdditionMultipleSuccess()
		{
			var state = new ParserState("1 + 2 + 3 + 4", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(10d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AdditionMultipleMultilineSuccess()
		{
			var state = new ParserState("1 +\n 2 +\n 3 + 4", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(10d), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Subtraction

		[Test]
		public static void SubtractionSuccess()
		{
			var state = new ParserState("1-2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-1d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void SubtractionVariableSuccess()
		{
			var state = new ParserState("foobar - 2", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", 1234);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1232), n.Evaluate(scope));
		}

		[Test]
		public static void SubtractionPlusSignSuccess()
		{
			var state = new ParserState("1-+2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-1d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void SubtractionMinusSignSuccess()
		{
			var state = new ParserState("1--2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(3d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void SubtractionWhitespaceSuccess()
		{
			var state = new ParserState("1  -   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-1d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void SubtractionMultilineSuccess()
		{
			var state = new ParserState("1\n   -\n   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-1d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void SubtractionMultipleSuccess()
		{
			var state = new ParserState("1 - 2 - 3 - 4", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-8d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void SubtractionMultipleMultilineSuccess()
		{
			var state = new ParserState("1 -\n 2 -\n 3 - 4", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-8d), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Multiplication

		[Test]
		public static void MultiplicationSuccess()
		{
			var state = new ParserState("1*2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(2d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void MultiplicationVariableSuccess()
		{
			var state = new ParserState("foobar * 2", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", 1234);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(2468), n.Evaluate(scope));
		}

		[Test]
		public static void MultiplicationPlusSignSuccess()
		{
			var state = new ParserState("1*+2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(2d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void MultiplicationMinusSignSuccess()
		{
			var state = new ParserState("1*-2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-2d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void MultiplicationWhitespaceSuccess()
		{
			var state = new ParserState("1  *   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(2d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void MultiplicationMultilineSuccess()
		{
			var state = new ParserState("1\n   *\n   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(2d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void MultiplicationMultipleSuccess()
		{
			var state = new ParserState("1 * 2 * 3 * 4", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(24d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void MultiplicationMultipleMultilineSuccess()
		{
			var state = new ParserState("1 *\n 2 *\n 3 * 4", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(24d), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Division

		[Test]
		public static void DivisionSuccess()
		{
			var state = new ParserState("1/2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(0.5d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DivisionVariableSuccess()
		{
			var state = new ParserState("foobar / 2", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", 1234);

			var n = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(617), n.Evaluate(scope));
		}

		[Test]
		public static void DivisionPlusSignSuccess()
		{
			var state = new ParserState("1/+2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(0.5d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DivisionMinusSignSuccess()
		{
			var state = new ParserState("1/-2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(-0.5d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DivisionWhitespaceSuccess()
		{
			var state = new ParserState("1  /   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(0.5d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DivisionMultilineSuccess()
		{
			var state = new ParserState("1\n   /\n   2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(0.5d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DivisionMultipleSuccess()
		{
			var state = new ParserState("8 / 4 / 2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void DivisionMultipleMultilineSuccess()
		{
			var state = new ParserState("8 /\n 4 /\n 2", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(1d), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Mixed Arithmetic

		[Test]
		public static void MixedArithmeticSuccess()
		{
			var state = new ParserState("8 + 2 * 5 / 5 + 1", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(11d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void ParenthesisSuccess()
		{
			var state = new ParserState("(8 + 2) * 5", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(50d), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void ParenthesisMultilineSuccess()
		{
			var state = new ParserState("(8 \n + 2) * 5", 4);

			var exp = Compiler.Math(state);
			Assert.AreEqual(new NumberValue(50d), exp.Evaluate(new RumorScope()));
		}

		#endregion
	}
}
