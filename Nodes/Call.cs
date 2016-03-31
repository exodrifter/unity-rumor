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
		public readonly string to;

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
