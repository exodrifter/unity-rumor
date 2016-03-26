using Exodrifter.Rumor.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Runs and represents the state of a Rumor.
	/// </summary>
	public sealed class Rumor
	{
		private IEnumerator<Node> nodes;
		private IEnumerator<RumorYield> yield;

		/// <summary>
		/// The current node.
		/// </summary>
		public Node Current { get { return nodes.Current; } }

		/// <summary>
		/// True if the script has been started.
		/// </summary>
		public bool Started { get; private set; }

		/// <summary>
		/// True if the script has finished.
		/// </summary>
		public bool Finished { get; private set; }

		/// <summary>
		/// Creates a new Rumor.
		/// </summary>
		/// <param name="nodes">The nodes to use in the rumor script.</param>
		public Rumor(IEnumerable<Node> nodes)
		{
			this.nodes = nodes.GetEnumerator();
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

			Started = true;
			Finished = false;
			nodes.Reset();

			while (nodes.MoveNext()) {

				yield = nodes.Current.Run();

				while (yield.MoveNext()) {
					while (!yield.Current.Finished) {
						yield return null;
					}
				}
			}

			yield = null;
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
	}
}
