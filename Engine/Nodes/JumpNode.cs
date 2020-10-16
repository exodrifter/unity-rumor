namespace Exodrifter.Rumor.Engine
{
	public class JumpNode
	{
		public string Identifier { get; }

		public JumpNode(string identifier)
		{
			Identifier = identifier;
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

			return Identifier == other.Identifier;
		}

		public override int GetHashCode()
		{
			return Identifier.GetHashCode();
		}

		public override string ToString()
		{
			return "jump " + Identifier;
		}
	}
}
