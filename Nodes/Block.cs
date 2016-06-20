using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// A block is a structure that holds a reference to a list of blocks.
	/// </summary>
	public abstract class Block
	{
		/// <summary>
		/// The children of this block.
		/// </summary>
		private readonly List<Node> children;

		/// <summary>
		/// Returns a list of children in this block.
		/// </summary>
		public List<Node> Children
		{
			get { return new List<Node>(children); }
		}

		/// <summary>
		/// Creates a new block with no children.
		/// </summary>
		protected Block()
		{
			children = new List<Node>();
		}

		/// <summary>
		/// Creates a new block the specified children.
		/// </summary>
		/// <param name="children">The children of this block.</param>
		protected Block(IEnumerable<Node> children)
		{
			if (children == null) {
				this.children = new List<Node>();
			}
			else {
				this.children = new List<Node>(children);
			}
		}
	}
}
