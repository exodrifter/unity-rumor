using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Specifies a location that can be jumped to.
	/// </summary>
	public class Label : Node
	{
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
	}
}
