using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class JumpNode : Node, ISerializable
	{
		public string Label { get; }

		public JumpNode(string label)
		{
			Label = label;
		}

		public override Yield Execute(Rumor rumor)
		{
			rumor.Jump(Label);
			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as JumpNode);
		}

		public bool Equals(JumpNode other)
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

		public JumpNode(SerializationInfo info, StreamingContext context)
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
