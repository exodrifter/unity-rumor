using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Exits the current block.
	/// </summary>
	[Serializable]
	public sealed class Return : Node, ISerializable
	{
		public Return() { }

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.ExitBlock();
			yield return null;
		}

		#region Serialization

		public Return(SerializationInfo info, StreamingContext context)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}

		#endregion
	}
}
