using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// A jump node jumps to a label in a script.
	/// </summary>
	public class Jump : Node
	{
		public readonly string to;

		/// <summary>
		/// Creates a new jump node.
		/// </summary>
		/// <param name="to">The name of the label to jump to.</param>
		public Jump(string to)
		{
			this.to = to;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.JumpToLabel(to);
			yield return null;
		}
	}
}
