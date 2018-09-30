using System;
using System.Collections.Generic;
using System.Text;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Nodes;

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
				"==", "!=",
				"and", "xor", "or",
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

		public List<Node> Compile(Reader reader, int? targetDepth = null)
		{
			var nodes = new List<Node>();
			var temp = new Reader(reader);

			while (!temp.EOF)
			{
				// Get the depth of the next line
				var depth = ReadNextDepth(temp);
				if (depth == -1)
				{
					break;
				}

				// Validate block
				targetDepth = targetDepth ?? depth;
				if (depth < targetDepth)
				{
					break; // Block has ended
				}
				if (depth > targetDepth)
				{
					throw new ParseException(temp, "Unexpected block: '" + temp.ReadUntil('\n').Trim() + "'");
				}

				// Parse the command
				string command;
				if (temp.Peek() == '$')
				{
					command = temp.Read().ToString();
				}
				else
				{
					command = temp.ReadUntil(' ', '\t', '\n').Trim();
				}
				temp.Skip();
				switch (command)
				{
					default:
						throw new UnknownCommandException(temp, command);

					case "$":
						nodes.Add(CompileStatement(temp));
						break;

					case "add":
						nodes.Add(CompileAdd(temp));
						break;

					case "call":
						nodes.Add(CompileCall(temp));
						break;

					case "choice":
						nodes.Add(CompileChoice(temp, depth));
						break;

					case "choose":
						nodes.Add(CompileChoose(temp));
						break;

					case "clear":
						nodes.Add(CompileClear(temp));
						break;

					case "if":
						nodes.Add(CompileIf(temp, depth));
						break;

					case "jump":
						nodes.Add(CompileJump(temp));
						break;

					case "label":
						nodes.Add(CompileLabel(temp, depth));
						break;

					case "pause":
						nodes.Add(CompilePause(temp));
						break;

					case "return":
						nodes.Add(CompileReturn(temp));
						break;

					case "say":
						nodes.Add(CompileSay(temp));
						break;
				}

				// Catch up reader
				reader.Update(temp);
				temp = GetTempOnNextLine(reader);
			}

			return nodes;
		}

		private List<Node> CompileChildren(Reader reader, int currentDepth)
		{
			var temp = GetTempOnNextLine(reader);

			// Check the depth
			var nextDepth = ReadNextDepth(temp);
			if (nextDepth > currentDepth)
			{
				return Compile(reader);
			}

			// No children
			return new List<Node>();
		}

		private Reader GetTempOnNextLine(Reader reader)
		{
			var temp = new Reader(reader);

			// Skip blank space after command
			temp.Skip();

			// Ignore
			if (!temp.EOF && temp.Peek() == '#')
			{
				temp.NextLine();
			}
			else
			{
				// Check for unused characters after command
				var errorLocation = new Reader(temp);
				var unused = temp.ReadUntil('\n').Trim();
				if (!string.IsNullOrEmpty(unused))
				{
					throw new UnusedException(errorLocation, unused);
				}

				temp.NextLine();
			}

			return temp;
		}

		private Reader GetTempOnNextNonEmptyLine(Reader reader)
		{
			var temp = GetTempOnNextLine(reader);
			while (!temp.EOF)
			{
				// Check if the line is not blank
				try
				{
					GetTempOnNextLine(temp);
				}
				catch (UnusedException)
				{
					return temp;
				}

				// Keep reading
				temp.NextLine();
			}

			return temp;
		}

		/// <summary>
		/// Advances the reader to the next line and returns the depth of the
		/// reader.
		/// </summary>
		/// <param name="reader">The reader to use.</param>
		/// <returns>
		/// The depth of the next line or -1 if there is no next line.
		/// </returns>
		private static int ReadNextDepth(Reader reader)
		{
			while (!reader.EOF)
			{
				// Indentation level
				var depth = reader.Skip();
				if (reader.EOF)
				{
					break;
				}

				// Ignore
				var next = reader.Peek();
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

				return depth;
			}

			return -1;
		}

		#endregion

		#region Compile Commands

		private Add CompileAdd(Reader reader)
		{
			var first = CompileExpression(reader);
			var second = CompileExpression(reader);

			// Check for no_wait as second parameter
			bool noWait = false;
			if (second is VariableExpression)
			{
				if ((second as VariableExpression).Name == "no_wait")
				{
					second = new NoOpExpression();
					noWait = true;
				}
			}

			// Check for no_wait at end of line
			if (!noWait)
			{
				reader.Skip();
				if (reader.HasToken("no_wait"))
				{
					reader.Read("no_wait".Length);
					noWait = true;
				}
			}

			if (second is NoOpExpression)
			{
				return new Add(first, noWait);
			}
			else
			{
				return new Add(first, second, noWait);
			}
		}

		private Call CompileCall(Reader reader)
		{
			var name = ParseVariable(reader);
			return new Call(name);
		}

		private Choice CompileChoice(Reader reader, int depth)
		{
			var text = CompileExpression(reader);
			reader.Skip();
			reader.Expect(':');
			return new Choice(text, CompileChildren(reader, depth));
		}

		private Choose CompileChoose(Reader reader)
		{
			var number = CompileExpression(reader);
			if (number is NoOpExpression)
			{
				number = new LiteralExpression(1);
			}

			Expression seconds = new LiteralExpression(0);
			reader.Skip();
			if (reader.HasToken("in"))
			{
				reader.Read("in".Length);
				seconds = CompileExpression(reader);
			}

			Expression @default = new LiteralExpression(0);
			reader.Skip();
			if (reader.HasToken("default"))
			{
				reader.Read("default".Length);
				@default = CompileExpression(reader);
			}

			return new Choose(number, seconds, @default);
		}

		private Clear CompileClear(Reader reader)
		{
			var type = ClearType.ALL;
			reader.Skip();

			if (reader.HasToken("choices"))
			{
				reader.Read("choices".Length);
				type = ClearType.CHOICES;
			}
			else if (reader.HasToken("dialog"))
			{
				reader.Read("dialog".Length);
				type = ClearType.DIALOG;
			}

			return new Clear(type);
		}

		private Condition CompileIf(Reader reader, int depth)
		{
			var exp = CompileExpression(reader);
			reader.Skip();
			reader.Expect(':');
			var children = CompileChildren(reader, depth);

			var temp = GetTempOnNextNonEmptyLine(reader);
			var nextDepth = temp.Skip();

			if (depth == nextDepth && temp.HasToken("elif"))
			{
				temp.Read("elif".Length);
				temp.Skip();

				// Catch up reader
				reader.Update(temp);

				var elif = CompileElif(reader, depth);
				return new Condition(new If(exp, children, elif));
			}
			else if (depth == nextDepth && temp.HasToken("else"))
			{
				temp.Read("else".Length);
				temp.Skip();

				// Catch up reader
				reader.Update(temp);

				var @else = CompileElse(reader, depth);
				return new Condition(new If(exp, children, @else));
			}

			return new Condition(new If(exp, children));
		}

		private Elif CompileElif(Reader reader, int depth)
		{
			var exp = CompileExpression(reader);
			reader.Skip();
			reader.Expect(':');
			var children = CompileChildren(reader, depth);

			var temp = GetTempOnNextNonEmptyLine(reader);
			var nextDepth = temp.Skip();

			if (depth == nextDepth && temp.HasToken("elif"))
			{
				temp.Read("elif".Length);
				temp.Skip();

				// Catch up reader
				reader.Update(temp);

				var elif = CompileElif(reader, depth);
				return new Elif(exp, children, elif);
			}
			else if (depth == nextDepth && temp.HasToken("else"))
			{
				temp.Read("else".Length);
				temp.Skip();

				// Catch up reader
				reader.Update(temp);

				var @else = CompileElse(reader, depth);
				return new Elif(exp, children, @else);
			}

			return new Elif(exp, children);
		}

		private Else CompileElse(Reader reader, int depth)
		{
			reader.Skip();
			reader.Expect(':');
			var children = CompileChildren(reader, depth);
			return new Else(children);
		}

		private Jump CompileJump(Reader reader)
		{
			var name = ParseVariable(reader);
			return new Jump(name);
		}

		private Label CompileLabel(Reader reader, int depth)
		{
			var name = ParseVariable(reader);
			reader.Skip();
			reader.Expect(':');
			return new Label(name, CompileChildren(reader, depth));
		}

		private Pause CompilePause(Reader reader)
		{
			var exp = CompileExpression(reader);

			// Check for cant_skip argument
			bool cantSkip = false;
			reader.Skip();
			if (reader.HasToken("cant_skip"))
			{
				reader.Read("cant_skip".Length);
				cantSkip = true;
			}

			return new Pause(exp, cantSkip);
		}

		private Return CompileReturn(Reader reader)
		{
			return new Return();
		}

		private Say CompileSay(Reader reader)
		{
			var first = CompileExpression(reader);
			var second = CompileExpression(reader);

			// Check for no_wait as second parameter
			bool noWait = false;
			if (second is VariableExpression)
			{
				if ((second as VariableExpression).Name == "no_wait")
				{
					second = new NoOpExpression();
					noWait = true;
				}
			}

			// Check for no_wait at end of line
			if (!noWait)
			{
				reader.Skip();
				if (reader.HasToken("no_wait"))
				{
					reader.Read("no_wait".Length);
					noWait = true;
				}
			}

			if (second is NoOpExpression)
			{
				return new Say(first, noWait);
			}
			else
			{
				return new Say(first, second, noWait);
			}
		}

		private Statement CompileStatement(Reader reader)
		{
			return new Statement(CompileExpression(reader));
		}

		#endregion

		#region Bool

		public bool ParseBool(Reader reader)
		{
			if (reader.HasToken("false"))
			{
				reader.Read("false".Length);
				return false;
			}
			else if (reader.HasToken("true"))
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

		public Expression ParseString(Reader reader)
		{
			var expressions = new List<Expression>();

			reader.Expect('\"');

			var whitespace = false;
			var builder = new StringBuilder();
			while (!reader.EOF)
			{
				var toAppend = reader.ReadUntil(
					'{', '\\', '"', ' ', '\t', '\n');
				builder.Append(toAppend);
				if (toAppend.Length > 0)
				{
					whitespace = false;
				}

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
						var str = builder.ToString();
						if (!string.IsNullOrEmpty(str))
						{
							expressions.Add(new LiteralExpression(str));
						}

						// Build expression
						Expression exp = null;
						for (int i = 0; i < expressions.Count; ++i)
						{
							if (exp != null)
							{
								exp = new AddExpression(exp, expressions[i]);
							}
							else
							{
								exp = expressions[i];
							}
						}
						return exp;

					// Substitution
					case '{':
						var str2 = builder.ToString();
						if (!string.IsNullOrEmpty(str2))
						{
							expressions.Add(new LiteralExpression(str2));
						}
						builder = new StringBuilder();

						expressions.Add(CompileExpression(reader));
						whitespace = false;
						break;

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

				// Escaped substitution
				case '{':
					return '{';

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

		public string ParseVariable(Reader reader)
		{
			var variable = new StringBuilder();
			while (!reader.EOF && Reader.VALID_VAR_CHARS.Contains(reader.Peek()))
			{
				variable.Append(reader.Read());
			}

			if (variable.Length == 0)
			{
				throw new ReadException(
					reader, Reader.VALID_VAR_CHARS.ToCharArray());
			}
			return variable.ToString();
		}

		#endregion

		#region Expression

		/// <summary>
		/// Tokenizes an expression.
		/// </summary>
		/// <param name="reader">The reader to tokenize.</param>
		public List<Token> TokenizeExpression(Reader reader)
		{
			// Sanity Check
			if (reader.EOF)
			{
				return new List<Token>();
			}

			Reader temp = null;
			var tokens = new List<Token>();
			var addOp = true;
			var addElement = true;
			var parenthesis = new List<Reader>();
			var isSubstitution = false;
			while (true)
			{
				// Catch up the reader
				if (temp != null)
				{
					reader.Update(temp);
				}
				temp = new Reader(reader);

				// Skip whitespace
				temp.Skip();
				while (!temp.EOF && temp.Peek() == '\n')
				{
					temp.NextLine();
					temp.Skip();
				}

				// Expression has ended
				if (temp.EOF)
				{
					break;
				}

				// Check for parenthesis
				var success = false;
				switch (temp.Peek())
				{
					case '{':
						if (tokens.Count > 0)
						{
							addOp = false;
							addElement = false;
							success = false;
							break;
						}
						isSubstitution = true;
						temp.Read();

						addOp = true;
						addElement = true;
						success = true;
						break;

					case '}':
						if (!isSubstitution)
						{
							throw new CloseSubstitutionException(temp);
						}
						temp.Read();

						addOp = false;
						addElement = false;
						success = true;
						break;

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

				// Check for expression elements
				if (addElement)
				{
					// String Literal
					Expression expression = null;
					var old = new Reader(temp);
					var next = temp.Peek();
					if ('"' == next)
					{
						expression = ParseString(temp);
					}
					// Number Literal
					else if ("1234567890+-.".Contains(next))
					{
						var num = ParseNumber(temp);
						expression = new LiteralExpression(num);
					}
					// Boolean Literal
					else if (temp.HasToken("true") || temp.HasToken("false"))
					{
						var b = ParseBool(temp);
						expression = new LiteralExpression(b);
					}
					// Variable
					else if (Reader.VALID_VAR_CHARS.Contains(next))
					{
						// Check for keywords
						bool isKeyword = false;
						foreach (var keyword in new List<string>() {
							"add", "call", "choice", "choose", "clear", "jump",
							"label", "pause", "return", "say", "if", "elif"})
						{
							if (temp.HasToken(keyword))
							{
								isKeyword = true;
								break;
							}
						}
						if (!isKeyword && temp.HasToken("else"))
						{
							isKeyword = true;
						}

						if (!isKeyword)
						{
							var name = ParseVariable(temp);
							expression = new VariableExpression(name);
						}
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

				if (success)
				{
					continue;
				}

				// Check for operators
				string potentialOp = "";
				if (addOp)
				{
					foreach (var op in ops.Keys)
					{
						if (temp.HasMatch(op, false))
						{
							// Get the longest operator
							if (potentialOp.Length < op.Length)
							{
								potentialOp = op;
							}
						}
					}
				}
				if (!string.IsNullOrEmpty(potentialOp))
				{
					tokens.Add(new Token(temp, potentialOp.Length));
					temp.Read(potentialOp.Length);

					addOp = false;
					addElement = true;
					success = true;
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

		public Expression CompileExpression(
			string expression, int tabSize = Reader.DEFAULT_TAB_SIZE)
		{
			var reader = new Reader(expression, tabSize);
			return CompileExpression(TokenizeExpression(reader));
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

			int opValue = -1; // operator precedence
			int opIndex = -1; // operator index in the token list
			var parenthesis = new List<Token>();

			// Find an operator
			for (int i = 0; i < tokens.Count; ++i)
			{
				var token = tokens[i];

				if (parenthesis.Count == 0 && ops.ContainsKey(token.Text))
				{
					var newOpValue = ops[token.Text];

					// Token is an operator with a lower precedence
					if (newOpValue > opValue)
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
