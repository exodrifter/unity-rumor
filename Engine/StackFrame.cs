using System.Collections;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Contains a pointer to a list of nodes that are currently being
	/// executed in Rumor.
	/// </summary>
	internal sealed class StackFrame
	{
		/// <summary>
		/// The list of nodes in this stack frame.
		/// </summary>
		private List<Node> Nodes { get; }

		/// <summary>
		/// The index of the next node to execute.
		/// </summary>
		private int NextIndex { get; set; }

		/// <summary>
		/// True if all nodes have been executed.
		internal bool Done { get { return NextIndex >= Nodes.Count; } }

		/// <summary>
		/// Creates a new stack frame.
		/// </summary>
		/// <param name="nodes">
		/// The list of nodes to use in this stack frame.
		/// </param>
		internal StackFrame(List<Node> nodes)
		{
			// Make a copy so our version does not change
			Nodes = new List<Node>(nodes);
			NextIndex = 0;
		}

		/// <summary>
		/// Executes the next node in the stack frame.
		/// </summary>
		/// <param name="rumor">The rumor executing this stack frame.</param>
		/// <returns>The yield preventing further execution, if any.</returns>
		internal Yield Execute(Rumor rumor)
		{
			var node = Nodes[NextIndex];
			NextIndex++;

			return node.Execute(rumor);
		}
	}
}
