﻿using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class ChooseNode : Node
	{
		public ChooseNode() { }

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			// TODO: Implement
			yield break;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ChooseNode);
		}

		public bool Equals(ChooseNode other)
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
			return "choose";
		}
	}
}
