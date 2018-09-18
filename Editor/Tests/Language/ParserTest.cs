#if UNITY_EDITOR

using NUnit.Framework;
using Exodrifter.Rumor.Language;
using Exodrifter.Rumor.Expressions;

namespace Exodrifter.Rumor.Test.Lang
{
	internal sealed class ParserTest
	{
		#region Boolean

		/// <summary>
		/// Check if simple boolean parsing works.
		/// </summary>
		[Test]
		public void ParseBoolean()
		{
			var parser = new Parser();

			var reader = new Reader("true");
			var boolean = parser.ParseBool(reader);
			Assert.True(boolean);

			reader = new Reader("TRUE");
			boolean = parser.ParseBool(reader);
			Assert.True(boolean);

			reader = new Reader("tRuE");
			boolean = parser.ParseBool(reader);
			Assert.True(boolean);

			reader = new Reader("false");
			boolean = parser.ParseBool(reader);
			Assert.False(boolean);

			reader = new Reader("FALSE");
			boolean = parser.ParseBool(reader);
			Assert.False(boolean);

			reader = new Reader("fAlSe");
			boolean = parser.ParseBool(reader);
			Assert.False(boolean);
		}

		#endregion

		#region Number

		/// <summary>
		/// Check if simple float parsing works.
		/// </summary>
		[Test]
		public void ParseFloat()
		{
			var parser = new Parser();

			// Normal
			var reader = new Reader("1.23");
			var number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(1.23f, number);

			reader = new Reader(".23");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(0.23f, number);

			reader = new Reader("1.");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(1f, number);

			// Positive sign
			reader = new Reader("+1.23");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(1.23f, number);

			reader = new Reader("+.23");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(0.23f, number);

			reader = new Reader("+1.");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(1f, number);

			// Negative sign
			reader = new Reader("-1.23");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(-1.23f, number);

			reader = new Reader("-.23");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(-0.23f, number);

			reader = new Reader("-1.");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(-1f, number);
		}

		/// <summary>
		/// Check if simple int parsing works.
		/// </summary>
		[Test]
		public void ParseInt()
		{
			var parser = new Parser();

			// Normal
			var reader = new Reader("1");
			var number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(1, number);

			// Positive sign
			reader = new Reader("+1");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(1, number);

			// Negative sign
			reader = new Reader("-1");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(-1, number);
		}

		/// <summary>
		/// Check that potential invalid numbers throws the correct exception.
		/// </summary>
		[Test]
		public void InvalidNumber()
		{
			var parser = new Parser();

			// Normal
			var reader = new Reader(".");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader("+");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader("-");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader("+.");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader("+-");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader("-.");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader("-+");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader(".+");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);

			reader = new Reader(".-");
			Assert.Throws<InvalidNumberException>(
				() => parser.ParseNumber(reader)
			);
		}

		/// <summary>
		/// Check if number parsing works with trailing characters.
		/// </summary>
		[Test]
		public void ParseNumberExtra()
		{
			var parser = new Parser();

			var reader = new Reader("12.3+");
			var number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(12.3f, number);

			reader = new Reader("12.3-");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(float), number);
			Assert.AreEqual(12.3f, number);

			reader = new Reader("12+");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(12f, number);

			reader = new Reader("12-");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(12f, number);

			reader = new Reader("12f");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(12f, number);

