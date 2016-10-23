using Exodrifter.Rumor.Nodes;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Runs and represents the state of a stack frame in a Rumor.
	/// </summary>
	[Serializable]
	public sealed class StackFrame : ISerializable
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
		/// The next node or null if the stack frame is about to be exhausted.
		/// </summary>
		internal Node Next
		{
			get
			{
				if (0 <= index && index < nodes.Count) {
					return nodes[index];
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
		/// An event that is called right before a new node is executed.
		/// </summary>
		public event Action<Node> OnNextNode;

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
		/// Creates a copy of an existing stack frame.
		/// </summary>
		/// <param name="frame">
		/// The stack frame to copy.
		/// </param>
		internal StackFrame(StackFrame frame)
		{
			this.nodes = frame.nodes;
			this.index = frame.index;
		}

		/// <summary>
		/// Moves the frame's index pointer back to the beginning.
		/// </summary>
		internal void Reset()
		{
			index = 0;
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
			if (index < nodes.Count) {
				var yield = nodes[index].Run(rumor);

				if (OnNextNode != null) {
					OnNextNode(nodes[index]);
				}

				++index;
				while (yield.MoveNext()) {
					yield return yield.Current;
				}
			}
		}

		/// <summary>
		/// Returns true if this stack frame has a label with the specified
		/// name.
		/// </summary>
		/// <param name="name">The name of the label to find.</param>
		/// <returns>True if the label exists.</returns>
		internal bool HasLabel(string name)
		{
			foreach (var node in nodes) {
				var label = node as Label;
				if (label != null && label.name == name) {
					return true;
				}
			}

			return false;
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

		/// <summary>
		/// Returns the children of the specified label. Returns an empty list
		/// if the label does not exist.
		/// </summary>
		/// <param name="name">
		/// The name of the label to get the children of.
		/// </param>
		internal IEnumerable<Node> GetChildrenOfLabel(string name) {
			for (int i = 0; i < nodes.Count; ++i) {
				var label = nodes[i] as Label;

				if (label != null && label.name == name) {
					return label.Children;
				}
			}

			return new List<Node>();
		}

		#region Serialization

		public StackFrame(SerializationInfo info, StreamingContext context)
		{
			nodes = info.GetValue<List<Node>>("nodes");
			index = info.GetValue<int>("index");
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<List<Node>>("nodes", nodes);
			info.AddValue<int>("index", index);
		}

		#endregion
	}
}
