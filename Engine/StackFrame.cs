using Exodrifter.Rumor.Nodes;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Runs and represents the state of a stack frame in a Rumor.
	/// </summary>
	public sealed class StackFrame
	{
		/// <summary>
		/// The list of nodes in this stack frame.
		/// </summary>
		private readonly List<Node> nodes;

		/// <summary>
		/// The index of the next node.
		/// </summary>
		private int index;

		/// <summary>
		/// The current node or null if the stack frame is exhausted.
		/// </summary>
		internal Node Current
		{
			get
			{
				var i = index - 1;
				if (0 <= i && i < nodes.Count) {
					return nodes[i];
				}
				return null;
			}
		}

		/// <summary>
		/// Returns true if the stack frame is exhausted.
		/// </summary>
		public bool Finished
		{
			get { return index >= nodes.Count; }
		}

		/// <summary>
		/// Creates a new stack frame.
		/// </summary>
		/// <param name="nodes">
		/// The list of nodes in this stack frame.
		/// </param>
		internal StackFrame(IEnumerable<Node> nodes)
		{
			this.nodes = new List<Node>(nodes);
			this.index = 0;
		}

		/// <summary>
		/// Starts or resumes execution of the stack frame. Note that this
		/// does not actually run the stack frame, but instead returns an
		/// IEnumerator that can be used to wait on the completion of a yield
		/// that was returned by a node.
		/// 
		/// After the stack frame has been exhausted, calling this method
		/// again will return immediately.
		/// </returns>
		/// <param name="rumor">The rumor that owns this stack frame.</param>
		/// <returns>
		/// Returns a IEnumerator that can be used to run the current yield.
		/// The IEnumerator will return true for MoveNext until the yield has
		/// terminated.
		/// </returns>
		internal IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			while (index < nodes.Count) {
				var yield = nodes[index].Run(rumor);

				++index;
				if (yield.MoveNext()) {
					yield return yield.Current;
				}
			}
		}

		/// <summary>
		/// Moves the frame's index pointer so that the next Advance() call
		/// executes the specified label.
		/// </summary>
		/// <returns>True if the specified label was found.</returns>
		/// <param name="name">The name of the label to jump to.</param>
		internal bool JumpToLabel(string name)
		{
			for (int i = 0; i < nodes.Count; ++i) {
				var label = nodes[i] as Label;

				if (label != null && label.name == name) {
					index = i;
					return true;
				}
			}

			return false;
		}
	}
}
