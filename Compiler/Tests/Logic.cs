using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Logic
	{
		#region Literal

		[Test]
		public static void BooleanTrueSuccess()
		{
			var state = new State("true", 4, 0);

			var n = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), n.Evaluate());
		}

		[Test]
		public static void BooleanFalseSuccess()
		{
			var state = new State("false", 4, 0);

			var n = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), n.Evaluate());
		}

		#endregion

		#region Not

		[Test]
		public static void NotTrueSuccess()
		{
			var state = new State("not true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void NotFalseSuccess()
		{
			var state = new State("not false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		#endregion

		#region Or

		[Test]
		public static void OrTrueTrueSuccess()
		{
			var state = new State("true or true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void OrTrueFalseSuccess()
		{
			var state = new State("true or false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void OrFalseFalseSuccess()
		{
			var state = new State("false or false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void OrOpTrueTrueSuccess()
		{
			var state = new State("true || true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void OrOpTrueFalseSuccess()
		{
			var state = new State("true || false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void OrOpFalseFalseSuccess()
		{
			var state = new State("false || false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		#endregion

		#region And

		[Test]
		public static void AndTrueTrueSuccess()
		{
			var state = new State("true and true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void AndTrueFalseSuccess()
		{
			var state = new State("true and false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void AndFalseFalseSuccess()
		{
			var state = new State("false and false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void AndOpTrueTrueSuccess()
		{
			var state = new State("true && true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void AndOpTrueFalseSuccess()
		{
			var state = new State("true && false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void AndOpFalseFalseSuccess()
		{
			var state = new State("false && false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		#endregion

		#region Xor

		[Test]
		public static void XorTrueTrueSuccess()
		{
			var state = new State("true xor true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void XorTrueFalseSuccess()
		{
			var state = new State("true xor false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void XorFalseFalseSuccess()
		{
			var state = new State("false xor false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void XorOpTrueTrueSuccess()
		{
			var state = new State("true ^ true", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		[Test]
		public static void XorOpTrueFalseSuccess()
		{
			var state = new State("true ^ false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void XorOpFalseFalseSuccess()
		{
			var state = new State("false ^ false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate());
		}

		#endregion

		#region Complex

		[Test]
		public static void ComplexLogicSuccess()
		{
			var state = new State("false || true && true ^ false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		[Test]
		public static void ComplexLogicMultilineSuccess()
		{
			var state = new State("false\n || true\n && true\n ^ false", 4, 0);

			var exp = ExpressionCompiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate());
		}

		#endregion
	}
}
