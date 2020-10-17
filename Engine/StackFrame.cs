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
		/// The iterator pointing to the current execution position.
		/// </summary>
		private IEnumerator<Yield> Iterator { get; set; } = null;

		/// <summary>
		/// The current yield preventing further execution of the stack frame.
		/// </summary>
		internal Yield Yield { get { return Iterator?.Current; } }

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
		/// Starts or resumes execution of this call stack.
		/// </summary>
		/// <param name="rumor"></param>
		/// <returns></returns>
		internal IEnumerator Execute(Rumor rumor)
		{
			while (true)
			{
				if (Iterator == null)
				{
					if (NextIndex < Nodes.Count)
					{
						var node = Nodes[NextIndex];
						NextIndex++;
						Iterator = node.Execute(rumor);
					}
					else
					{
						// No more nodes to execute
						yield break;
					}
				}

				// Execute the next node
				while (Iterator.MoveNext())
				{
					// Wait for the yield to finish
					while (!Iterator.Current.Finished)
					{
						yield return null;
					}
				}
				Iterator = null;
			}
		}
	}
}