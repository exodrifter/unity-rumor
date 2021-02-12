using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class SetVariable
	{
		[Test]
		public static void SetBooleanVariableLine()
		{
			var state = new ParserState(
				"foobar = { true }",
				4, new RumorParserState()
			);

			var node = Compiler.SetVariableLogic(state);
			Assert.AreEqual(
				new SetVariableNode(
					"foobar",
					new BooleanLiteral(true)
				),
				node
			);
		}
		[Test]
		public static void SetBooleanVariableMultiLine()
		{
			var state = new ParserState(
				"foobar = { true\n and false }",
				4, new RumorParserState()
			);

			var node = Compiler.SetVariableLogic(state);
			Assert.AreEqual(
				new SetVariableNode(
					"foobar",
					new BooleanLiteral(false)
				),
				node
			);
		}

		[Test]
		public static void SetNumberVariableLine()
		{
			var state = new ParserState(
				"foobar = { 3 }", 4,
				new RumorParserState()
			);

			var node = Compiler.SetVariableMath(state);
			Assert.AreEqual(
				new SetVariableNode(
					"foobar",
					new NumberLiteral(3)
				),
				node
			);
		}

		[Test]
		public static void SetNumberVariableMultiLine()
		{
			var state = new ParserState(
				"foobar = { 3\n + 4 }",
				4, new RumorParserState()
			);

			var node = Compiler.SetVariableMath(state);
			Assert.AreEqual(
				new SetVariableNode(
					"foobar",
					new NumberLiteral(7)
				),
				node
			);
		}

		[Test]
		public static void SetStringVariableLine()
		{
			var state = new ParserState(
				"foobar = { \"Hello world!\" }",
				4, new RumorParserState()
			);

			var node = Compiler.SetVariableText(state);
			Assert.AreEqual(
				new SetVariableNode(
					"foobar",
					new StringLiteral("Hello world!")
				),
				node
			);
		}

		[Test]
		public static void SetStringVariableMultiLine()
		{
			var state = new ParserState(
				"foobar = { \"Hello \n world!\" }",
				4, new RumorParserState()
			);

			var node = Compiler.SetVariableText(state);
			Assert.AreEqual(
				new SetVariableNode(
					"foobar",
					new StringLiteral("Hello \n world!")
				),
				node
			);
		}
	}
}
