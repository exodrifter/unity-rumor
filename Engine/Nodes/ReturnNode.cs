﻿namespace Exodrifter.Rumor.Engine
{
	public class ReturnNode
	{
		public ReturnNode() { }

		public override bool Equals(object obj)
		{
			return Equals(obj as ReturnNode);
		}

		public bool Equals(ReturnNode other)
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
			return "return";
		}
	}
}
