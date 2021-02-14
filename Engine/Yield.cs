namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// This type may be returned from a node to indicate some condition that
	/// should happen before execution may continue.
	/// </summary>
	public abstract class Yield
	{
		public bool Finished { get; protected set; }
		public double Elapsed { get; protected set; }

		public virtual void Advance() { }

		public virtual void Update(double delta)
		{
			Elapsed += delta;
		}

		public virtual void Choose() { }
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

		public ForSeconds(double seconds)
		{
			time = seconds;
			Elapsed = 0;
		}

		public override void Update(double delta)
		{
			base.Update(delta);
			Finished = Elapsed >= time;
		}
	}

	/// <summary>
	/// Yield execution until Rumor is advanced.
	/// </summary>
	public class ForChoose : Yield
	{
		public ForChoose() { }

		public override void Choose()
		{
			Finished = true;
		}
	}
}
