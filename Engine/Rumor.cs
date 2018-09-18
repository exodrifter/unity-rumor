using Exodrifter.Rumor.Language;
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
		/// The bindings.
		/// </summary>
		public Bindings Bindings { get { return bindings; } }
		private readonly Bindings bindings;

		/// <summary>
		/// The current yield.
		/// </summary>
		private IEnumerator<RumorYield> iter;

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
		/// Returns the contents of the last choice that was chosen. Returns
		/// null if no choices have been chosen yet.
		/// </summary>
		public string LastChoice { get; set; }

		/// <summary>
		/// Returns true if we are currently on a choose node.
		/// </summary>
		public bool Choosing
		{
			get
			{
				if (iter == null || iter.Current == null) {
					return false;
				}
				return iter.Current.GetType() == typeof(ForChoice);
			}
		}

		/// <summary>
		/// Returns a float indicating the amount of time left in seconds the
		/// user has to choose a choice. If the user has an unlimited amount of
		/// time, this will return null.
		/// </summary>
		public float? SecondsLeftToChoose
		{
			get
			{
				var yield = iter.Current as ForChoice;
				if (yield == null) {
					return null;
				}
				return yield.SecondsLeft;
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
		/// True if the script was cancelled.
		/// </summary>
		public bool Cancelled { get; private set; }

		/// <summary>
		/// True if the script is running.
		/// </summary>
		public bool Running { get { return Started && !(Finished || Cancelled); } }

		/// <summary>
		/// If positive, the amount of time in seconds before the script should
		/// attempt to automatically advance.
		/// </summary>
		public float AutoAdvance { get; set; }

		/// <summary>
		/// An event that is called right before a new node is executed.
		/// </summary>
		public event Action<Node> OnNextNode;

		/// <summary>
		/// An event for when a choice needs to be made.
		/// </summary>
		public event Action<List<string>, float?> OnWaitForChoose;

		/// <summary>
		/// An event for when an advance needs to be made.
		/// </summary>
		public event Action OnWaitForAdvance;

		/// <summary>
		/// An event that is called when the Rumor is starts executing.
		/// </summary>
		public event Action OnStart;

		/// <summary>
		/// An event that is called when the Rumor is finished executing.
		/// </summary>
		public event Action OnFinish;

		/// <summary>
		/// An event that is called when the Rumor is execution is cancelled.
		/// </summary>
		public event Action OnCancel;

		/// <summary>
		/// The number of times this rumor has been finished.
		/// </summary>
		public int FinishCount { get; set; }

		/// <summary>
		/// The number of times this rumor has been cancelled.
		/// </summary>
		public int CancelCount { get; set; }

		private Rumor()
		{
			this.stack = new Stack<StackFrame>();
			this.scope = new Scope();
			this.bindings = new Bindings();

			State = new RumorState();
			Started = false;
			Finished = false;
			Cancelled = false;
			AutoAdvance = -1;
		}

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="nodes">The nodes to use in the rumor.</param>
		/// <param name="scope">The scope to store data in.</param>
		/// <param name="bindings">The bindings to use.</param>
		public Rumor(IEnumerable<Node> nodes, Scope scope = null, Bindings bindings = null)
			: this()
		{
			this.nodes = new List<Node>(nodes);
			this.scope = scope ?? new Scope();
			this.bindings = bindings ?? new Bindings();
		}

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="script">The script to use in the rumor.</param>
		/// <param name="scope">The scope to store data in.</param>
		public Rumor(string script, Scope scope = null, Bindings bindings = null)
			: this()
		{
			this.nodes = new List<Node>(Compiler.Compile(script));
			this.scope = scope ?? new Scope();
			this.bindings = bindings ?? new Bindings();
		}

		/// <summary>
		/// Setup default bindings.
		/// </summary>
		public void SetupDefaultBindings()
		{
			bindings.Bind("_auto_advance", (float seconds) => { AutoAdvance = seconds; });
			bindings.Bind("_cancel_count", () => { return CancelCount; });
			bindings.Bind("_finish_count", () => { return FinishCount; });
			bindings.Bind("_choice", () => { return LastChoice; });

			bindings.Bind("_set_default_speaker", (object speaker) => { Scope.DefaultSpeaker = speaker; });
		}

		/// <summary>
		/// Starts execution of the Rumor. Note that this does not actually
		/// run the Rumor, but instead returns an IEnumerator that can be used
		/// to continue execution. This method cannot be used to spawn
		/// multiple instances of the same rumor script; create multiple
		/// Rumor instances instead if that behaviour is desired. If execution
		/// has not finished, the Start method will restart the script.
		/// 
		/// You can use this method in Unity by passing the return value to
		/// the StartCoroutine method.
		/// </summary>
		/// <returns>
		/// Returns a IEnumerator that can be used to run the Rumor. The
		/// IEnumerator will return true for MoveNext until the Rumor has
		/// terminated.
		/// </returns>
		public IEnumerator Start()
		{
			if (Running) {
				stack.Clear();
			}

			// If the stack is empty, this is a new game
			if (stack.Count == 0) {
				Init();
			}

			// Attach listeners for OnNextNode
			foreach (var stackFrame in stack) {
				stackFrame.OnNextNode += OnNextNodeHandler;
			}

			Started = true;
			Finished = false;
			Cancelled = false;

			if (OnStart != null) {
				OnStart();
			}

			while (stack.Count > 0 && !(Finished || Cancelled)) {
				var yield = ExecuteStack();
				while (yield.MoveNext() && !(Finished || Cancelled)) {
					if (yield.Current == true) {
						yield return null;
					}
				}
			}

			Finish();
		}

		private IEnumerator<bool> ExecuteStack()
		{
			// Check if the stack frame is exhausted
			if (stack.Peek().Finished) {
				stack.Pop();
				yield break;
			}

			// Execute the next statement
			var origStack = stack.Peek();
			var origIter = origStack.Run(this);
			iter = origIter;
			while (origIter.MoveNext() && !(Finished || Cancelled)) {
				var trigger = true;
				var yield = origIter.Current;
				while (yield != null && !(Finished || Cancelled)) {
					if (AutoAdvance >= 0 && AutoAdvance <= yield.Elapsed) {
						Advance();
					}

					// Check if we need to retrigger any events
					if (trigger) {
						if (yield is ForChoice && OnWaitForChoose != null) {
							OnWaitForChoose(State.Choices, SecondsLeftToChoose);
						}
						if (yield is ForAdvance && OnWaitForAdvance != null) {
							// We might have just auto-advanced
							if (!yield.Finished) {
								OnWaitForAdvance();
							}
						}
						trigger = false;
					}

					// Check for stack additions
					while (stack.Count != 0 && stack.Peek() != origStack) {
						trigger = true;
						var otherYield = ExecuteStack();
						while (otherYield.MoveNext() && !(Finished || Cancelled)) {
							if (otherYield.Current == true) {
								yield return true;
							}
						}
					}
					iter = origIter;

					// Check only after we finished executing the stack
					if (yield.Finished) {
						break;
					}

					yield return true;
				}
			}

			if (stack.Count > 0 && stack.Peek().Finished) {
				stack.Pop();
			}
		}

		private void Init()
		{
			stack.Push(new StackFrame(nodes));
			State.Clear();
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
			if (null == iter) {
				return;
			}

			if (null != iter.Current) {
				iter.Current.OnUpdate(this, delta);
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
			if (null == iter) {
				return;
			}

			if (null != iter.Current) {
				iter.Current.OnAdvance();
			}
		}

		/// <summary>
		/// Forces the script to immediately finish.
		/// </summary>
		public void Finish()
		{
			if (!Running) {
				return;
			}

			State.Clear();
			Finished = true;

			FinishCount++;

			if (OnFinish != null) {
				OnFinish();
			}
		}

		/// <summary>
		/// Cancels the rumor script, as if the user cancelled the dialog.
		/// </summary>
		public void Cancel()
		{
			if (!Running) {
				return;
			}

			State.Clear();
			Cancelled = true;

			CancelCount++;

			if (OnCancel != null) {
				OnCancel();
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
				LastChoice = State.Choices[index];
				State.RemoveChoice(index);
			}

			if (null == iter) {
				return;
			}

			if (null != iter.Current) {
				iter.Current.OnChoice();
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
		public void JumpToLabel(string label)
		{
			if (!Started || !Running) {
				Init();
			}

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
		public void CallLabel(string label)
		{
			if (!Started || !Running) {
				Init();
			}

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

		public void Inject(IEnumerable<Node> nodes)
		{
			stack.Push(new StackFrame(nodes));
		}

		#region Serialization

		public Rumor(SerializationInfo info, StreamingContext context)
		{
			nodes = info.GetValue<List<Node>>("nodes");
			stack = info.GetValue<Stack<StackFrame>>("stack");
			scope = info.GetValue<Scope>("scope");

			State = info.GetValue<RumorState>("state");

			AutoAdvance = info.GetValue<float>("autoAdvance");

			FinishCount = info.GetValue<int>("finishCount");
			CancelCount = info.GetValue<int>("cancelCount");
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<List<Node>>("nodes", nodes);
			info.AddValue<Stack<StackFrame>>("stack", stack);
			info.AddValue<Scope>("scope", scope);

			info.AddValue<RumorState>("state", State);

			info.AddValue<float>("autoAdvance", AutoAdvance);

			info.AddValue<int>("finishCount", FinishCount);
			info.AddValue<int>("cancelCount", CancelCount);
		}

		#endregion
	}
}
