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

		/// <summary>
		/// Called when a choice is made.
		/// </summary>
		public virtual void OnChoice() { }
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
	/// Yields until the correct number of choice event occurs.
	/// </summary>
	public class ForChoice : RumorYield
	{
		private int number;
		private float seconds;
		private float secondsLeft;
		private int @default;
		private bool doUpdate;

		/// <summary>
		/// The number of choices left to make.
		/// </summary>
		public int NumberLeft { get { return number; } }

		/// <summary>
		/// The number of seconds left to make a choice.
		/// </summary>
		public float SecondsLeft { get { return seconds; } }
		public int Default { get { return @default; } }

		public ForChoice(int number, float seconds, int @default)
		{
			this.number = number;
			this.seconds = seconds;
			this.secondsLeft = seconds;
			this.@default = @default;
			this.doUpdate = seconds > 0;
		}

		public override void OnUpdate(float delta)
		{
			if (!doUpdate || Finished) {
				return;
			}

			if (secondsLeft > 0) {
				secondsLeft -= delta;
			}
			Finished = secondsLeft <= 0;
		}

		public override void OnChoice()
		{
			if (Finished) {
				return;
			}

			number = number - 1;
			Finished = number <= 0;

			// Reset the timer
			secondsLeft = seconds;
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