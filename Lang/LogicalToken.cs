namespace Exodrifter.Rumor.Lang
{
	/// <summary>
	/// A logical token is a token with row and column information.
	/// </summary>
	public class LogicalToken
	{
		/// <summary>
		/// The contents of this token.
		/// </summary>
		public readonly string text;

		/// <summary>
		/// The line this token appears on.
		/// </summary>
		public readonly int? row;

		/// <summary>
		/// The index (of the first character in the token) within the row.
		/// </summary>
		public readonly int? col;

		/// <summary>
		/// Creates a new logical token.
		/// </summary>
		/// <param name="text">
		/// The contents of this token.
		/// </param>
		/// <param name="row">
		/// The line this token appears on.
		/// </param>
		/// <param name="col">
		/// The index (of the first character in the token) within the row.
		/// </param>
		public LogicalToken(string text, int? row = null, int? col = null)
		{
			this.row = row;
			this.col = col;
			this.text = text;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\"[{1},{2}]",
				text, (object)row ?? "?", (object)col ?? "?");
		}
	}
}
