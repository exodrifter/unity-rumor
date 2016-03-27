using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// A jump node jumps to a label in a script.
	/// </summary>
	[Serializable]
	public sealed class Jump : Node, ISerializable
	{
		public readonly string to;

		/// <summary>
		/// Creates a new jump node.
		/// </summary>
		/// <param name="to">The name of the label to jump to.</param>
		public Jump(string to)
		{
			this.to = to;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.JumpToLabel(to);
			yield return null;
		}

		#region Serialization

		public Jump(SerializationInfo info, StreamingContext context)
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
