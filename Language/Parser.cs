using System;
using System.Collections.Generic;
using System.Text;
using Exodrifter.Rumor.Expressions;

namespace Exodrifter.Rumor.Language
{
	public class Parser
	{
		#region Properties

		/// <summary>
		/// A mapping of operators to precedence value (a lower value means
		/// higher precedence).
		/// </summary>
		private readonly Dictionary<string, int> ops;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new parser with the default operator precedence.
		/// </summary>
		public Parser()
		{
			this.ops = new Dictionary<string, int>();
			SetUpOpsHash(new List<string>() {
				".", "!",
				"*", "/", "+", "-",
				"<", ">", "<=", ">=",
				"and", "xor", "or",
				"==", "!=",
				"*=", "/=", "+=", "-=",
				"=",
			});
		}

		/// <summary>
		/// Creates a new parser.
		/// </summary>
		/// <param name="ops">
		/// The operators in order of most precedence to least precedence.
		/// </param>
		public Parser(List<string> ops)
		{
			this.ops = new Dictionary<string, int>();
			SetUpOpsHash(ops);
		}

		/// <summary>
		/// Sets up the operator precedence table.
		/// </summary>
		/// <param name="ops">
		/// The operators in order of most precedence to least precedence.
		/// </param>
		private void SetUpOpsHash(List<string> ops)
		{
			for (int i = 0; i < ops.Count; ++i)
			{
				this.ops[ops[i]] = i;
			}
		}

		#endregion

		#region Compile

		public void Compile(Reader reader)
		{
			while (!reader.EOF)
			{
				// Indentation level
				var depth = reader.Skip();
				var next = reader.Peek();

				// Ignore
				if (next == '\n')
				{
					reader.NextLine();
					continue;
				}
				else if (next == '#')
				{
					reader.NextLine();
					continue;
				}

				// Parse the command
				var errorLocation = new Reader(reader);
				var command = ParseCommand(reader);
				switch (command)
				{
					default:
						throw new UnknownCommandException(reader, command);
				}

				// Skip blank space after command
				reader.Skip();
				next = reader.Peek();

				// Ignore
				if (next == '#')
				{
					reader.NextLine();
					continue;
				}

				// Unused characters after command
				errorLocation = new Reader(reader);
				var unused = reader.ReadUntil('\n').Trim();
				if (!string.IsNullOrEmpty(unused))
				{
					throw new UnusedException(errorLocation, unused);
				}

				reader.NextLine();
			}
		}

		#endregion

		#region Parse

		private string ParseCommand(Reader reader)
		{
			var command = reader.ReadUntil(' ', '\t', '\n');
			reader.Skip();
			return command.Trim();
		}

		#endregion

		#region Bool

		public bool ParseBool(Reader reader)
		{
			if (reader.HasMatch("false"))
			{
				reader.Read("false".Length);
				return false;
			}
			else if (reader.HasMatch("true"))
			{
				reader.Read("true".Length);
				return true;
			}

			throw new InvalidBooleanException(reader);
		}

		#endregion

		#region Number

		public object ParseNumber(Reader reader)
		{
			// Sanity Check
			if (reader.EOF)
			{
				throw new ReadException(reader, "+-.1234567890".ToCharArray());
			}
			var errorLocation = new Reader(reader);
			var builder = new StringBuilder();

			// Parse the sign
			if (!reader.EOF)
			{
				var temp = reader.Peek();
				if (temp == '+' || temp == '-')
				{
					reader.Read();
					builder.Append(temp);
				}
			}

			// Parse numbers
			if (!reader.EOF)
			{
				var temp = reader.Peek();
				while ("1234567890".Contains(temp))
				{
					reader.Read();
					builder.Append(temp);

					if (reader.EOF)
					{
						break;
					}
					temp = reader.Peek();
				}
			}

			// Parse dot
			if (!reader.EOF)
			{
				var temp = reader.Peek();
				if (temp == '.')
				{
					reader.Read();
					builder.Append(temp);
				}
			}

			// Parse numbers
			if (!reader.EOF)
			{
				var temp = reader.Peek();
				while ("1234567890".Contains(temp))
				{
					reader.Read();
					builder.Append(temp);

					if (reader.EOF)
					{
						break;
					}
					temp = reader.Peek();
				}
			}

			var str = builder.ToString();

			int @int;
			if (int.TryParse(str, out @int))
			{
				return @int;
			}

			float @float;
			if (float.TryParse(str, out @float))
			{
				return @float;
			}

			throw new InvalidNumberException(errorLocation, builder);
		}

		#endregion

		#region String

