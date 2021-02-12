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
			var state = new ParserState("true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void BooleanFalseSuccess()
		{
			var state = new ParserState("false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Not

		[Test]
		public static void NotTrueSuccess()
		{
			var state = new ParserState("not true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void NotFalseSuccess()
		{
			var state = new ParserState("not false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Or

		[Test]
		public static void OrTrueTrueSuccess()
		{
			var state = new ParserState("true or true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void OrTrueFalseSuccess()
		{
			var state = new ParserState("true or false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void OrFalseFalseSuccess()
		{
			var state = new ParserState("false or false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void OrOpTrueTrueSuccess()
		{
			var state = new ParserState("true || true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void OrOpTrueFalseSuccess()
		{
			var state = new ParserState("true || false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void OrOpFalseFalseSuccess()
		{
			var state = new ParserState("false || false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region And

		[Test]
		public static void AndTrueTrueSuccess()
		{
			var state = new ParserState("true and true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AndTrueFalseSuccess()
		{
			var state = new ParserState("true and false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AndFalseFalseSuccess()
		{
			var state = new ParserState("false and false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AndOpTrueTrueSuccess()
		{
			var state = new ParserState("true && true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AndOpTrueFalseSuccess()
		{
			var state = new ParserState("true && false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void AndOpFalseFalseSuccess()
		{
			var state = new ParserState("false && false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Xor

		[Test]
		public static void XorTrueTrueSuccess()
		{
			var state = new ParserState("true xor true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void XorTrueFalseSuccess()
		{
			var state = new ParserState("true xor false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void XorFalseFalseSuccess()
		{
			var state = new ParserState("false xor false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void XorOpTrueTrueSuccess()
		{
			var state = new ParserState("true ^ true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void XorOpTrueFalseSuccess()
		{
			var state = new ParserState("true ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void XorOpFalseFalseSuccess()
		{
			var state = new ParserState("false ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(false), exp.Evaluate(new RumorScope()));
		}

		#endregion

		#region Complex

		[Test]
		public static void ComplexLogicSuccess()
		{
			var state = new ParserState("false || true && true ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		[Test]
		public static void ComplexLogicMultilineSuccess()
		{
			var state = new ParserState("false\n || true\n && true\n ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(new BooleanValue(true), exp.Evaluate(new RumorScope()));
		}

		#endregion
	}
}
