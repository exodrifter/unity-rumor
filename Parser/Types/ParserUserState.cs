/// <summary>
/// Represents some arbitrary user state that can optionally accompany a parsing
/// operation.
/// </summary>
public interface ParserUserState
{
	/// <summary>
	/// Returns a new deep copy of the current state.
	/// </summary>
	ParserUserState Clone();
}
