﻿namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Thrown when the parser is unable to find the expected input.
	/// </summary>
	public class ExpectedException : ParserException
	{
		/// <summary>
		/// The potential valid input the parser was expecting.
		/// </summary>
		public string[] Expected { get; }

		public ExpectedException(int index, params string[] expected)
			: base(index)
		{
			Expected = expected;
		}

		protected override string GenerateMessage()
		{
			return "expected " + string.Join(", ", Expected);
		}
	}
}
