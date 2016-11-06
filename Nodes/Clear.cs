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
	public sealed class Clear : Node
	{
		/// <summary>
		/// Creates a new Return node.
		/// </summary>
		public Clear() { }

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.State.Clear();
			yield return null;
		}

		#region Serialization

		public Clear(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		#endregion
	}
}
