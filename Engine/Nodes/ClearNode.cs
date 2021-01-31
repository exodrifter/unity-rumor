using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class ClearNode : Node
	{
		public ClearType Type { get; }

		public ClearNode(ClearType type)
		{
			Type = type;
		}

		public override Yield Execute(Rumor rumor)
		{
			rumor.State.Clear(Type);
			return null;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ClearNode);
		}

		public bool Equals(ClearNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Type == other.Type;
		}

		public override int GetHashCode()
		{
			return Type.GetHashCode();
		}

		public override string ToString()
		{
			switch (Type)
			{
				case ClearType.All:
					return "clear all";
				case ClearType.Choices:
					return "clear choices";
				case ClearType.Dialog:
					return "clear dialog";

				default:
					return "clear " + Type;
			}
		}
	}
}
