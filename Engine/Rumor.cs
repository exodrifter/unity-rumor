using System;
using System.Collections;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// A Rumor is the class used to execute a Rumor script.
	/// </summary>
	public class Rumor
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

		public event Action OnFinish;
		public event Action OnWaitForAdvance;
		public event Action<Dictionary<string, string>> OnWaitForChoose;

		public void Start(string label = MainIdentifier)
		{
			if (Stack.Count > 0)
			{
				Stop();
			}

			Jump(label);
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
					if (Yield?.Finished == true)
					{
						Yield = null;
					}
					else
					{
						// Wait for the current yield to finish
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
				if (Yield is ForAdvance)
				{
					OnWaitForAdvance?.Invoke();
					return;
				}
				else if (Yield is ForChoose)
				{
					OnWaitForChoose?.Invoke(State.GetChoices());
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
		}

		/// <summary>
		/// Pushes a labeled list of nodes as a new stack frame onto the call
		/// stack.
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

			Stack.Push(new StackFrame(Nodes[label]));
		}

		/// <summary>
		/// Removes the top-most stack frame from the call stack.
		/// </summary>
		public void Return()
		{
			Stack.Pop();
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
			Yield?.Update(delta);
			Continue();
		}

		#endregion
	}
}