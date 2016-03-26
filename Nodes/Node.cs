using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	public abstract class Node
	{
		/// <summary>
		/// Runs the current node.
		/// </summary>
		/// <returns>
		/// A IEnumerator that can be used to continue execution of the node.
		/// </returns>
		public abstract IEnumerator<RumorYield> Run();
	}
}
