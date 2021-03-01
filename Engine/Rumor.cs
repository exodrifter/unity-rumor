using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// A Rumor is the class used to execute a Rumor script.
	/// </summary>
	[Serializable]
	public class Rumor : ISerializable
	{
		/// <summary>
		/// The identifier used for the main entry point of a script.
		/// </summary>
		public const string MainIdentifier = "_main";

		/// <summary>
		/// The script that is being executed.
		/// </summary>
		private Dictionary<string, List<Node>> Nodes { get; }

		/// <summary>
		/// The labels that exist in this Rumor. These labels can be jumped to
		/// using <see cref="Jump(string)"/>.
		/// </summary>
		public IEnumerable<string> Identifiers => Nodes.Keys;

		public RumorState State { get; }
		public RumorScope Scope { get; }
		public RumorBindings Bindings { get; }

		public Rumor(Dictionary<string, List<Node>> nodes, RumorScope scope = null)
		{
			// Make a copy so our version does not change
			Nodes = new Dictionary<string, List<Node>>(nodes);
			Stack = new Stack<StackFrame>();
			State = new RumorState();
			Scope = scope ?? new RumorScope();
			Bindings = new RumorBindings();
		}

		#region Execution

		/// <summary>
		/// The current call stack.
		/// </summary>
		private Stack<StackFrame> Stack { get; }

		/// <summary>
		/// True if Rumor's call stack is not empty.
		/// </summary>
		public bool Executing => Stack.Count != 0;

		/// <summary>
		/// The number of times Rumor has run to completion. In other words, how
		/// many times the call stack was emptied under normal conditions.
		/// </summary>
		public int FinishCount { get; private set; }

		/// <summary>
		/// The number of times Rumor failed to run to completion. In other
		/// words, the number of times execution was stopped by calling
		/// <see cref="Stop"/> during execution or by calling
		/// <see cref="Start"/> again before execution finished.
		/// </summary>
		public int CancelCount { get; private set; }

		/// <summary>
		/// The current yield preventing further execution.
		/// </summary>
		private Yield Yield;

		public bool Choosing { get { return Yield is ForChoose; } }
		public bool Waiting { get { return Yield is ForAdvance; } }
		public bool Pausing { get { return Yield is ForSeconds; } }

		/// <summary>
		/// If non-negative, the amount of time that must elapse before the
		/// dialog is automatically advanced.
		/// </summary>
		private float AutoAdvance { get; set; } = -1;

		public event Action OnFinish;
		public event Action OnWaitForAdvance;
		public event Action<Dictionary<string, string>> OnWaitForChoose;
		public event Action<Dictionary<string, string>, double?> OnWaitForChooseTimeout;

		public void Start(string label = MainIdentifier)
		{
			if (Stack.Count > 0)
			{
				Stop();
			}

			Call(label);
			Continue();
		}

		/// <summary>
		/// Tells Rumor that <paramref name="delta"/> seconds have passed.
		/// </summary>
		/// <param name="delta">
		/// The amount of time in seconds that has passed since the last time
		/// this function was called.
		/// </param>
		public void Update(double delta)
		{
			Yield?.Update(this, delta);
			Continue();
		}

		/// <summary>
		/// This should be called after any operation that may allow execution
		/// to continue.
		/// </summary>
		private void Continue()
		{
			while (Stack.Count > 0)
			{
				// Check if the current yield is finished
				if (Yield != null)
				{
					if (Yield.Finished == true)
					{
						Yield = null;
					}

					// Auto-advance, if applicable
					else if (Yield is ForAdvance
						&& AutoAdvance >= 0 && Yield.Elapsed >= AutoAdvance)
					{
						Yield = null;
					}

					// Wait for the current yield to finish
					else
					{
						return;
					}
				}

				// Check if this frame is complete
				var frame = Stack.Peek();
				if (frame.Done)
				{
					Stack.Pop();
					continue;
				}

				// Execute the next node in the stack frame
				Yield = frame.Execute(this);
				if (Yield is ForAdvance && AutoAdvance != 0)
				{
					OnWaitForAdvance?.Invoke();
					return;
				}
				else if (Yield is ForChoose)
				{
					OnWaitForChoose?.Invoke(State.GetChoices());
					OnWaitForChooseTimeout?.Invoke(
						State.GetChoices(),
						(Yield as ForChoose).Timeout
					);
					return;
				}
				else if (Yield is ForSeconds)
				{
					return;
				}
			}

			OnFinish?.Invoke();
			FinishCount++;
		}

		/// <summary>
		/// Clears the call stack.
		/// </summary>
		public void Stop()
		{
			CancelCount++;
			Stack.Clear();
		}

		#endregion

		#region Controls

		/// <summary>
		/// Attempts to advances execution of the script. This should be used
		/// whenever the player presses an input to advance the state of the
		/// dialog.
		/// </summary>
		public void Advance()
		{
			Yield?.Advance();
			Continue();
		}

		/// <summary>
		/// Sets the auto-advance setting.
		/// </summary>
		/// <param name="value">
		/// A negative value to stop automatically advancing the dialog, or a
		/// positive number indicating the number of seconds that must elapse
		/// before automatically advancing the dialog.
		/// </param>
		public void SetAutoAdvance(float value)
		{
			AutoAdvance = value;
			Continue();
		}

		/// <summary>
		/// Pushes a labeled list of nodes as a new stack frame onto the call
		/// stack.
		/// </summary>
		/// <param name="label">
		/// The label of the list of nodes to jump execution to.
		/// </param>
		public void Call(string label)
		{
			if (!Nodes.ContainsKey(label))
			{
				throw new InvalidOperationException(
					"The label \"" + label + "\" does not exist!"
				);
			}

			Stack.Push(new StackFrame(Nodes[label]));
		}

		public void Choose(string label)
		{
			if (State.GetChoices().ContainsKey(label))
			{
				Yield?.Choose();
				Jump(label);
				State.ClearChoices();
				Continue();
			}
		}

		/// <summary>
		/// Pushes a list of nodes as a new stack frame onto the call stack.
		/// </summary>
		/// <param name="label">
		/// The list of nodes to push onto the call stack.
		/// </param>
		public void Inject(List<Node> nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException(nameof(nodes));
			}

			Stack.Push(new StackFrame(nodes));

			Yield = null;
			Continue();
		}

		/// <summary>
		/// Pops the current stack frame and pushes a labeled list of nodes as a
		/// new stack frame onto the call stack.
		/// </summary>
		/// <param name="label">
		/// The label of the list of nodes to jump execution to.
		/// </param>
		public void Jump(string label)
		{
			if (!Nodes.ContainsKey(label))
			{
				throw new InvalidOperationException(
					"The label \"" + label + "\" does not exist!"
				);
			}

			Stack.Pop();
			Stack.Push(new StackFrame(Nodes[label]));
		}

		/// <summary>
		/// Removes the top-most stack frame from the call stack.
		/// </summary>
		public void Return()
		{
			Stack.Pop();
		}

		#endregion

		#region Serialization

		public Rumor(SerializationInfo info, StreamingContext context)
		{
			Nodes = info.GetValue<Dictionary<string, List<Node>>>("nodes");
			State = info.GetValue<RumorState>("state");
			Scope = info.GetValue<RumorScope>("scope");
			Stack = info.GetValue<Stack<StackFrame>>("stack");
			FinishCount = info.GetValue<int>("finishCount");
			CancelCount = info.GetValue<int>("cancelCount");
			Yield = info.GetValue<Yield>("yield");
			AutoAdvance = info.GetValue<float>("autoAdvance");

			Bindings = new RumorBindings();
		}

		public void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Dictionary<string, List<Node>>>("nodes", Nodes);
			info.AddValue<RumorState>("state", State);
			info.AddValue<RumorScope>("scope", Scope);
			info.AddValue<Stack<StackFrame>>("stack", Stack);
			info.AddValue<int>("finishCount", FinishCount);
			info.AddValue<int>("cancelCount", CancelCount);
			info.AddValue<Yield>("yield", Yield);
			info.AddValue<float>("autoAdvance", AutoAdvance);
		}

		#endregion
	}
}