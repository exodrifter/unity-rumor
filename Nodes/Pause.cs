using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Pauses execution of the Rumor for a certain amount of time.
	/// </summary>
	public class Pause : Node
	{
		private readonly float seconds;

		/// <summary>
		/// Creates a new pause node.
		/// </summary>
		/// <param name="seconds">The number of seconds to pause for.</param>
		public Pause(float seconds)
		{
			this.seconds = seconds;
		}

		public override IEnumerator<RumorYield> Run()
		{
			yield return new ForSeconds(seconds);
		}
	}
}
