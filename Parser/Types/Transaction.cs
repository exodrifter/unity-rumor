using System;

namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// A class that can be used to roll back a <see cref="ParserState"/> to a
	/// previous set of values after some actions are done, even if an
	/// exception is thrown.
	/// </summary>
	public class Transaction : IDisposable
	{
		private readonly ParserState state;

		private int index;
		private int indentIndex;

		private ParserUserState userState;

		public Transaction(ParserState state)
		{
			this.state = state;
			index = state.Index;
			indentIndex = state.IndentIndex;
			userState = state.UserState?.Clone();
		}

		/// <summary>
		/// Rolls back the state in this transaction to the most-recently
		/// committed state.
		/// </summary>
		public void Rollback()
		{
			state.Index = index;
			state.IndentIndex = indentIndex;
			state.UserState = userState;
		}

		/// <summary>
		/// Updates the transaction to use the current index when rolling back.
		/// </summary>
		public void CommitIndex()
		{
			index = state.Index;
			userState = state.UserState;
		}

		public void Dispose()
		{
			Rollback();
		}
	}
}
