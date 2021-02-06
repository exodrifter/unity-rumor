using System;

namespace Exodrifter.Rumor.Engine
{
	public class UndefinedVariableException : Exception
	{
		public UndefinedVariableException(string message) : base(message) { }
	}
}
