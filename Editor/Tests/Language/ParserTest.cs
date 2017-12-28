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

		/// <summary>
		/// Check if boolean parsing works with trailing characters.
		/// </summary>
		[Test]
		public void ParseBooleanExtra()
		{
			var parser = new Parser();

			var reader = new Reader("truefoobar");
			var boolean = parser.ParseBool(reader);
			Assert.True(boolean);

			reader = new Reader("falsefoobar");
			boolean = parser.ParseBool(reader);
			Assert.False(boolean);

			reader = new Reader("truefalse");
			boolean = parser.ParseBool(reader);
			Assert.True(boolean);

			reader = new Reader("falsetrue");
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
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

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
			Assert.AreEqual(parser.ParseString(reader), "Hello \\ world");

			reader = new Reader("\"Hello \\\" world\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello \" world");

			reader = new Reader("\"Hello \\t world\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello \t world");

			reader = new Reader("\"Hello \\n world\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello \n world");
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
			Assert.AreEqual(parser.ParseString(reader), "\u4F60\u597D");

			// Lowercase
			reader = new Reader("\"\\u4f60\\u597d\"");
			Assert.AreEqual(parser.ParseString(reader), "\u4F60\u597D");
		}

		/// <summary>
		/// Check if whitespace in strings is interpreted correctly.
		/// </summary>
		[Test]
		public void ParseStringWhitespace()
		{
			var parser = new Parser();

			var reader = new Reader("\"Hello world\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello\tworld\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello\nworld\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello\n\n\nworld\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello\n\t\nworld\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello \t \n \t world\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");
		}

		/// <summary>
		/// Check if string parsing works with trailing characters.
		/// </summary>
		[Test]
		public void ParseStringExtra()
		{
			var parser = new Parser();

			var reader = new Reader("\"Hello world\"abcd");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello world\"\"");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello world\" ");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");

			reader = new Reader("\"Hello world\"\t\t ");
			Assert.AreEqual(parser.ParseString(reader), "Hello world");
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
			Assert.AreEqual(4, tokens.Count);
			Assert.AreEqual("+", tokens[0].Text);
			Assert.AreEqual("4", tokens[1].Text);
			Assert.AreEqual("*", tokens[2].Text);
			Assert.AreEqual("+3", tokens[3].Text);

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

		#endregion

		#region Expression Compilation

		/// <summary>
		/// Check if simple expression compilation works.
		/// </summary>
		[Test]
		public void CompileExpression()
		{
			var parser = new Parser();

			var reader = new Reader("4 * 3.0");
			var exp = parser.CompileExpression(reader);
			Assert.IsAssignableFrom<MultiplyExpression>(exp);
			Assert.IsAssignableFrom<LiteralExpression>((exp as OpExpression).Left);
			Assert.IsAssignableFrom<LiteralExpression>((exp as OpExpression).Right);

			reader = new Reader("foo.bar");
			exp = parser.CompileExpression(reader);
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>((exp as OpExpression).Left);
			Assert.IsAssignableFrom<VariableExpression>((exp as OpExpression).Right);

			reader = new Reader("foo.bar()");
			exp = parser.CompileExpression(reader);
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>((exp as OpExpression).Left);
			Assert.IsAssignableFrom<FunctionExpression>((exp as OpExpression).Right);
		}

		/// <summary>
		/// Check if expression compilation with trailing characters.
		/// </summary>
		[Test]
		public void CompileExpressionExtra()
		{
			var parser = new Parser();

			var reader = new Reader("4 * 3.0 foobar");
			var exp = parser.CompileExpression(reader);
			Assert.IsAssignableFrom<MultiplyExpression>(exp);
			Assert.IsAssignableFrom<LiteralExpression>((exp as OpExpression).Left);
			Assert.IsAssignableFrom<LiteralExpression>((exp as OpExpression).Right);

			reader = new Reader("foo.bar 4");
			exp = parser.CompileExpression(reader);
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>((exp as OpExpression).Left);
			Assert.IsAssignableFrom<VariableExpression>((exp as OpExpression).Right);

			reader = new Reader("foo.bar() 3.0");
			exp = parser.CompileExpression(reader);
			Assert.IsAssignableFrom<DotExpression>(exp);
			Assert.IsAssignableFrom<VariableExpression>((exp as OpExpression).Left);
			Assert.IsAssignableFrom<FunctionExpression>((exp as OpExpression).Right);
		}

		#endregion
	}
}

#endif
