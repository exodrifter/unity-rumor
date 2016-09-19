using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Jumps execution to a label in a script.
	/// </summary>
	[Serializable]
	public sealed class Jump : Node
	{
		/// <summary>
		/// The name of the label to jump to.
		/// </summary>
		public readonly string to;

		/// <summary>
		/// Creates a new jump node.
		/// </summary>
		/// <param name="to">
		/// The name of the label to jump to.
		/// </param>
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
			: base(info, context)
		{
			to = info.GetValue<string>("to");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<string>("to", to);
		}

		#endregion
	}
}