		public string ParseString(Reader reader)
		{
			reader.Expect('\"');

			var whitespace = false;
			var builder = new StringBuilder();
			while (!reader.EOF)
			{
				builder.Append(reader.ReadUntil('\\', '"', ' ', '\t', '\n'));

				// Did not find end quote
				if (reader.EOF)
				{
					throw new ReadException(reader, '\"');
				}

				var ch = reader.Peek();
				switch (ch)
				{
					// Unknown string break sequence
					default:
						throw new ReadException(reader,
							'\\', '"', ' ', '\t', '\n');

					// Escaped character
					case '\\':
						builder.Append(ParseEscape(reader));
						whitespace = false;
						break;

					// End Quotes
					case '\"':
						reader.Read();
						return builder.ToString();

					// Whitespace
					case ' ':
					case '\t':
					case '\n':
						reader.Read();
						if (!whitespace)
						{
							builder.Append(' ');
							whitespace = true;
						}
						break;
				}
			}

			// Did not find end quote
			throw new ReadException(reader, '\"');
		}

		private char ParseEscape(Reader reader)
		{
			reader.Expect('\\');

			var errorLocation = new Reader(reader);
			var ch = reader.Read();
			switch (ch)
			{
				default:
					throw new ReadException(errorLocation, "escape sequence");

				// Escaped double quotes
				case '"':
					return '"';

				// Escaped backslash
				case '\\':
					return '\\';

				// Escaped newline
				case 'n':
					return '\n';

				// Escaped tab
				case 't':
					return '\t';

				// Escaped unicode
				case 'u':
					var builder = new StringBuilder();

					var count = 0;
					while (count < 4)
					{
						errorLocation = new Reader(reader);
						var u = reader.Read();
						if (!"0123456789abcdefABCDEF".Contains(u))
						{
							throw new ReadException(errorLocation,
								"0123456789abcdefABCDEF".ToCharArray());
						}

						builder.Append(u);
						count++;
					}

					return (char)Convert.ToInt32(builder.ToString(), 16);
			}
		}

		#endregion

		#region Variable

		private const string VALID_VAR_CHARS =
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890";

		public string ParseVariable(Reader reader)
		{
			var variable = new StringBuilder();
			while (!reader.EOF && VALID_VAR_CHARS.Contains(reader.Peek()))
			{
				variable.Append(reader.Read());
			}

			if (variable.Length == 0)
			{
				throw new ReadException(
					reader, VALID_VAR_CHARS.ToCharArray());
			}
			return variable.ToString();
		}

		#endregion

		#region Expression

		/// <summary>
		/// Tokenizes an expression.
		/// </summary>
		public List<Token> TokenizeExpression(Reader reader)
		{
			// Sanity Check
			if (reader.EOF)
			{
				throw new ReadException(reader);
			}

			Reader temp = null;
			var tokens = new List<Token>();
			var addOp = true;
			var addElement = true;
			var parenthesis = new List<Reader>();
			while (true)
			{
				// Catch up the reader
				if (temp != null)
				{
					reader.Read(temp.Index - reader.Index);
				}
				temp = new Reader(reader);

				// Skip whitespace
				temp.Skip();

				// Expression has ended
				if (temp.EOF)
				{
					break;
				}

				// Check for parenthesis
				var success = false;
				switch (temp.Peek())
				{
					case '(':
						tokens.Add(new Token(temp, 1));
						parenthesis.Add(new Reader(temp));
						temp.Read();

						addOp = true;
						addElement = true;
						success = true;
						break;

					case ')':
						if (parenthesis.Count == 0)
						{
							throw new CloseParenthesisException(temp);
						}

						tokens.Add(new Token(temp, 1));
						parenthesis.RemoveAt(parenthesis.Count - 1);
						temp.Read();

						addOp = true;
						addElement = false;
						success = true;
						break;

					case ',':
						if (parenthesis.Count == 0)
						{
							throw new CommaException(temp);
						}

						tokens.Add(new Token(temp, 1));
						temp.Read();

						addOp = true;
						addElement = true;
						success = true;
						break;
				}

				if (success)
				{
					continue;
				}

				// Check for operators
				if (addOp)
				{
					foreach (var op in ops.Keys)
					{
						if (temp.HasMatch(op))
						{
							tokens.Add(new Token(temp, op.Length));
							temp.Read(op.Length);

							addOp = false;
							addElement = true;
							success = true;
							break;
						}
					}
				}

				if (success)
				{
					continue;
				}

				// Check for expression elements
				if (addElement)
				{
					// String Literal
					Expression expression = null;
					var old = new Reader(temp);
					var next = temp.Peek();
					if ('"' == next)
					{
						var str = ParseString(temp);
						expression = new LiteralExpression(str);
					}
					// Number Literal
					else if ("1234567890+-.".Contains(next))
					{
						var num = ParseNumber(temp);
						expression = new LiteralExpression(num);
					}
					// Boolean Literal
					else if (temp.HasMatch("true") || temp.HasMatch("false"))
					{
						var b = ParseBool(temp);
						expression = new LiteralExpression(b);
					}
					// Variable
					else if (VALID_VAR_CHARS.Contains(next))
					{
						var name = ParseVariable(temp);
						expression = new VariableExpression(name);
					}

					if (expression != null)
					{
						var length = temp.Index - old.Index;
						tokens.Add(new Token(old, length, expression));
						addOp = true;
						addElement = false;
						success = true;
					}
				}

				// Stop if the expression has ended
				if (!success)
				{
					break;
				}
			}

			// Check parenthesis
			if (parenthesis.Count != 0)
			{
				var errorLocation = parenthesis[parenthesis.Count - 1];
				throw new OpenParenthesisException(errorLocation);
			}

			return tokens;
		}

