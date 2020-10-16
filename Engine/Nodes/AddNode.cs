namespace Exodrifter.Rumor.Engine
{
	public class AddNode : DialogNode
	{
		public AddNode(string speaker, string dialog)
			: base(speaker, dialog) { }

		public AddNode(string speaker, Expression<StringValue> dialog)
			: base(speaker, dialog) { }

		public override bool Equals(object obj)
		{
			return Equals(obj as AddNode);
		}

		public bool Equals(AddNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Speaker == other.Speaker
				&& Dialog == other.Dialog;
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Speaker, Dialog);
		}

		public override string ToString()
		{
			return Speaker + "+ " + Dialog;
		}
	}
}
