using System;

namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// A class that can be used to roll back a <see cref="State"/> to a
	/// previous set of values after some actions are done, event if an
	/// exception is thrown.
	/// </summary>
	public class Transaction : IDisposable
	{
		private readonly State state;

		private int index;
		private int indentIndex;

		public Transaction(State state)
		{
			this.state = state;
			index = state.Index;
			indentIndex = state.IndentIndex;
		}

		/// <summary>
		/// Rolls back the state in this transaction to the most-recently
		/// committed state.
		/// </summary>
		public void Rollback()
		{
			state.Index = index;
			state.IndentIndex = indentIndex;
		}

		/// <summary>
		/// Updates the transaction to use the current index when rolling back.
		/// </summary>
		public void CommitIndex()
		{
			index = state.Index;
		}

		public void Dispose()
		{
			Rollback();
		}
	}
}
