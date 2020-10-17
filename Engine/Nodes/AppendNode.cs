using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class AppendNode : DialogNode
	{
		public AppendNode(string speaker, string dialog)
			: base(speaker, dialog) { }

		public AppendNode(string speaker, Expression<StringValue> dialog)
			: base(speaker, dialog) { }

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			// TODO: Implement
			yield break;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as AppendNode);
		}

		public bool Equals(AppendNode other)
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
