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

		public event Action OnFinish;

		/// <summary>
		/// Clears the call stack and returns an iterator that the caller must
		/// use to execute this Rumor.
		///
		/// Note that this function doesn't actually do anything and that it
		/// will return immediately; the caller is required to manipulate the
		/// returned <see cref="IEnumerator"/> to execute Rumor. In Unity, this
		/// can be done by passing the <see cref="IEnumerator"/> to
		/// StartCoroutine.
		///
		/// This method cannot be used to spawn multiple execution instances;
		/// create multiple Rumor instances instead if that behaviour is
		/// desired.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="IEnumerator"/> that can be used to control
		/// the execution of Rumor.
		/// </returns>
		public IEnumerator Start(string label = MainIdentifier)
		{
			if (Stack.Count > 0)
			{
				Stop();
			}

			Jump(label);

			while (Stack.Count > 0)
			{
				// Continue executing this stack frame until it is either
				// finished or it is no longer the top-most stack frame.
				var frame = Stack.Peek();
				var iter = frame.Execute(this);
				while (iter.MoveNext()
					&& Stack.Count > 0
					&& Stack.Peek() == frame)
				{
					yield return null;
				}

				// Don't pop the stack if the stack frame we were executing is
				// no longer the top-most stack frame.
				if (Stack.Count > 0 && Stack.Peek() == frame)
				{
					Stack.Pop();
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
			if (Stack.Count > 0)
			{
				Stack.Peek().Yield?.Advance();
			}
		}

		public void Choose(string label)
		{
			if (State.GetChoices().ContainsKey(label))
			{
				if (Stack.Count > 0)
				{
					Stack.Peek().Yield?.Choose();
				}

				Jump(label);
				State.ClearChoices();
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
			if (Stack.Count > 0)
			{
				Stack.Peek().Yield?.Update(delta);
			}
		}

		#endregion
	}
}