			reader = new Reader("12f");
			number = parser.ParseNumber(reader);
			Assert.IsAssignableFrom(typeof(int), number);
			Assert.AreEqual(12f, number);
		}

		#endregion

		#region String

		/// <summary>
		/// Check if simple string parsing works.
		/// </summary>
		[Test]
		public void ParseString()
		{
			var parser = new Parser();

			var reader = new Reader("\"Hello world\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader(" \"Hello world\"");
			Assert.Throws<ReadException>(() => parser.ParseString(reader));
		}

		/// <summary>
		/// Check if string escapes work.
		/// </summary>
		[Test]
		public void ParseStringEscapes()
		{
			var parser = new Parser();

			var reader = new Reader("\"Hello \\\\ world\"");
			AssertString("Hello \\ world", parser.ParseString(reader));

			reader = new Reader("\"Hello \\\" world\"");
			AssertString("Hello \" world", parser.ParseString(reader));

			reader = new Reader("\"Hello \\t world\"");
			AssertString("Hello \t world", parser.ParseString(reader));

			reader = new Reader("\"Hello \\n world\"");
			AssertString("Hello \n world", parser.ParseString(reader));

			reader = new Reader("\"\\{Hello world}\"");
			AssertString("{Hello world}", parser.ParseString(reader));
		}

		/// <summary>
		/// Check if unicode strings work.
		/// </summary>
		[Test]
		public void ParseUnicodeString()
		{
			var parser = new Parser();

			// Uppercase
			var reader = new Reader("\"\\u4F60\\u597D\"");
			AssertString("\u4F60\u597D", parser.ParseString(reader));

			// Lowercase
			reader = new Reader("\"\\u4f60\\u597d\"");
			AssertString("\u4F60\u597D", parser.ParseString(reader));
		}

		/// <summary>
		/// Check if whitespace in strings is interpreted correctly.
		/// </summary>
		[Test]
		public void ParseStringWhitespace()
		{
			var parser = new Parser();

			var reader = new Reader("\"Hello world\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello\tworld\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello\nworld\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello\n\n\nworld\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello\n\t\nworld\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello \t \n \t world\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\" Hello world\"");
			AssertString(" Hello world", parser.ParseString(reader));

			reader = new Reader("\" \tHello world\"");
			AssertString(" Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello world \"");
			AssertString("Hello world ", parser.ParseString(reader));

			reader = new Reader("\"Hello world \t\"");
			AssertString("Hello world ", parser.ParseString(reader));
		}

		/// <summary>
		/// Check if string parsing works with trailing characters.
		/// </summary>
		[Test]
		public void ParseStringExtra()
		{
			var parser = new Parser();

			var reader = new Reader("\"Hello world\"abcd");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello world\"\"");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello world\" ");
			AssertString("Hello world", parser.ParseString(reader));

			reader = new Reader("\"Hello world\"\t\t ");
			AssertString("Hello world", parser.ParseString(reader));
		}

		/// <summary>
		/// Utility method for asserting the value of an expression that is
		/// just a literal string value
		/// </summary>
		private void AssertString(string expected, Expression actual)
		{
			Assert.IsAssignableFrom<LiteralExpression>(actual);
			var value = ((LiteralExpression)actual).Value;
			Assert.IsTrue(value.IsString());
			Assert.AreEqual(expected, value.AsString());
		}

		#endregion

		#region Variable

		/// <summary>
		/// Check if simple boolean parsing works.
		/// </summary>
		[Test]
		public void ParseVariable()
		{
			var parser = new Parser();

			var reader = new Reader("abcdefghijklmnopqrstuvwxyz");
			var name = parser.ParseVariable(reader);
			Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", name);

			reader = new Reader("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", name);

			reader = new Reader("1234567890");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("1234567890", name);

			reader = new Reader("foobar");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foobar", name);

			reader = new Reader("foo_bar");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foo_bar", name);

			reader = new Reader("foo_bar_2");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foo_bar_2", name);
		}

		/// <summary>
		/// Check if variable parsing works with trailing characters.
		/// </summary>
		[Test]
		public void ParseVariableExtra()
		{
			var parser = new Parser();

			var reader = new Reader("foobar+");
			var name = parser.ParseVariable(reader);
			Assert.AreEqual("foobar", name);

			reader = new Reader("foobar123.4");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foobar123", name);

			reader = new Reader("foo_bar_ ");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foo_bar_", name);

			reader = new Reader("foo_bar_\n");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foo_bar_", name);

			reader = new Reader("foo_bar_\t");
			name = parser.ParseVariable(reader);
			Assert.AreEqual("foo_bar_", name);
		}

		#endregion

		#region Expression Tokenizer

		/// <summary>
		/// Check if simple expression tokenization works.
		/// </summary>
		[Test]
		public void TokenizeExpression()
		{
			var parser = new Parser();

			var tokens = parser.TokenizeExpression(new Reader("4 + 3"));
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("4", tokens[0].Text);
			Assert.AreEqual("+", tokens[1].Text);
			Assert.AreEqual("3", tokens[2].Text);

			tokens = parser.TokenizeExpression(new Reader("+4 * +3"));
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("+4", tokens[0].Text);
			Assert.AreEqual("*", tokens[1].Text);
			Assert.AreEqual("+3", tokens[2].Text);

			tokens = parser.TokenizeExpression(new Reader("!foo"));
			Assert.AreEqual(2, tokens.Count);
			Assert.AreEqual("!", tokens[0].Text);
			Assert.AreEqual("foo", tokens[1].Text);

			tokens = parser.TokenizeExpression(new Reader("foo.bar()"));
			Assert.AreEqual(5, tokens.Count);
			Assert.AreEqual("foo", tokens[0].Text);
			Assert.AreEqual(".", tokens[1].Text);
			Assert.AreEqual("bar", tokens[2].Text);
			Assert.AreEqual("(", tokens[3].Text);
			Assert.AreEqual(")", tokens[4].Text);

			tokens = parser.TokenizeExpression(new Reader("foo.bar(5, 4)"));
			Assert.AreEqual(8, tokens.Count);
			Assert.AreEqual("foo", tokens[0].Text);
			Assert.AreEqual(".", tokens[1].Text);
			Assert.AreEqual("bar", tokens[2].Text);
			Assert.AreEqual("(", tokens[3].Text);
			Assert.AreEqual("5", tokens[4].Text);
			Assert.AreEqual(",", tokens[5].Text);
			Assert.AreEqual("4", tokens[6].Text);
			Assert.AreEqual(")", tokens[7].Text);
		}

		/// <summary>
		/// Check that potential invalid parenthesis matching throws the correct
		/// exception.
		/// </summary>
		[Test]
		public void InvalidParenthesis()
		{
			var parser = new Parser();

			// Normal
			var reader = new Reader("(4 + 3");
			Assert.Throws<OpenParenthesisException>(
				() => parser.TokenizeExpression(reader)
			);

			reader = new Reader("4 + 3)");
			Assert.Throws<CloseParenthesisException>(
				() => parser.TokenizeExpression(reader)
			);

			reader = new Reader("4 + 3,");
			Assert.Throws<CommaException>(
				() => parser.TokenizeExpression(reader)
			);
		}

		/// <summary>
		/// Check if expression tokenization works with trailing characters.
		/// </summary>
		[Test]
		public void TokenizeExpressionExtra()
		{
			var parser = new Parser();

			var reader = new Reader("4 + 3 4 + 3");
			var tokens = parser.TokenizeExpression(reader);
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("4", tokens[0].Text);
			Assert.AreEqual("+", tokens[1].Text);
			Assert.AreEqual("3", tokens[2].Text);

			reader = new Reader("4 + 3 foo.bar()");
			tokens = parser.TokenizeExpression(reader);
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("4", tokens[0].Text);
			Assert.AreEqual("+", tokens[1].Text);
			Assert.AreEqual("3", tokens[2].Text);

			reader = new Reader("foo.bar() foo.bar()");
			tokens = parser.TokenizeExpression(reader);
			Assert.AreEqual(5, tokens.Count);
			Assert.AreEqual("foo", tokens[0].Text);
			Assert.AreEqual(".", tokens[1].Text);
			Assert.AreEqual("bar", tokens[2].Text);
			Assert.AreEqual("(", tokens[3].Text);
			Assert.AreEqual(")", tokens[4].Text);

			reader = new Reader("foo.bar() 4 + 3");
			tokens = parser.TokenizeExpression(reader);
			Assert.AreEqual(5, tokens.Count);
			Assert.AreEqual("foo", tokens[0].Text);
			Assert.AreEqual(".", tokens[1].Text);
			Assert.AreEqual("bar", tokens[2].Text);
			Assert.AreEqual("(", tokens[3].Text);
			Assert.AreEqual(")", tokens[4].Text);
		}

		/// <summary>
		/// Check if expression tokenization works across newlines.
		/// </summary>
		[Test]
		public void TokenizeExpressionNewline()
		{
			var parser = new Parser();

			var reader = new Reader("4\n+\n3");
			var tokens = parser.TokenizeExpression(reader);
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("4", tokens[0].Text);
			Assert.AreEqual("+", tokens[1].Text);
			Assert.AreEqual("3", tokens[2].Text);

			reader = new Reader("4\n+\n3\n5 + 2");
			tokens = parser.TokenizeExpression(reader);
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("4", tokens[0].Text);
			Assert.AreEqual("+", tokens[1].Text);
			Assert.AreEqual("3", tokens[2].Text);
		}

		#endregion

		#region Expression Compilation

		/// <summary>
		/// Check if simple expression compilation works.
		/// </summary>
		[Test]
		public void CompileExpression()
		{
			var parser = new Parser();

			var exp = parser.CompileExpression("4 * 3.0");
			Assert.IsAssignableFrom<MultiplyExpression>(exp);
			Assert.IsAssignableFrom<LiteralExpression>(
				(exp as OpExpression).Left);
			Assert.IsAssignableFrom<LiteralExpression>(
				(exp as OpExpression).Right);

			exp = parser.CompileExpression("foo.bar");
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>(
				(exp as OpExpression).Left);
			Assert.IsAssignableFrom<VariableExpression>(
				(exp as OpExpression).Right);

			exp = parser.CompileExpression("foo.bar()");
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>(
				(exp as OpExpression).Left);
			Assert.IsAssignableFrom<FunctionExpression>(
				(exp as OpExpression).Right);
		}

		/// <summary>
		/// Check if expression compilation with trailing characters.
		/// </summary>
		[Test]
		public void CompileExpressionExtra()
		{
			var parser = new Parser();

			var exp = parser.CompileExpression("4 * 3.0 foobar");
			Assert.IsAssignableFrom<MultiplyExpression>(exp);
			Assert.IsAssignableFrom<LiteralExpression>(
				(exp as OpExpression).Left);
			Assert.IsAssignableFrom<LiteralExpression>(
				(exp as OpExpression).Right);

			exp = parser.CompileExpression("foo.bar 4");
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>(
				(exp as OpExpression).Left);
			Assert.IsAssignableFrom<VariableExpression>(
				(exp as OpExpression).Right);

			exp = parser.CompileExpression("foo.bar() 3.0");
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>(
				(exp as OpExpression).Left);
			Assert.IsAssignableFrom<FunctionExpression>(
				(exp as OpExpression).Right);
		}

		/// <summary>
		/// Check if expression compilation with floats works.
		/// </summary>
		[Test]
		public void CompileExpressionFloat()
		{
			var parser = new Parser();

			var exp = parser.CompileExpression("foo(.6)");
			Assert.IsAssignableFrom<FunctionExpression>(exp);
			Assert.AreEqual("foo", (exp as FunctionExpression).Name);

			var @params = (exp as FunctionExpression).Params;
			Assert.AreEqual(1, @params.Count);
			Assert.IsAssignableFrom<LiteralExpression>(@params[0]);
			Assert.IsTrue((@params[0] as LiteralExpression).Value.IsFloat());
			Assert.AreEqual(0.6f, (@params[0] as LiteralExpression).Value.AsFloat());

			exp = parser.CompileExpression("foo(.06)");
			Assert.IsAssignableFrom<FunctionExpression>(exp);
			Assert.AreEqual("foo", (exp as FunctionExpression).Name);

			@params = (exp as FunctionExpression).Params;
			Assert.AreEqual(1, @params.Count);
			Assert.IsAssignableFrom<LiteralExpression>(@params[0]);
			Assert.IsTrue((@params[0] as LiteralExpression).Value.IsFloat());
			Assert.AreEqual(0.06f, (@params[0] as LiteralExpression).Value.AsFloat());
		}

		#endregion
	}
}

#endif
