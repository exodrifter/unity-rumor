using Exodrifter.Rumor.Nodes;
using Exodrifter.Rumor.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Runs and represents the state of a Rumor.
	/// </summary>
	[Serializable]
	public sealed partial class Rumor : ISerializable
	{
		/// <summary>
		/// The nodes in this Rumor.
		/// </summary>
		private readonly List<Node> nodes;

		/// <summary>
		/// The current call stack.
		/// </summary>
		private readonly Stack<StackFrame> stack;

		/// <summary>
		/// The current scope.
		/// </summary>
		public Scope Scope { get { return scope; } }
		private readonly Scope scope;

		/// <summary>
		/// The current yield.
		/// </summary>
		private IEnumerator<RumorYield> yield;

		/// <summary>
		/// The current node.
		/// </summary>
		public Node Current
		{
			get
			{
				if (stack.Count > 0) {
					return stack.Peek().Current;
				}
				return null;
			}
		}

		/// <summary>
		/// The next node.
		/// </summary>
		public Node Next
		{
			get
			{
				if (stack.Count > 0) {
					return stack.Peek().Next;
				}
				return null;
			}
		}

		/// <summary>
		/// The state of the script.
		/// </summary>
		public RumorState State { get; private set; }

		/// <summary>
		/// True if the script has been started.
		/// </summary>
		public bool Started { get; private set; }

		/// <summary>
		/// True if the script has finished.
		/// </summary>
		public bool Finished { get; private set; }

		/// <summary>
		/// An event that is called right before a new node is executed.
		/// </summary>
		public event Action<Node> OnNextNode;

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="nodes">The nodes to use in the rumor script.</param>
		/// <param name="state">The state to store data in.</param>
		public Rumor(IEnumerable<Node> nodes)
		{
			this.nodes = new List<Node>(nodes);
			this.stack = new Stack<StackFrame>();
			this.scope = new Scope();

			State = new RumorState();
			Started = false;
			Finished = false;
		}

		/// <summary>
		/// Starts execution of the Rumor. Note that this does not actually
		/// run the Rumor, but instead returns an IEnumerator that can be used
		/// to continue execution. This method cannot be used to spawn
		/// multiple instances of the same rumor script; create multiple
		/// Rumor instances instead if that behaviour is desired. After
		/// execution has finished, the Run method may be called again.
		/// 
		/// You can use this method in Unity by passing the return value to
		/// the StartCoroutine method.
		/// </summary>
		/// <returns>
		/// Returns a IEnumerator that can be used to run the Rumor. The
		/// IEnumerator will return true for MoveNext until the Rumor has
		/// terminated.
		/// </returns>
		public IEnumerator Run()
		{
			if (Started && !Finished) {
				throw new InvalidOperationException(
					"The rumor has not finished execution yet.");
			}

			// If the stack is empty, this is a new game
			if (stack.Count == 0) {
				stack.Push(new StackFrame(nodes));
				State.Reset();
			}

			// Attach listeners for OnNextNode
			foreach (var stackFrame in stack) {
				stackFrame.OnNextNode += OnNextNodeHandler;
			}

			Started = true;
			Finished = false;

			while (stack.Count > 0) {

				// Check if the stack frame is exhausted
				if (stack.Peek().Finished) {
					stack.Pop();
					continue;
				}

				// Execute the next statement
				yield = stack.Peek().Run(this);
				while (yield.MoveNext()) {
					while (yield.Current != null && !yield.Current.Finished) {
						yield return null;
					}
				}
			}

			// Reset the state when we are finished
			scope.ClearVars();
			State.Reset();
			Finished = true;
		}

		/// <summary>
		/// Updates the state of the rumor script. This should be called from
		/// the main update loop of your game.
		/// 
		/// This may sometimes cause the script to advance.
		/// </summary>
		/// <param name="delta">
		/// The amount of time in seconds since Update was last called.
		/// </param>
		public void Update(float delta)
		{
			if (null == yield) {
				return;
			}

			if (null != yield.Current) {
				yield.Current.OnUpdate(delta);
			}
		}

		/// <summary>
		/// Requests the rumor script to advance.
		/// 
		/// This might not always cause the script to advance, as some nodes
		/// may decide to ignore this event.
		/// </summary>
		public void Advance()
		{
			if (null == yield) {
				return;
			}

			if (null != yield.Current) {
				yield.Current.OnAdvance();
			}
		}

		/// <summary>
		/// Choose the choice at the specified index.
		/// </summary>
		/// <param name="index">The index of the choice to pick.</param>
		public void Choose(int index)
		{
			if (0 <= index && index < State.Consequences.Count) {
				EnterBlock(State.Consequences[index]);
				State.ClearChoices();
			}

			if (null == yield) {
				return;
			}

			if (null != yield.Current) {
				yield.Current.OnChoice();
			}
		}

		/// <summary>
		/// Pushes a new stack frame onto the stack.
		/// </summary>
		/// <param name="nodes">
		/// The nodes to use in the new stack frame.
		/// </param>
		internal void EnterBlock(IEnumerable<Node> nodes)
		{
			var frame = new StackFrame(nodes);

			// Ignore empty frames
			if (frame.Finished) {
				return;
			}

			frame.OnNextNode += OnNextNodeHandler;
			stack.Push(frame);
		}

		/// <summary>
		/// Pops the top-most stack frame from the stack.
		/// </summary>
		internal void ExitBlock()
		{
			stack.Pop();
		}

		/// <summary>
		/// Jumps execution to a label with the specified name that is closest
		/// to the top of the stack.
		/// </summary>
		/// <param name="label">
		/// The name of the label to jump to.
		/// </param>
		internal void JumpToLabel(string label)
		{
			while (stack.Count > 0) {
				if (stack.Peek().JumpToLabel(label)) {
					return;
				}
				stack.Pop();
			}

			throw new InvalidOperationException(
				"Label \"" + label + "\" cannot be found");
		}

		/// <summary>
		/// Moves execution to a label with the specified name that is closest
		/// to the top of the stack.
		/// </summary>
		/// <param name="label">move to.
		/// </param>
		internal void MoveToLabel(string label)
		{
			StackFrame frameContainingLabel = null;
			foreach (var frame in stack) {
				if (frame.HasLabel(label)) {
					frameContainingLabel = frame;
					break;
				}
			}

			if (frameContainingLabel == null) {
				throw new InvalidOperationException(
					"Label \"" + label + "\" cannot be found");
			}

			EnterBlock(frameContainingLabel.GetChildrenOfLabel(label));
		}

		internal void OnNextNodeHandler(Node node)
		{
			if (OnNextNode != null && node != null) {
				OnNextNode(node);
			}
		}

		#region Serialization

		public Rumor(SerializationInfo info, StreamingContext context)
		{
			nodes = info.GetValue<List<Node>>("nodes");
			stack = info.GetValue<Stack<StackFrame>>("stack");
			scope = info.GetValue<Scope>("scope");

			State = info.GetValue<RumorState>("state");
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<List<Node>>("nodes", nodes);
			info.AddValue<Stack<StackFrame>>("stack", stack);
			info.AddValue<Scope>("scope", scope);

			info.AddValue<RumorState>("state", State);
		}

		#endregion
	}
}
