using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ReturnNode : Node, ISerializable
	{
		public ReturnNode() { }

		public override Yield Execute(Rumor rumor)
		{
			rumor.Return();
			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as ReturnNode);
		}

		public bool Equals(ReturnNode other)
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

		public ReturnNode(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context) { }

		#endregion

		public override string ToString()
		{
			return "return";
		}
	}
}
