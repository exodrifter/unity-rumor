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
		public Rumor(IEnumerable<Node> nodes)
			: this()
		{
			this.nodes = new List<Node>(nodes);
		}

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="nodes">The nodes to use in the rumor.</param>
		/// <param name="scope">The scope to store data in.</param>
		public Rumor(IEnumerable<Node> nodes, Scope scope)
			: this()
		{
			this.nodes = new List<Node>(nodes);
			this.scope = scope ?? new Scope();
		}

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="script">The script to use in the rumor.</param>
		public Rumor(string script)
			: this()
		{
			this.nodes = Compiler.Compile(script);
		}

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="script">The script to use in the rumor.</param>
		/// <param name="scope">The scope to store data in.</param>
		public Rumor(string script, Scope scope)
			: this()
		{
			this.nodes = new List<Node>(Compiler.Compile(script));
			this.scope = scope;
		}

		/// <summary>
		/// Setup default bindings.
		/// </summary>
		public void SetupDefaultBindings()
		{
			Bind("_auto_advance", (float seconds) => { AutoAdvance = seconds; });
			Bind("_cancel_count", () => { return CancelCount; });
			Bind("_finish_count", () => { return FinishCount; });
			Bind("_choice", () => { return LastChoice; });

			Bind("_set_default_speaker", (object speaker) => { Scope.DefaultSpeaker = speaker; });
		}

		/// <summary>
		/// Starts execution of the Rumor. Note that this does not actually
		/// run the Rumor, but instead returns an IEnumerator that can be used
		/// to continue execution. This method cannot be used to spawn
		/// multiple instances of the same rumor script; create multiple
		/// Rumor instances instead if that behaviour is desired. If execution
		/// has not finished, the Run method will restart the script.
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

		#region Bindings

		/// <summary>
		/// Stores functions, methods, or constructors.
		/// </summary>
		private Dictionary<string, RumorBinding> bindings;

		/// <summary>
		/// Bind a <see cref="Action"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The action to bind.</param>
		public void Bind(string name, Action action)
		{
			AddBinding(name, 0, new BindingAction(action));
		}

		/// <summary>
		/// Bind a <see cref="Action{T}"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The <see cref="Action{T}"/> to bind.</param>
		public void Bind<T1>(string name, Action<T1> action)
		{
			AddBinding(name, 1, new BindingAction<T1>(action));
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2}"/> to the Rumor metatable so it
		/// can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2}"/> to bind.
		/// </param>
		public void Bind<T1, T2>(string name, Action<T1, T2> action)
		{
			AddBinding(name, 2, new BindingAction<T1, T2>(action));
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2, T3}"/> to the Rumor metatable so
		/// it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2, T3}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3>
			(string name, Action<T1, T2, T3> action)
		{
			AddBinding(name, 3, new BindingAction<T1, T2, T3>(action));
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2, T3, T4}"/> to the Rumor metatable
		/// so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2, T3, T4}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3, T4>
			(string name, Action<T1, T2, T3, T4> action)
		{
			AddBinding(name, 4, new BindingAction<T1, T2, T3, T4>(action));
		}

		/// <summary>
		/// Bind a <see cref="Func{TResult}"/> to the Rumor metatable so it can
		/// be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{TResult}"/> to bind.
		/// </param>
		public void Bind<TResult>(string name, Func<TResult> func)
		{
			AddBinding(name, 0, new BindingFunc<TResult>(func));
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, TResult>(string name, Func<T1, TResult> func)
		{
			AddBinding(name, 1, new BindingFunc<T1, TResult>(func));
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, T2, TResult>
			(string name, Func<T1, T2, TResult> func)
		{
			AddBinding(name, 2, new BindingFunc<T1, T2, TResult>(func));
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, T3, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, T3, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3, TResult>
			(string name, Func<T1, T2, T3, TResult> func)
		{
			AddBinding(name, 3, new BindingFunc<T1, T2, T3, TResult>(func));
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, T3, T4, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, T3, T4, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3, T4, TResult>
			(string name, Func<T1, T2, T3, T4, TResult> func)
		{
			AddBinding(name, 4, new BindingFunc<T1, T2, T3, T4, TResult>(func));
		}

		/// <summary>
		/// Add a binding.
		/// </summary>
		/// <param name="name">The name associate with the binding.</param>
		/// <param name="paramCount">
		/// The number of parameters in the binding.
		/// </param>
		/// <param name="binding">The binding to use.</param>
		private void AddBinding(string name, int paramCount, RumorBinding binding)
		{
			if (binding == null) {
				throw new ArgumentNullException();
			}

			bindings = bindings ?? new Dictionary<string, RumorBinding>();

			var mungedName = MungeName(name, paramCount);
			if (bindings.ContainsKey(mungedName))
			{
				var paramStr = paramCount == 1 ? "parameter" : "parameters";

				throw new InvalidOperationException(string.Format(
					"A binding \"{0}\" with {1} {2} is already in use!",
					name, paramCount, paramStr));
			}

			bindings.Add(mungedName, binding);
		}

		/// <summary>
		/// Call a binding with the specified name and arguments
		/// </summary>
		/// <param name="name">The name of the binding to call.</param>
		/// <param name="p">The arguments to pass to the binding.</param>
		/// <returns>The result of calling the binding.</returns>
		public object CallBinding(string name, params object[] p)
		{
			bindings = bindings ?? new Dictionary<string, RumorBinding>();

			var mungedName = MungeName(name, p.Length);
			if (!bindings.ContainsKey(mungedName))
			{
				var paramStr = p.Length == 1 ? "parameter" : "parameters";

				throw new InvalidOperationException(string.Format(
					"No binding of the name \"{0}\" with {1} {2} exists!",
					name, p.Length, paramStr));
			}

			return bindings[mungedName].Invoke(p);
		}

		/// <summary>
		/// Remove a binding.
		/// </summary>
		/// <param name="name">The name of the binding to remove.</param>
		/// <param name="paramCount">
		/// The number of parameters of the binding to remove.
		/// </param>
		public void RemoveBinding(string name, int paramCount)
		{
			var mungedName = MungeName(name, paramCount);
			bindings.Remove(mungedName);
		}

		/// <summary>
		/// Removes all of the bindings from this scope.
		/// </summary>
		public void ClearBindings()
		{
			bindings.Clear();
		}

		/// <summary>
		/// Munges a binding name.
		/// </summary>
		/// <param name="name">The binding name to munge.</param>
		/// <param name="paramCount">
		/// The number of parameters in the binding.
		/// </param>
		/// <returns>The munged name</returns>
		private string MungeName(string name, int paramCount)
		{
			return string.Format("{0}@{1}", name, paramCount);
		}

		#endregion

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
