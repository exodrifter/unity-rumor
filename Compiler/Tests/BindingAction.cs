using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class BindingAction
	{
		[Test]
		public static void BindingAction0Success()
		{
			var userState = new RumorParserState();
			userState.LinkAction("foobar");

			var state = new ParserState(
				"foobar()", 4, userState
			);

			var result = Compiler.BindingAction()(state);
			Assert.AreEqual(new BindingActionNode("foobar"), result);
		}

		[Test]
		public static void BindingAction1Success()
		{
			var userState = new RumorParserState();
			userState.LinkAction("foobar",
				ValueType.Boolean
			);

			var state = new ParserState(
				"foobar(true or false)", 4, userState
			);

			var result = Compiler.BindingAction()(state);
			Assert.AreEqual(
				new BindingActionNode("foobar",
					new BooleanLiteral(true)
				),
				result
			);
		}

		[Test]
		public static void BindingAction2Success()
		{
			var userState = new RumorParserState();
			userState.LinkAction("foobar",
				ValueType.Boolean,
				ValueType.Number
			);

			var state = new ParserState(
				"foobar(true or false, 4 + 4)", 4, userState
			);

			var result = Compiler.BindingAction()(state);
			Assert.AreEqual(
				new BindingActionNode("foobar",
					new BooleanLiteral(true),
					new NumberLiteral(8)
				),
				result
			);
		}

		[Test]
		public static void BindingAction3Success()
		{
			var userState = new RumorParserState();
			userState.LinkAction("foobar",
				ValueType.Boolean,
				ValueType.Number,
				ValueType.String
			);

			var state = new ParserState(
				"foobar(true or false, 4 + 4, \"hello\")", 4, userState
			);

			var result = Compiler.BindingAction()(state);
			Assert.AreEqual(
				new BindingActionNode("foobar",
					new BooleanLiteral(true),
					new NumberLiteral(8),
					new StringLiteral("hello")
				),
				result
			);
		}

		[Test]
		public static void BindingAction4Success()
		{
			var userState = new RumorParserState();
			userState.LinkAction("foobar",
				ValueType.Boolean,
				ValueType.Number,
				ValueType.String,
				ValueType.String
			);

			var state = new ParserState(
				"foobar(true or false, 4 + 4, \"hello\", \"world\")",
				4, userState
			);

			var result = Compiler.BindingAction()(state);
			Assert.AreEqual(
				new BindingActionNode("foobar",
					new BooleanLiteral(true),
					new NumberLiteral(8),
					new StringLiteral("hello"),
					new StringLiteral("world")
				),
				result
			);
		}

		[Test]
		public static void BindingActionOverloadedSuccess()
		{
			var userState = new RumorParserState();
			userState.LinkAction("foobar",
				ValueType.Boolean
			);
			userState.LinkAction("foobar",
				ValueType.Boolean,
				ValueType.Number
			);
			userState.LinkAction("foobar",
				ValueType.Boolean,
				ValueType.Number,
				ValueType.String
			);
			userState.LinkAction("foobar",
				ValueType.Boolean,
				ValueType.Number,
				ValueType.String,
				ValueType.String
			);

			var state = new ParserState(
				"foobar(true or false, 4 + 4, \"hello\")",
				4, userState
			);

			var result = Compiler.BindingAction()(state);
			Assert.AreEqual(
				new BindingActionNode("foobar",
					new BooleanLiteral(true),
					new NumberLiteral(8),
					new StringLiteral("hello")
				),
				result
			);
		}
	}
}
