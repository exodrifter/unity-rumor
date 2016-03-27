using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Pauses execution of the Rumor for a certain amount of time.
	/// </summary>
	[Serializable]
	public sealed class Pause : Node, ISerializable
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

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			yield return new ForSeconds(seconds);
		}

		#region Serialization

		public Pause(SerializationInfo info, StreamingContext context)
		{
			seconds = (float)info.GetValue("seconds", typeof(float));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("seconds", seconds, typeof(float));
		}

		#endregion
	}
}
