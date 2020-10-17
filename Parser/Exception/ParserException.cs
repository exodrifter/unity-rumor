using System;

namespace Exodrifter.Rumor.Parser
{
	public abstract class ParserException : Exception
	{
		/// <summary>
		/// The index where this exception occurred.
		/// </summary>
		public int Index { get; }

		public ParserException(int index)
		{
			Index = index;
		}

		protected abstract string GenerateMessage();

		public override string ToString()
		{
			return "Parser Exception at index " + Index + ": " +
				GenerateMessage();
		}
	}
}