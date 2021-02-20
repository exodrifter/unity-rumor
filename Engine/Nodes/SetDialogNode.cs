using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class SetDialogNode : DialogNode, ISerializable
	{
		public SetDialogNode(string speaker, string dialog)
			: base(speaker, dialog) { }

		public SetDialogNode(string speaker, Expression dialog)
			: base(speaker, dialog) { }

		public override Yield Execute(Rumor rumor)
		{
			var dialog = Dialog.Evaluate(rumor.Scope).AsString().Value;
			rumor.State.SetDialog(Speaker, dialog);
			return new ForAdvance();
		}

		#region Equality

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

		#endregion

		#region Serialization

		public SetDialogNode(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		public override string ToString()
		{
			return Speaker + ": " + Dialog;
		}
	}
}
