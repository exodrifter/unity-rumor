using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class WaitNode : Node, ISerializable
	{
		public WaitNode() { }

		public override Yield Execute(Rumor rumor)
		{
			return new ForAdvance();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as WaitNode);
		}

		public bool Equals(WaitNode other)
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

		public WaitNode(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context) { }

		#endregion

		public override string ToString()
		{
			return "wait";
		}
	}
}
