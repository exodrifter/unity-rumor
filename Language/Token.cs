using Exodrifter.Rumor.Expressions;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// A token is a substring at a specific position with a specific length.
	/// </summary>
	public class Token : ITextPosition
	{
		#region Properties

		/// <summary>
		/// The line number at the beginning of the token.
		/// </summary>
		public int Line
		{
			get { return line; }
		}
		private readonly int line;

		/// <summary>
		/// The column number at the beginning of the token.
		/// </summary>
		public int Column
		{
			get { return column; }
		}
		private readonly int column;

		/// <summary>
		/// The index at the beginning of the token.
		/// </summary>
		public int Index
		{
			get { return index; }
		}
		private readonly int index;

		/// <summary>
		/// The length of the token.
		/// </summary>
		public int Length
		{
			get { return length; }
		}
		private readonly int length;

		/// <summary>
		/// The text of the token.
		/// </summary>
		public string Text
		{
			get { return text; }
		}
		private readonly string text;

		/// <summary>
		/// The expression the token's text evaluates into, if the token is a
		/// literal or variable.
		/// </summary>
		public Expression Expression
		{
			get { return expression; }
		}
		private readonly Expression expression = null;

		#endregion

		/// <summary>
		/// Creates a new token.
		/// </summary>
		/// <param name="reader">The reader at the token's position.</param>
		/// <param name="length">The length of the token.</param>
		/// <param name="expression">The token as an expression.</param>
		public Token(Reader reader, int length, Expression expression = null)
		{
			// Sanity check
			if (reader.Index + length > reader.Script.Length)
			{
				throw new System.ArgumentException(
					"Token is longer than the script!"
				);
			}

			this.line = reader.Line;
			this.column = reader.Column;
			this.index = reader.Index;
			this.length = length;
			this.text = reader.Script.Substring(index, length);
			this.expression = expression;
		}
	}
}