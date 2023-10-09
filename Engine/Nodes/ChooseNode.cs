using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ChooseNode : Node, ISerializable
	{
		private readonly Expression timeout;
		private readonly MoveType? moveType;
		private readonly string label;

		public ChooseNode() { }

		public ChooseNode(Expression timeout, MoveType moveType, string label)
		{
			this.timeout = timeout;
			this.moveType = moveType;
			this.label = label;
		}

		public override Yield Execute(Rumor rumor)
		{
			return new ForChoose(
				timeout?.Evaluate(rumor.Scope, rumor.Bindings).AsNumber().Value,
				moveType,
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
			moveType = info.GetValue<MoveType?>("moveType");
			label = info.GetValue<string>("label");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("timeout", timeout);
			info.AddValue<MoveType?>("moveType", moveType);
			info.AddValue<string>("label", label);
		}

		#endregion

		public override string ToString()
		{
			if (timeout == null)
			{
				return "choose";
			}
			else
			{
				var move = moveType == MoveType.Jump ? "jump" : "call";
				return "choose in {" + timeout + "} seconds or " + move + " " +
					label;
			}
		}
	}
}
