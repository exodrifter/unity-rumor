using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ChooseNode : Node, ISerializable
	{
		public ChooseNode() { }

		public override Yield Execute(Rumor rumor)
		{
			return new ForChoose();
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
			: base(info, context) { }

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context) { }

		#endregion

		public override string ToString()
		{
			return "choose";
		}
	}
}
