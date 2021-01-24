using System.Collections.Generic;
using Exodrifter.Rumor.Engine;

namespace Exodrifter.Rumor.Compiler
{
	public class RumorParserState : ParserUserState
	{
		/// <summary>
		/// A list of identifiers that have already been used.
		/// </summary>
		public HashSet<string> UsedIdentifiers { get; }

		/// <summary>
		/// A list of variables that have been referenced.
		/// </summary>
		public Dictionary<string, ValueType> UsedVariables { get; }

		public RumorParserState()
		{
			UsedIdentifiers = new HashSet<string>();
			UsedVariables = new Dictionary<string, ValueType>();
		}

		public RumorParserState(RumorParserState other)
		{
			UsedIdentifiers = new HashSet<string>(other.UsedIdentifiers);
			UsedVariables = new Dictionary<string, ValueType>(other.UsedVariables);
		}

		public ParserUserState Clone()
		{
			return new RumorParserState(this);
		}
	}
}
