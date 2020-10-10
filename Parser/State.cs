namespace Exodrifter.Rumor.Parser
{
	public class State
	{
		public string Source { get; }
		public int Index { get; }

		public State(string source, int index = 0)
		{
			Source = source;
			Index = index;
		}

		/// <summary>
		/// Makes a copy of another state except for the index.
		/// </summary>
		/// <param name="state">The state to copy.</param>
		/// <param name="index">The new index.</param>
		public State(State state, int index = 0)
		{
			Source = state.Source;
			Index = index;
		}

		public State AddIndex(int indexDelta)
		{
			return new State(this, Index + indexDelta);
		}
	}
}
