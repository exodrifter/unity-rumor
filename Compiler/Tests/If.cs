using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Compiler.Tests
{
	using Rumor = Engine.Rumor;

	public static class If
	{
		#region If

		[Test]
		public static void IfSuccess()
		{
			var state = new ParserState("if { 5 == 5 }\n  wait", 4, 0);

			var script = Compiler.If(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression<NumberValue>(
								new NumberLiteral(5),
								new NumberLiteral(5)
							),
							new List<Node>()
							{
								new WaitNode()
							},
							null)
						}
					},
				},
				script
			);
		}

		[Test]
		public static void IfElseSuccess()
		{
			var state = new ParserState("if { 5 == 5 }\n  wait\nelse\n  wait", 4, 0);

			var script = Compiler.If(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression<NumberValue>(
								new NumberLiteral(5),
								new NumberLiteral(5)
							),
							new List<Node>()
							{
								new WaitNode()
							},
							new ControlNode(
								null,
								new List<Node>()
								{
									new WaitNode()
								},
								null
							))
						}
					},
				},
				script
			);
		}

		[Test]
		public static void IfElseElifSuccess()
		{
			var state = new ParserState(
				"if { 5 == 5 }\n  wait\nelif { 4 == 4 }\n  wait\nelse\n  wait",
				4, 0
			);

			var script = Compiler.If(state);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression<NumberValue>(
								new NumberLiteral(5),
								new NumberLiteral(5)
							),
							new List<Node>()
							{
								new WaitNode()
							},
							new ControlNode(
								new IsExpression<NumberValue>(
									new NumberLiteral(4),
									new NumberLiteral(4)
								),
								new List<Node>()
								{
									new WaitNode()
								},
								new ControlNode(
									null,
									new List<Node>()
									{
										new WaitNode()
									},
									null
								)
							))
						}
					},
				},
				script
			);
		}

		#endregion
	}
}
