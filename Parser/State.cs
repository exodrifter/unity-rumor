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
		public int Index { get; }

		/// <summary>
		/// The index in the source file to check the current indentation level
		/// against, if needed.
		/// </summary>
		public int IndentIndex { get; }

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
		/// Returns a new state with the index changed by the specified delta.
		/// </summary>
		/// <param name="indexDelta">
		/// The amount to change the index.
		/// </param>
		/// <returns>
		/// A new state with a modified index.
		/// </returns>
		public State AddIndex(int indexDelta)
		{
			return new State(
				Source,
				TabSize,
				Index + indexDelta,
				IndentIndex
			);
		}

		/// <summary>
		/// Sets the index of the location used for checking indentation levels
		/// to the current index.
		/// </summary>
		/// <returns>
		/// A new state with a modified index for checking indentation levels.
		/// </returns>
		public State SetIndent()
		{
			return new State(
				Source,
				TabSize,
				Index,
				Index
			);
		}

		#region Internal

		private State(string source, int tabSize, int index, int indentIndex)
		{
			Source = source;
			TabSize = tabSize;
			Index = index;
			IndentIndex = indentIndex;
		}

		#endregion
	}
}
