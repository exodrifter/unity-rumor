using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	public abstract class Node
	{
		/// <summary>
		/// The children of this node.
		/// </summary>
		protected readonly List<Node> children;

		/// <summary>
		/// Returns a copy of the children in this node.
		/// </summary>
		public List<Node> Children
		{
			get
			{
				return new List<Node>(children);
			}
		}

		/// <summary>
		/// Creates a new node with no children.
		/// </summary>
		protected Node()
		{
			children = new List<Node>();
		}

		/// <summary>
		/// Creates a new node with the specified children.
		/// </summary>
		/// <param name="children">The children of this node.</param>
		protected Node(List<Node> children)
		{
			if (children == null) {
				this.children = new List<Node>();
			}
			else {
				this.children = new List<Node>(children);
			}
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
