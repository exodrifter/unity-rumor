using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class JumpNode : Node
	{
		public string Label { get; }

		public JumpNode(string label)
		{
			Label = label;
		}

		public override Yield Execute(Rumor rumor)
		{
			rumor.Jump(Label);
			return null;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as JumpNode);
		}

		public bool Equals(JumpNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Label == other.Label;
		}

		public override int GetHashCode()
		{
			return Label.GetHashCode();
		}

		public override string ToString()
		{
			return "jump " + Label;
		}
	}
}
