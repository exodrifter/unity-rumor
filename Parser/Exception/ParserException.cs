using System;

namespace Exodrifter.Rumor.Parser
{
	public abstract class ParserException : Exception
	{
		public int Index { get; }

		public ParserException(int index, string message)
			: base(MakeMessage(index, message))
		{
			Index = index;
		}

		public ParserException(int index, string message, Exception inner)
			: base(MakeMessage(index, message), inner)
		{
			Index = index;
		}

		private static string MakeMessage(int index, string message)
		{
			return "parse exception at index " + index + ": " + message;
		}
	}
}
