using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	public abstract class Node : Block
	{
		/// <summary>
		/// Creates a new node with no children.
		/// </summary>
		protected Node()
			: base()
		{
		}

		/// <summary>
		/// Creates a new node with the specified children.
		/// </summary>
		/// <param name="children">The children of this node.</param>
		protected Node(IEnumerable<Node> children)
			: base(children)
		{
		}

		/// <summary>
		/// Runs the current node.
		/// </summary>
		/// <returns>
		/// A IEnumerator that can be used to continue execution of the node.
		/// </returns>
		public abstract IEnumerator<RumorYield> Run(Engine.Rumor rumor);
	}
}
