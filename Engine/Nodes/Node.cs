using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public abstract class Node
	{
		public Node() { }

		public abstract IEnumerator<Yield> Execute(Rumor rumor);
	}
}
