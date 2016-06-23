using Exodrifter.Rumor.Nodes;
using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// The compiler for Rumor scripts.
	/// </summary>
	public class RumorCompiler
	{
		private readonly Tokenizer tokenizer;
		private readonly Cleaner cleaner;
		private readonly Parser parser;

		private readonly int tabsize;

		private delegate Node NodeParser
			(LogicalLine line, ref int pos, List<Node> children);
		private readonly Dictionary<string, NodeParser> handlers;

		private delegate Conditional ConditionalParser
			(LogicalLine line, ref int pos, List<Node> children,
			List<LogicalLine> lines, ref int index, int depth);
		private readonly Dictionary<string, ConditionalParser> conditions;

		/// <summary>
		/// Creates a new Rumor compiler.
		/// </summary>
		/// <param name="tabsize">
		/// The number of spaces to treat each tab as.
		/// </param>
		public RumorCompiler(int tabsize = 4)
		{
			this.tabsize = tabsize;

			tokenizer = new Tokenizer(
				new List<string>() {
					" ", "\t", "\n", "\r",
					"+", "-", "/", "*",
					"$", "=", "!",
					"==", "!=", "&&", "||",
					"\"", "\'", "\\",
					"(", ")", "{", "}",
					":", ".",
				},
				new List<string>() {
					"([-+]?(?:(?:[0-9]+(?:\\.[0-9]+)?|\\.[0-9]+)(?:[eE][0-9]+)?))",
				}
			);

			cleaner = new Cleaner.Factory()
				.SetDiscardEmptyLines(true)
				.AddNewline("\n")
				.AddRemove("\r")
				.Instantiate();

			parser = new Parser.Factory()
				.AddDelimiter("\"", "\"", new List<string>() { "\\" })
				.AddTerminator("\n")
				.Instantiate();

			handlers = new Dictionary<string, NodeParser>();
			handlers["$"] = CompileStatement;
			handlers["add"] = CompileAdd;
			handlers["choice"] = CompileChoice;
			handlers["label"] = CompileLabel;
			handlers["pause"] = CompilePause;
			handlers["say"] = CompileSay;

			conditions = new Dictionary<string, ConditionalParser>();
			conditions["if"] = CompileIf;
			conditions["elif"] = CompileElif;
			conditions["else"] = CompileElse;
		}

		public IEnumerable<Node> Compile(string code)
		{
			var tokens = cleaner.Clean(tokenizer.Tokenize(code));
			var lines = new List<LogicalLine>(parser.Parse(tokens));

			int pos = 0;
			int index = 0;
			var depth = SkipWhitespace(lines[0], ref pos);
			return CompileNodes(lines, ref index, depth);
		}

		/// <summary>
		/// Returns the keyword at the beginning of the logical line.
		/// </summary>
		/// <param name="line">
		/// The logical line to parse.
		/// </param>
		/// <param name="pos">
		/// The position of the next non-whitespace character after the keyword
		/// at the beginning of the line.
		/// </param>
		/// <param name="depth">
		/// The depth of the line.
		/// </param>
		/// <returns>
		/// The key of the logical line.
		/// </returns>
		private string GetKey(LogicalLine line, out int pos, out int depth)
		{
			pos = 0;
			depth = SkipWhitespace(line, ref pos);
			var key = line.tokens[pos].text;

			pos++;
			SkipWhitespace(line, ref pos);

			return key;
		}

		private List<Node> GetChildren(List<LogicalLine> lines, ref int index, int depth)
		{
			var children = new List<Node>();
			if (index + 1 < lines.Count) {
				int nextPos, nextDepth;
				GetKey(lines[index + 1], out nextPos, out nextDepth);

				if (nextDepth > depth) {
					index++;
					children = CompileNodes(lines, ref index, nextDepth);
				}
			}
			return children;
		}

		private List<Node> CompileNodes
			(List<LogicalLine> lines, ref int index, int depth)
		{
			var nodes = new List<Node>();

			for (; index < lines.Count; ++index) {
				var line = lines[index];
				int pos, currentDepth;
				var key = GetKey(line, out pos, out currentDepth);

				// Check if the line is at an exit depth
				if (currentDepth < depth) {
					index--;
					return nodes;
				}
				// Check if the line is at an unexpected block depth
				if (currentDepth > depth) {
					throw new CompilerError(line, "Unexpected block");
				}

				if (conditions.ContainsKey(key)) {
					var conditional = CompileCondition(lines, ref index, depth);
					if (conditional is Elif || conditional is Else) {
						throw new CompilerError(line,
							string.Format(
								"Unexpected conditional of type \"{0}\"",
								conditional.GetType()));
					}
					nodes.Add(new Condition(conditional));
					continue;
				}

				if (!handlers.ContainsKey(key)) {
					throw new CompilerError(line.tokens[pos],
						string.Format("Unknown keyword \"{0}\"", key));
				}

				var children = GetChildren(lines, ref index, depth);
				nodes.Add(handlers[key](line, ref pos, children));
			}

			return nodes;
		}

		public Conditional CompileCondition
			(List<LogicalLine> lines, ref int index, int depth)
		{
			var line = lines[index];
			int pos, currentDepth;
			var key = GetKey(line, out pos, out currentDepth);

			// Check if the line is at the same depth
			if (currentDepth != depth) {
				return null;
			}

			if (!conditions.ContainsKey(key)) {
				throw new CompilerError(line.tokens[pos],
					string.Format("Unknown keyword \"{0}\"", key));
			}

			var children = GetChildren(lines, ref index, depth);

			return conditions[key](line, ref pos, children,
				lines, ref index, depth);
		}

		public Expression CompileExpression(List<LogicalToken> tokens)
		{
			tokens = Trim(tokens);
			if (tokens == null || tokens.Count == 0) {
				return new NoOpExpression();
			}

			var ops = new List<string>() {
				"*", "/", "+", "-",
				"and", "xor", "or",
				"==", "!=",
				"=",
			};
			int opValue = int.MinValue; // operator's index in ops list
			int opIndex = -1; // operator's index in the token list
			int parenthesis = 0;

			for (int i = 0; i < tokens.Count; ++i) {
				var token = tokens[i];

				if (parenthesis == 0 && ops.Contains(token.text)) {
					var newOpValue = ops.IndexOf(token.text);

					// Token is an operator with a lower precedence
					if (newOpValue > opValue) {
						opValue = newOpValue;
						opIndex = i;
					}
				}
				else if (token.text == "(") {
					parenthesis++;
				}
				else if (token.text == ")") {
					parenthesis--;
					if (parenthesis < 0) {
						throw new CompilerError(token,
							"Unexpected close parenthesis!");
					}
				}
			}

			if (parenthesis != 0) {
				throw new CompilerError(tokens[tokens.Count - 1],
					"Expected close parenthesis, but there is none!");
			}

			// Split on the operator, if there is one
			if ((0 <= opValue && opValue < ops.Count)|| opIndex != -1) {
				var left = CompileExpression(Slice(tokens, 0, opIndex));
				var right = CompileExpression(Slice(tokens, opIndex + 1));
				switch (ops[opValue]) {
					case "=":
						return new SetExpression(left, right);
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
					case "and":
						return new BoolAndExpression(left, right);
					case "xor":
						return new BoolXorExpression(left, right);
					case "or":
						return new BoolOrExpression(left, right);
				}
			}

			else {
				// Drop parenthesis, if they exist
				var parenthesisAtStart = tokens[0].text == "(";
				var parenthesisAtEnd = tokens[tokens.Count - 1].text == ")";
				if (parenthesisAtStart && parenthesisAtEnd) {
					return CompileExpression(Slice(tokens, 1, -1));
				}
				// Parse the literal
				else if (tokens.Count == 1) {
					var str = tokens[0].text;
					float @float;
					if (float.TryParse(str, out @float)) {
						return new LiteralExpression(@float);
					}
					int @int;
					if (int.TryParse(str, out @int)) {
						return new LiteralExpression(@int);
					}
					// Assume the token is a variable
					return new VariableExpression(str);
				}
				// Parse a string
				else {
					int pos = 0;
					var str = Quote(new LogicalLine(tokens), ref pos);
					if (str != null && pos == tokens.Count) {
						return new LiteralExpression(str);
					}
					else {
						throw new CompilerError(tokens[tokens.Count - 1],
							"Unable to parse expression");
					}
				}
			}

			return null;
		}

		#region Node Compilation Functions

		private Node CompileAdd(LogicalLine line, ref int pos, List<Node> children)
		{
			ExpectNoChildren(line, children);

			string text = Quote(line, ref pos);
			return new Add(text);
		}

		private Node CompileChoice(LogicalLine line, ref int pos, List<Node> children)
		{
			string text = Quote(line, ref pos);

			SkipWhitespace(line, ref pos);
			Expect(line, pos++, ":");

			return new Choice(text, children);
		}

		private Node CompileLabel(LogicalLine line, ref int pos, List<Node> children)
		{
			var label = Expect(line, pos++);

			SkipWhitespace(line, ref pos);
			Expect(line, pos++, ":");

			return new Label(label, children);
		}

		private Node CompilePause(LogicalLine line, ref int pos, List<Node> children)
		{
			ExpectNoChildren(line, children);

			float seconds = Float(line, ref pos);
			return new Pause(seconds);
		}

		private Node CompileSay(LogicalLine line, ref int pos, List<Node> children)
		{
			ExpectNoChildren(line, children);
			
			var tokens = Slice(line.tokens, pos);
			var expression = CompileExpression(tokens);
			return new Say(expression);
		}

		private Node CompileStatement(LogicalLine line, ref int pos, List<Node> children)
		{
			ExpectNoChildren(line, children);

			var tokens = Slice(line.tokens, pos);
			var expression = CompileExpression(tokens);
			return new Statement(expression);
		}

		#endregion

		#region Conditional Compilation Functions

		private Expression CompileConditional(LogicalLine line, ref int pos, List<Node> children)
		{
			int end = Seek(line, pos, ":");

			var tokens = Slice(line.tokens, pos, end);
			var expression = CompileExpression(tokens);

			pos = end;

			Expect(line, pos++, ":");

			return expression;
		}

		private Conditional CompileIf(LogicalLine line, ref int pos, List<Node> children,
			List<LogicalLine> lines, ref int index, int depth)
		{
			var expression = CompileConditional(line, ref pos, children);

			int tempIndex = index++;
			var nextCondition = CompileCondition(lines, ref index, depth);
			if (nextCondition is Elif) {
				return new If(expression, children, (Elif)nextCondition);
			} else if (nextCondition is Else) {
				return new If(expression, children, (Else)nextCondition);
			}
			index = tempIndex;
			return new If(expression, children);
		}

		private Conditional CompileElif(LogicalLine line, ref int pos, List<Node> children,
			List<LogicalLine> lines, ref int index, int depth)
		{
			var expression = CompileConditional(line, ref pos, children);

			int tempIndex = index++;
			var nextCondition = CompileCondition(lines, ref index, depth);
			if (nextCondition is Elif) {
				return new Elif(expression, children, (Elif)nextCondition);
			} else if (nextCondition is Else) {
				return new Elif(expression, children, (Else)nextCondition);
			}
			index = tempIndex;
			return new Elif(expression, children);
		}

		private Conditional CompileElse(LogicalLine line, ref int pos, List<Node> children,
			List<LogicalLine> lines, ref int index, int depth)
		{
			Expect(line, pos++, ":");
			return new Else(children);
		}

		#endregion

		#region Generic Lexer Functions

		public int Seek(LogicalLine line, int pos, string text)
		{
			while (pos < line.tokens.Count) {
				if (line.tokens[pos].text == text) {
					return pos;
				}
				pos++;
			}

			throw new CompilerError(line,
				"Expected to find a token, but there is none!");
		}

		public string Expect(LogicalLine line, int pos)
		{
			if (pos >= line.tokens.Count) {
				throw new CompilerError(line,
					"Expected a token, but there is none!");
			}

			var token = line.tokens[pos];
			if (IsWhitespace(token)) {
				throw new CompilerError(token,
					"Expected a token, but found whitespace instead!");
			}

			return token.text;
		}

		public string Expect(LogicalLine line, int pos, string text)
		{
			if (pos >= line.tokens.Count) {
				throw new CompilerError(line,
					"Expected a token \"" + text + "\", but there is none!");
			}

			var token = line.tokens[pos];
			if (token.text != text) {
				throw new CompilerError(token,
					string.Format("Expected \"{0}\", got \"{1}\" instead!",
						text, token.text));
			}

			return token.text;
		}

		public void ExpectNoChildren(LogicalLine line, List<Node> children)
		{
			if (children.Count > 0) {
				throw new CompilerError(line,
					string.Format("unexpected children following statement"));
			}
		}

		/// <summary>
		/// Returns true if the token is considered whitespace.
		/// </summary>
		/// <param name="tk">
		/// The token to check.
		/// </param>
		/// <returns>
		/// True if the token is whitespace.
		/// </returns>
		public bool IsWhitespace(LogicalToken tk)
		{
			switch (tk.text) {
				case " ": return true;
				case "\t": return true;
				case "\r": return true;
				case "\n": return true;
				default: return false;
			}
		}

		/// <summary>
		/// Returns the "size" of the whitespace.
		/// </summary>
		/// <param name="tk">
		/// The token to check.
		/// </param>
		/// <returns>
		/// The size of the whitespace, or 0 if the token is not considered
		/// whitespace.
		/// </returns>
		public int Whitespace(LogicalToken tk)
		{
			switch (tk.text) {
				case " ": return 1;
				case "\t": return tabsize;
				default: return 0;
			}
		}

		/// <summary>
		/// Moves the referenced position to the first non-whitespace
		/// character.
		/// </summary>
		/// <param name="line">The line to skip whitespace in.</param>
		/// <param name="pos">The position to move.</param>
		/// <return>The size of the whitespace skipped.</return>
		public int SkipWhitespace(LogicalLine line, ref int pos)
		{
			int size = 0;
			while (pos < line.tokens.Count) {
				var tk = line.tokens[pos];

				if (IsWhitespace(tk)) {
					pos++;
					size += Whitespace(tk);
					continue;
				}
				else {
					break;
				}
			}

			return size;
		}

		/// <summary>
		/// Returns the contents of an escape sequence.
		/// </summary>
		/// <param name="line">The line to find the escape sequence in.</param>
		/// <param name="pos">The position to start searching from.</param>
		/// <returns>The unescaped sequence.</returns>
		public string Escape(LogicalLine line, ref int pos)
		{
			// Check if the next character is an escape sequence
			Expect(line, pos++, "\\");
			var str = line.tokens[pos].text;

			// Escaped quote
			if (str == "\"") {
				pos++;
				return "\"";
			}
			// Escaped blackslash
			else if (str == "\\") {
				pos++;
				return "\\";
			}
			// Escaped newline
			else if (str == "n") {
				pos++;
				return "\n";
			}
			// Escaped unicode
			else if (str.StartsWith("u")) {
				string unicode = str.Substring(1);

				if (unicode.Length != 4) {
					throw new CompilerError(line.tokens[pos],
						"unicode sequence \"" + unicode
						+ "\" is not 4 characters long");
				}

				pos++;
				return "" + (char)Convert.ToInt32(unicode, 16);
			}
			else {
				throw new CompilerError(line.tokens[pos],
					"Unknown escape sequence \"\\" + str + "\"");
			}
		}

		/// <summary>
		/// Returns the value of the float at the specified position.
		/// </summary>
		/// <param name="line">The line to find the quote in.</param>
		/// <param name="pos">The position to start searching from.</param>
		/// <returns>The next float value.</returns>
		public float Float(LogicalLine line, ref int pos)
		{
			string str = line.tokens[pos++].text;
			try {
				return float.Parse(str);
			}
			catch (FormatException) {
				throw new CompilerError(line.tokens[pos],
					"Invalid float format \"\\" + str + "\"");
			}
			catch (OverflowException) {
				throw new CompilerError(line.tokens[pos],
					"Float \"\\" + str + "\" is too large");
			}
		}

		/// <summary>
		/// Returns the contents of the next quote at the specified position.
		/// 
		/// A quote is defined as a sequence of characters delimited by the
		/// '"' character.
		/// </summary>
		/// <param name="line">The line to find the quote in.</param>
		/// <param name="pos">The position to start searching from.</param>
		/// <returns>The next quote.</returns>
		public string Quote(LogicalLine line, ref int pos)
		{
			// Check if the next character is an open quote
			Expect(line, pos++, "\"");
			string quote = "";
			bool foundEndQuote = false;

			// Add the rest of the string
			while (pos < line.tokens.Count) {

				var str = line.tokens[pos].text;

				// Check if the next character is an escape
				if (str == "\\") {
					quote += Escape(line, ref pos);
				}
				// Check if the next character is the end quote
				else if (str == "\"") {
					foundEndQuote = true;
					pos++;
				}
				// Otherwise, continue building the quote
				else {
					quote += str;
					pos++;
				}

				if (foundEndQuote) {
					break;
				}
			}

			return foundEndQuote ? quote : null;
		}

		#endregion

		#region Helper Methods

		private static List<T> Slice<T>(List<T> list,
			int? begin = null, int? end = null)
		{
			begin = begin ?? 0;
			end = end ?? list.Count;
			while (begin < 0) {
				begin += list.Count;
			}
			while (end < 0) {
				end += list.Count;
			}

			var ret = new List<T>();
			for (int i = begin.Value; i < end.Value; ++i) {
				ret.Add(list[i]);
			}

			return ret;
		}

		private List<LogicalToken> Trim(List<LogicalToken> list)
		{
			if (list == null) {
				return null;
			}

			var ret = new List<LogicalToken>();

			bool add = false;
			for (int i = 0; i < list.Count; ++i) {
				if (add) {
					ret.Add(list[i]);
				}
				else if (!IsWhitespace(list[i])) {
					add = true;
					ret.Add(list[i]);
				}
			}

			while (ret.Count > 0 && IsWhitespace(ret[ret.Count - 1])) {
				ret.RemoveAt(ret.Count - 1);
			}

			return ret;
		}

		#endregion
	}
}