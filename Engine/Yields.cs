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
		/// The total amount of time spent on this yield.
		/// </summary>
		public float Elapsed { get; protected set; }

		/// <summary>
		/// Called when an update event occurs.
		/// </summary>
		/// <param name="rumor">
		/// The rumor using this yield.
		/// </param>
		/// <param name="delta">
		/// The amount of time in seconds since the last time this was called.
		/// </param>
		public virtual void OnUpdate(Rumor rumor, float delta)
		{
			Elapsed += delta;
		}

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
		public float SecondsLeft { get { return secondsLeft; } }
		public int Default { get { return @default; } }

		public ForChoice(int number, float seconds, int @default)
		{
			this.number = number;
			this.seconds = seconds;
			this.secondsLeft = seconds;
			this.@default = @default;
			this.doUpdate = seconds > 0;
		}

		public override void OnUpdate(Rumor rumor, float delta)
		{
			base.OnUpdate(rumor, delta);

			// Return early if there are no choices left
			if (rumor != null && rumor.State.Choices.Count == 0) {
				Finished = true;
			}

			if (!doUpdate || Finished) {
				return;
			}

			if (secondsLeft > 0) {
				secondsLeft -= delta;
			}

			if (secondsLeft <= 0) {
				if (rumor != null) {
					rumor.Choose(@default);
				}
				Finished = true;
			}
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
		private bool cantSkip;

		public ForSeconds(float seconds, bool cantSkip)
		{
			this.seconds = seconds;
			this.cantSkip = cantSkip;
		}

		public override void OnAdvance()
		{
			if (!cantSkip)
			{
				Finished = true;
			}
		}

		public override void OnChoice()
		{
			if (!cantSkip)
			{
				Finished = true;
			}
		}

		public override void OnUpdate(Rumor rumor, float delta)
		{
			base.OnUpdate(rumor, delta);
			Finished = Finished || seconds <= Elapsed;
		}
	}
}