using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ClearNode : Node, ISerializable
	{
		public ClearType Type { get; }
		public string LabelTarget { get; }

		public ClearNode(ClearType type, string labelTarget)
		{
			Type = type;
			LabelTarget = labelTarget;
		}

		public override Yield Execute(Rumor rumor)
		{
			rumor.State.Clear(Type, LabelTarget);
			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as ClearNode);
		}

		public bool Equals(ClearNode other)
		{
			if (other == null)
			{
				return false;
			}

			return this.Type == other.Type
				&& this.LabelTarget == other.LabelTarget;
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode("clear", Type, LabelTarget);
		}

		#endregion

		#region Serialization

		public ClearNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Type = info.GetValue<ClearType>("type");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<ClearType>("type", Type);
		}

		#endregion

		public override string ToString()
		{
			switch (Type)
			{
				case ClearType.All:
					return "clear all";
				case ClearType.Choices:
					return "clear choices";
				case ClearType.Dialog:
					return "clear dialog";

				default:
					return "clear " + Type;
			}
		}
	}
}
