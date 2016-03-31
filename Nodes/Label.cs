using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Specifies a location that can be jumped to.
	/// </summary>
	[Serializable]
	public sealed class Label : Node, ISerializable
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
		public Label(string name, List<Node> children)
			: base(children)
		{
			this.name = name;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.EnterBlock(children);
			yield return null;
		}

		#region Serialization

		public Label(SerializationInfo info, StreamingContext context)
			: base((List<Node>)info.GetValue("children", typeof(List<Node>)))
		{
			name = (string)info.GetValue("name", typeof(string));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("children", children, typeof(List<Node>));
			info.AddValue("name", name, typeof(string));
		}

		#endregion
	}
}
