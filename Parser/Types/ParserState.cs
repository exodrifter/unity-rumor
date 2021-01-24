using System.Collections.Generic;

namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Contains the state of a parsing operation.
	/// </summary>
	public class ParserState
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
		/// An arbitrary user state containing additional data that can be
		/// referenced while parsing.
		/// </summary>
		public ParserUserState UserState { get; set; }

		/// <summary>
		/// Creates a new parser state.
		/// </summary>
		/// <param name="source">
		/// The contents of the source to parse.
		/// </param>
		/// <param name="state">
		/// Some arbitrary user state that can accompany a parsing operation.
		/// </param>
		public ParserState
			(string source, int tabSize, ParserUserState userState = null)
		{
			Source = source;
			TabSize = tabSize;
			Index = 0;
			IndentIndex = 0;

			UserState = userState;
		}

		/// <summary>
		/// Creates a copy of a parser state.
		/// </summary>
		/// <param name="other">The other state to make a copy of.</param>
		public ParserState(ParserState other)
		{
			Source = other.Source;
			TabSize = other.TabSize;
			Index = other.Index;
			IndentIndex = other.IndentIndex;

			UserState = other.UserState?.Clone();
		}
	}
}