		public Expression CompileExpression(Reader reader)
		{
			return CompileExpression(TokenizeExpression(reader));
		}

		public Expression CompileExpression(List<Token> tokens)
		{
			if (tokens == null || tokens.Count == 0)
			{
				return new NoOpExpression();
			}

			int opValue = int.MaxValue; // operator precedence
			int opIndex = -1; // operator index in the token list
			var parenthesis = new List<Token>();

			// Find an operator
			for (int i = 0; i < tokens.Count; ++i)
			{
				var token = tokens[i];

				if (parenthesis.Count == 0 && ops.ContainsKey(token.Text))
				{
					var newOpValue = ops[token.Text];

					// Token is an operator with a higher precedence
					if (newOpValue < opValue)
					{
						opValue = newOpValue;
						opIndex = i;
					}
				}
				else if (token.Text == "(")
				{
					parenthesis.Add(token);
				}
				else if (token.Text == ")")
				{
					if (parenthesis.Count == 0)
					{
						throw new CloseParenthesisException(token);
					}
					parenthesis.RemoveAt(parenthesis.Count - 1);
				}
			}

			if (parenthesis.Count != 0)
			{
				var lastParenthesis = parenthesis[parenthesis.Count - 1];
				throw new OpenParenthesisException(lastParenthesis);
			}

			// Split on the operator, if there is one
			if (opIndex != -1)
			{
				var opToken = tokens[opIndex];
				var left = CompileExpression(tokens.Slice(0, opIndex));
				var right = CompileExpression(tokens.Slice(opIndex + 1));
				switch (opToken.Text) {
					default:
						throw new UnknownOpException(opToken, opToken.Text);

					case ".":
						return new DotExpression(left, right);
					case "=":
						return new SetExpression(left, right);
					case "*=":
						return new SetMultiplyExpression(left, right);
					case "/=":
						return new SetDivideExpression(left, right);
					case "+=":
						return new SetAddExpression(left, right);
					case "-=":
						return new SetSubtractExpression(left, right);
					case "!":
						if (left is NoOpExpression) {
							return new NotExpression(right);
						}
						throw new ParseException(tokens[opIndex],
							"Not Operator cannot have a left hand argument");
					case "*":
						return new MultiplyExpression(left, right);
					case "/":
						return new DivideExpression(left, right);
					case "+":
						return new AddExpression(left, right);
					case "-":
						return new SubtractExpression(left, right);
					case "==":
						return new EqualsExpression(left, right);
					case "!=":
						return new NotEqualsExpression(left, right);
					case "<":
						return new LessThanExpression(left, right);
					case ">":
						return new GreaterThanExpression(left, right);
					case "<=":
						return new LessThanOrEqualExpression(left, right);
					case ">=":
						return new GreaterThanOrEqualExpression(left, right);
					case "and":
						return new BoolAndExpression(left, right);
					case "xor":
						return new BoolXorExpression(left, right);
					case "or":
						return new BoolOrExpression(left, right);
				}
			}

			// Drop parenthesis, if they exist
			var parenthesisAtStart = tokens[0].Text == "(";
			var parenthesisAtEnd = tokens[tokens.Count - 1].Text == ")";
			if (parenthesisAtStart && parenthesisAtEnd)
			{
				return CompileExpression(tokens.Slice(1, -1));
			}
			// Parse the literal or variable
			else if (tokens.Count == 1)
			{
				if (tokens[0].Expression != null)
				{
					return tokens[0].Expression;
				}

				throw new ParseException(tokens[0],
					"Expected a literal or variable");
			}
			// Parse a function call
			else if (parenthesisAtEnd)
			{
				if (tokens[1].Text != "(")
				{
					throw new ParseException(tokens[1], string.Format(
						"Expected open parenthesis, but got \"{0}\" instead",
						tokens[1].Text));
				}

				var @params = new List<Expression>();
				var pd = 0;
				var start = 2;
				var end = 3;
				for (; end < tokens.Count; ++end) {
					var tk = tokens[end];

					if (tk.Text == "(")
					{
						pd++;
					}
					else if (tk.Text == ")" && pd > 0)
					{
						pd--;
					}
					else if (pd == 0 && (tk.Text == "," || tk.Text == ")"))
					{
						var exp = CompileExpression(tokens.Slice(start, end));
						@params.Add(exp);
						start = end + 1;
					}
				}

				return new FunctionExpression(tokens[0].Text, @params);
			}

			throw new ParseException(tokens[0], "Could not parse expression");
		}

		#endregion
	}
}
