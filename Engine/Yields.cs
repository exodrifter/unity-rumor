namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Defines a yield that may be returned from a node to wait for an event
	/// to occur.
	/// </summary>
	public abstract class RumorYield
	{
		/// <summary>
		/// True if the yield has finished waiting.
		/// </summary>
		public bool Finished { get; protected set; }

		/// <summary>
		/// Called when an update event occurs.
		/// </summary>
		/// <param name="delta">
		/// The amount of time in seconds since the last time this was called.
		/// </param>
		public virtual void OnUpdate(float delta) { }

		/// <summary>
		/// Called when an advance event occurs.
		/// </summary>
		public virtual void OnAdvance() { }
	}

	/// <summary>
	/// Yields until an advance event occurs.
	/// </summary>
	public class ForAdvance : RumorYield
	{
		public override void OnAdvance()
		{
			Finished = true;
		}
	}

	/// <summary>
	/// Yields until a certain number of seconds has passed.
	/// </summary>
	public class ForSeconds : RumorYield
	{
		private float seconds;

		public ForSeconds(float seconds)
		{
			this.seconds = seconds;
		}

		public override void OnUpdate(float delta)
		{
			if (seconds > 0) {
				seconds -= delta;
			}
			Finished = seconds <= 0;
		}
	}
}