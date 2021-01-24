using System.Collections.Generic;

public class RumorParserState : ParserUserState
{
	/// <summary>
	/// A list of identifiers that have already been used.
	/// </summary>
	public HashSet<string> UsedIdentifiers { get; }

	public RumorParserState()
	{
		UsedIdentifiers = new HashSet<string>();
	}

	public RumorParserState(RumorParserState other)
	{
		UsedIdentifiers = new HashSet<string>(other.UsedIdentifiers);
	}

	public ParserUserState Clone()
	{
		return new RumorParserState(this);
	}
}
