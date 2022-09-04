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
			var state = new ParserState(
				"if { 5 == 5 }\n  wait",
				4, new RumorParserState()
			);

			var script = Compiler.If(state);
			Assert.AreEqual(20, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new BooleanLiteral(true),
							"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
							null)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					}
				},
				script
			);
		}

		[Test]
		public static void IfElseSuccess()
		{
			var state = new ParserState(
				"if { 5 == 5 }\n  wait\nelse\n  wait",
				4, new RumorParserState()
			);

			var script = Compiler.If(state);
			Assert.AreEqual(32, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
								new BooleanLiteral(true),
								"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
								new ControlNode(
									null,
									"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
									null
								)
							)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
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
				"if { 5 == 5 }\n  wait\nelif {4 == 4}\n  wait\nelse\n  wait",
				4, new RumorParserState()
			);

			var script = Compiler.If(state);
			Assert.AreEqual(53, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
								new BooleanLiteral(true),
								"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
								new ControlNode(
									new BooleanLiteral(true),
									"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
									new ControlNode(
										null,
										"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
										null
									)
								)
							)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					},
				},
				script
			);
		}

		#endregion

		#region Function

		[Test]
		public static void IfFunction0Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar", ValueType.Number);

			var state = new ParserState(
				"if { foobar() == 5 }\n  wait",
				4, hints
			);

			var script = Compiler.If(state);
			Assert.AreEqual(27, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression(new NumberFunction("foobar"), new NumberLiteral(5)),
							"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
							null)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					}
				},
				script
			);
		}

		[Test]
		public static void IfFunction1Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar", ValueType.String, ValueType.Number);

			var state = new ParserState(
				"if { foobar(\"baz\") == 5 }\n  wait",
				4, hints
			);

			var script = Compiler.If(state);
			Assert.AreEqual(32, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression(new NumberFunction("foobar", new StringLiteral("baz")), new NumberLiteral(5)),
							"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
							null)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					}
				},
				script
			);
		}

		[Test]
		public static void IfFunction2Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.String,
				ValueType.String,
				ValueType.Number
			);

			var state = new ParserState(
				"if { foobar(\"a\", \"b\") == 5 }\n  wait",
				4, hints
			);

			var script = Compiler.If(state);
			Assert.AreEqual(35, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression(
								new NumberFunction(
									"foobar",
									new StringLiteral("a"),
									new StringLiteral("b")
								),
								new NumberLiteral(5)
							),
							"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
							null)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					}
				},
				script
			);
		}

		[Test]
		public static void IfFunction3Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.String,
				ValueType.String,
				ValueType.String,
				ValueType.Number
			);

			var state = new ParserState(
				"if { foobar(\"a\", \"b\", \"c\") == 5 }\n  wait",
				4, hints
			);

			var script = Compiler.If(state);
			Assert.AreEqual(40, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression(
								new NumberFunction(
									"foobar",
									new StringLiteral("a"),
									new StringLiteral("b"),
									new StringLiteral("c")
								),
								new NumberLiteral(5)
							),
							"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
							null)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					}
				},
				script
			);
		}

		[Test]
		public static void IfFunction4Success()
		{
			var hints = new RumorParserState();
			hints.LinkFunction("foobar",
				ValueType.String,
				ValueType.String,
				ValueType.String,
				ValueType.String,
				ValueType.Number
			);

			var state = new ParserState(
				"if { foobar(\"a\", \"b\", \"c\", \"d\") == 5 }\n  wait",
				4, hints
			);

			var script = Compiler.If(state);
			Assert.AreEqual(45, state.Index);
			Assert.AreEqual(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new ControlNode(
							new IsExpression(
								new NumberFunction(
									"foobar",
									new StringLiteral("a"),
									new StringLiteral("b"),
									new StringLiteral("c"),
									new StringLiteral("d")
								),
								new NumberLiteral(5)
							),
							"_tlifxqsNyCzxIJnRwtQKuZToQQw=",
							null)
						}
					},
					{ "_tlifxqsNyCzxIJnRwtQKuZToQQw=", new List<Node>()
						{
							new WaitNode()
						}
					}
				},
				script
			);
		}

		#endregion
	}
}
