using System;

namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// A class that can be used to roll back a <see cref="State"/> if an
	/// exception is thrown. Also resets the indent index back to the original
	/// value.
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
		/// Updates the transaction to use the current state when rolling back.
		/// </summary>
		public void Commit()
		{
			index = state.Index;
		}

		public void Dispose()
		{
			Rollback();
		}
	}
}
