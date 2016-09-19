using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Specifies a location that can be jumped to.
	/// </summary>
	[Serializable]
	public sealed class Label : Node
	{
		/// <summary>
		/// The name of the label.
		/// </summary>
		public readonly string name;

		/// <summary>
		/// Creates a new label node.
		/// </summary>
		/// <param name="name">The name of the label.</param>
		/// <param name="children">The children of the label.</param>
		public Label(string name, IEnumerable<Node> children)
			: base(children)
		{
			this.name = name;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.EnterBlock(Children);
			yield return null;
		}

		#region Serialization

		public Label(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			name = info.GetValue<string>("name");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<string>("name", name);
		}

		#endregion
	}
}
