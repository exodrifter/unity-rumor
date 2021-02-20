using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public abstract class DialogNode : Node, ISerializable
	{
		public string Speaker { get; }
		public Expression Dialog { get; }

		public DialogNode(string speaker, string dialog)
		{
			Speaker = speaker;
			Dialog = new StringLiteral(dialog);
		}

		public DialogNode(string speaker, Expression dialog)
		{
			Speaker = speaker;
			Dialog = dialog;
		}

		#region Serialization

		public DialogNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Speaker = info.GetValue<string>("speaker");
			Dialog = info.GetValue<Expression>("dialog");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("speaker", Speaker);
			info.AddValue<Expression>("dialog", Dialog);
		}

		#endregion
	}
}
