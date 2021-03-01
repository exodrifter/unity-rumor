using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ChooseNode : Node, ISerializable
	{
		private readonly Expression timeout;
		private readonly string label;

		public ChooseNode() { }

		public ChooseNode(Expression timeout, string label)
		{
			this.timeout = timeout;
			this.label = label;
		}

		public override Yield Execute(Rumor rumor)
		{
			return new ForChoose(
				timeout?.Evaluate(rumor.Scope).AsNumber().Value,
				label
			);
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as ChooseNode);
		}

		public bool Equals(ChooseNode other)
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

		#endregion

		#region Serialization

		public ChooseNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			timeout = info.GetValue<Expression>("timeout");
			label = info.GetValue<string>("label");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("timeout", timeout);
			info.AddValue<string>("label", label);
		}

		#endregion

		public override string ToString()
		{
			return "choose";
		}
	}
}
