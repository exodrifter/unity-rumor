namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Contains the state of a parsing operation.
	/// </summary>
	public class State
	{
		/// <summary>
		/// The contents of the source file that is being parsed.
		/// </summary>
		public string Source { get; }

		/// <summary>
		/// The number of spaces each tab in the source file should be treated
		/// as.
		/// </summary>
		public int TabSize { get; }

		/// <summary>
		/// The current index in the source file the parser is pointing at.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// The index in the source file to check the current indentation level
		/// against, if needed.
		/// </summary>
		public int IndentIndex { get; set; }

		/// <summary>
		/// Returns true if the state is pointing to the end of the file.
		/// </summary>
		public bool EOF { get { return Index >= Source.Length; } }

		/// <summary>
		/// Creates a new parser state.
		/// </summary>
		/// <param name="source">
		/// The contents of the source to parse.
		/// </param>
		/// <param name="index">
		/// The index where the parser should start.
		/// </param>
		public State(string source, int tabSize, int index = 0)
		{
			Source = source;
			TabSize = tabSize;
			Index = index;
			IndentIndex = 0;
		}

		/// <summary>
		/// Creates a copy of a parser state.
		/// </summary>
		/// <param name="other">The other state to make a copy of.</param>
		public State(State other)
		{
			Source = other.Source;
			TabSize = other.TabSize;
			Index = other.Index;
			IndentIndex = other.IndentIndex;
		}
	}
}
