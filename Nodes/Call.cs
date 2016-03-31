using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Moves execution to a label in a script.
	/// </summary>
	[Serializable]
	public sealed class Call : Node, ISerializable
	{
		/// <summary>
		/// The name of the label to move to.
		/// </summary>
		public readonly string to;

		/// <summary>
		/// Creates a new Call node
		/// </summary>
		/// <param name="to">
		/// The name of the label to move to.
		/// </param>
		public Call(string to)
		{
			this.to = to;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.MoveToLabel(to);
			yield return null;
		}

		#region Serialization

		public Call(SerializationInfo info, StreamingContext context)
		{
			to = (string)info.GetValue("to", typeof(string));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("to", to, typeof(string));
		}

		#endregion
	}
}
