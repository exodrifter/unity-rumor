using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class CallNode : Node, ISerializable
	{
		public string Label { get; }

		public CallNode(string label)
		{
			Label = label;
		}

		public override Yield Execute(Rumor rumor)
		{
			rumor.Call(Label);
			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as CallNode);
		}

		public bool Equals(CallNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Label == other.Label;
		}

		public override int GetHashCode()
		{
			return Label.GetHashCode();
		}

		#endregion

		#region Serialization

		public CallNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Label = info.GetValue<string>("label");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("label", Label);
		}

		#endregion

		public override string ToString()
		{
			return "jump " + Label;
		}
	}
}
