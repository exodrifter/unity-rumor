using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Pauses execution for a certain amount of time.
	/// </summary>
	[Serializable]
	public sealed class Pause : Node
	{
		/// <summary>
		/// The number of seconds to pause for.
		/// </summary>
		public readonly float seconds;

		/// <summary>
		/// Creates a new pause node.
		/// </summary>
		/// <param name="seconds">
		/// The number of seconds to pause for.
		/// </param>
		public Pause(float seconds)
		{
			this.seconds = seconds;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			yield return new ForSeconds(seconds);
		}

		#region Serialization

		public Pause(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			seconds = (float)info.GetValue("seconds", typeof(float));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("seconds", seconds, typeof(float));
		}

		#endregion
	}
}
