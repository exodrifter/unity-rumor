using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Logic
	{
		#region Primitives

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

		[Test]
		public static void VariableSuccess()
		{
			var state = new ParserState("foobar", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", true);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(scope, new RumorBindings())
			);
		}

		[Test]
		public static void Binding0Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean);

			var state = new ParserState("foobar()", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind("foobar", () => { return true; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Binding1Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean, ValueType.Boolean);

			var state = new ParserState("foobar(true)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool>("foobar", (a) => { return a; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Binding2Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool>(
				"foobar", (a, b) => { return a ^ b; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Binding3Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool>(
				"foobar", (a, b, c) => { return a ^ b ^ c; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void Binding4Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true, false)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool, bool>(
				"foobar", (a, b, c, d) => { return a ^ b ^ c ^ d; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
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

		[Test]
		public static void NotVariableSuccess()
		{
			var state = new ParserState("not foobar", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", true);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(scope, new RumorBindings())
			);
		}

		[Test]
		public static void NotBinding0Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean);

			var state = new ParserState("not foobar()", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind("foobar", () => { return true; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void NotBinding1Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean, ValueType.Boolean);

			var state = new ParserState("not foobar(true)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool>("foobar", (a) => { return a; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void NotBinding2Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("not foobar(true, false)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool>(
				"foobar", (a, b) => { return a ^ b; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void NotBinding3Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("not foobar(true, false, true)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool>(
				"foobar", (a, b, c) => { return a ^ b ^ c; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void NotBinding4Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("not foobar(true, false, true, false)", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool, bool>(
				"foobar", (a, b, c, d) => { return a ^ b ^ c ^ d; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
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

		[Test]
		public static void OrVariableSuccess()
		{
			var state = new ParserState("foobar or true", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", false);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(scope, new RumorBindings())
			);
		}

		[Test]
		public static void OrBinding0Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean);

			var state = new ParserState("foobar() or true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind("foobar", () => { return false; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void OrBinding1Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean, ValueType.Boolean);

			var state = new ParserState("foobar(false) or true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool>("foobar", (a) => { return a; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void OrBinding2Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false) or false", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool>(
				"foobar", (a, b) => { return a ^ b; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void OrBinding3Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true) or false", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool>(
				"foobar", (a, b, c) => { return a ^ b ^ c; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void OrBinding4Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true, false) or false", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool, bool>(
				"foobar", (a, b, c, d) => { return a ^ b ^ c ^ d; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
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

		[Test]
		public static void AndVariableSuccess()
		{
			var state = new ParserState("foobar and true", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", false);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(scope, new RumorBindings())
			);
		}

		[Test]
		public static void AndBinding0Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean);

			var state = new ParserState("foobar() and true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind("foobar", () => { return false; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void AndBinding1Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean, ValueType.Boolean);

			var state = new ParserState("foobar(false) and true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool>("foobar", (a) => { return a; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void AndBinding2Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false) and false", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool>(
				"foobar", (a, b) => { return a ^ b; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void AndBinding3Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true) and true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool>(
				"foobar", (a, b, c) => { return a ^ b ^ c; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void AndBinding4Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true, false) and true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool, bool>(
				"foobar", (a, b, c, d) => { return a ^ b ^ c ^ d; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
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

		[Test]
		public static void XorVariableSuccess()
		{
			var state = new ParserState("foobar xor true", 4, new RumorParserState());
			var scope = new RumorScope();
			scope.Set("foobar", false);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(scope, new RumorBindings())
			);
		}

		[Test]
		public static void XorBinding0Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean);

			var state = new ParserState("foobar() xor true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind("foobar", () => { return false; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void XorBinding1Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction("foobar", ValueType.Boolean, ValueType.Boolean);

			var state = new ParserState("foobar(false) xor true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool>("foobar", (a) => { return a; });

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void XorBinding2Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false) xor true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool>(
				"foobar", (a, b) => { return a ^ b; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(false),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void XorBinding3Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true) xor true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool>(
				"foobar", (a, b, c) => { return a ^ b ^ c; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
			);
		}

		[Test]
		public static void XorBinding4Success()
		{
			var rps = new RumorParserState();
			rps.LinkFunction(
				"foobar",
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean,
				ValueType.Boolean
			);

			var state = new ParserState("foobar(true, false, true, false) xor true", 4, rps);
			var bindings = new RumorBindings();
			bindings.Bind<bool, bool, bool, bool, bool>(
				"foobar", (a, b, c, d) => { return a ^ b ^ c ^ d; }
			);

			var n = Compiler.Logic(state);
			Assert.AreEqual(
				new BooleanValue(true),
				n.Evaluate(new RumorScope(), bindings)
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
	}
}
