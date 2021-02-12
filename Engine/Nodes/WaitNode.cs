using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class WaitNode : Node
	{
		public WaitNode() { }

		public override Yield Execute(Rumor rumor)
		{
			return new ForAdvance();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as WaitNode);
		}

		public bool Equals(WaitNode other)
		{
			if (other == null)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			return "wait";
		}
	}
}
