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
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void BooleanFalseSuccess()
		{
			var state = new ParserState("false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		#endregion

		#region Not

		[Test]
		public static void NotTrueSuccess()
		{
			var state = new ParserState("not true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void NotFalseSuccess()
		{
			var state = new ParserState("not false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		#endregion

		#region Or

		[Test]
		public static void OrTrueTrueSuccess()
		{
			var state = new ParserState("true or true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void OrTrueFalseSuccess()
		{
			var state = new ParserState("true or false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void OrFalseFalseSuccess()
		{
			var state = new ParserState("false or false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void OrOpTrueTrueSuccess()
		{
			var state = new ParserState("true || true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void OrOpTrueFalseSuccess()
		{
			var state = new ParserState("true || false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void OrOpFalseFalseSuccess()
		{
			var state = new ParserState("false || false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		#endregion

		#region And

		[Test]
		public static void AndTrueTrueSuccess()
		{
			var state = new ParserState("true and true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void AndTrueFalseSuccess()
		{
			var state = new ParserState("true and false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void AndFalseFalseSuccess()
		{
			var state = new ParserState("false and false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void AndOpTrueTrueSuccess()
		{
			var state = new ParserState("true && true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void AndOpTrueFalseSuccess()
		{
			var state = new ParserState("true && false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void AndOpFalseFalseSuccess()
		{
			var state = new ParserState("false && false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		#endregion

		#region Xor

		[Test]
		public static void XorTrueTrueSuccess()
		{
			var state = new ParserState("true xor true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void XorTrueFalseSuccess()
		{
			var state = new ParserState("true xor false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void XorFalseFalseSuccess()
		{
			var state = new ParserState("false xor false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void XorOpTrueTrueSuccess()
		{
			var state = new ParserState("true ^ true", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void XorOpTrueFalseSuccess()
		{
			var state = new ParserState("true ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void XorOpFalseFalseSuccess()
		{
			var state = new ParserState("false ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		#endregion

		#region Complex

		[Test]
		public static void ComplexLogicSuccess()
		{
			var state = new ParserState("false || true && true ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		[Test]
		public static void ComplexLogicMultilineSuccess()
		{
			var state = new ParserState("false\n || true\n && true\n ^ false", 4);

			var exp = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				exp.Evaluate(new RumorScope(), new RumorBindings())
			);
		}

		#endregion

		#region Function

		[Test]
		public static void Function0Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar", ValueType.Boolean);

			var state = new ParserState("foobar() and true", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<bool>("foobar", () => { return true; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Function1Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar", ValueType.Number, ValueType.Boolean);

			var state = new ParserState("foobar(3) and true", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, bool>("foobar",
				(i) => { return i > 8; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Function2Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(3, 4) and true", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, bool>("foobar",
				(i, j) => { return i + j > 8; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Function3Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number,
				ValueType.Number,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(3, 4, 5) and true", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int, bool>("foobar",
				(i, j, k) => { return i + j + k > 8; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Function4Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.Number,
				ValueType.Number,
				ValueType.Number,
				ValueType.Number,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(3, 4, 5, 6) and true", 4, hints);
			var bindings = new RumorBindings();
			bindings.Bind<int, int, int, int, bool>("foobar",
				(i, j, k, l) => { return i + j + k + l > 8; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		#endregion
	}
}
