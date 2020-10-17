namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// This type may be returned from a node to indicate some condition that
	/// should happen before execution may continue.
	/// </summary>
	public abstract class Yield
	{
		public bool Finished { get; protected set; }

		public virtual void Advance() { }

		public virtual void Update(double delta) { }
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

	/// <summary>
	/// Yield execution until Rumor is advanced.
	/// </summary>
	public class ForSeconds : Yield
	{
		private readonly double time;
		private double elapsed;

		public ForSeconds(double seconds)
		{
			time = seconds;
			elapsed = 0;
		}

		public override void Update(double delta)
		{
			if (elapsed < time)
			{
				elapsed += delta;
				Finished = elapsed >= time;
			}
		}
	}
}
