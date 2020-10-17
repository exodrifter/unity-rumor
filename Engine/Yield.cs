namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// This type may be returned from a node to indicate some condition that
	/// should happen before execution may continue.
	/// </summary>
	public abstract class Yield
	{
		public bool Finished { get; protected set; }

		public abstract void Advance();
	}

	/// <summary>
	/// Yield execution until Rumor is advanced.
	/// </summary>
	public class ForAdvance : Yield
	{
		public override void Advance()
		{
			Finished = true;
		}
	}
}
