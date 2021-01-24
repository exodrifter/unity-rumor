using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class SetDialogNode : DialogNode
	{
		public SetDialogNode(string speaker, string dialog)
			: base(speaker, dialog) { }

		public SetDialogNode(string speaker, Expression<StringValue> dialog)
			: base(speaker, dialog) { }

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			var dialog = Dialog.Evaluate(rumor.Scope).Value;
			rumor.State.SetDialog(Speaker, dialog);
			yield return new ForAdvance();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as SetDialogNode);
		}

		public bool Equals(SetDialogNode other)
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
			return Speaker + ": " + Dialog;
		}
	}
}
