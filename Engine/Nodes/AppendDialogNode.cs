using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class AppendDialogNode : DialogNode, ISerializable
	{
		public AppendDialogNode(string speaker, string dialog)
			: base(speaker, dialog) { }

		public AppendDialogNode(string speaker, Expression dialog)
			: base(speaker, dialog) { }

		public override Yield Execute(Rumor rumor)
		{
			var value = Dialog.Evaluate(rumor.Scope, rumor.Bindings);
			var dialog = value.AsString().Value;
			rumor.State.AppendDialog(Speaker, dialog);
			return new ForAdvance();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as AppendDialogNode);
		}

		public bool Equals(AppendDialogNode other)
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
			return Util.GetHashCode("append", Speaker, Dialog);
		}

		#endregion

		#region Serialization

		public AppendDialogNode(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		public override string ToString()
		{
			return Speaker + "+ " + Dialog;
		}
	}
}
