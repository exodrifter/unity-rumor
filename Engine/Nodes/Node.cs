using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public abstract class Node : ISerializable
	{
		public Node() { }

		public abstract Yield Execute(Rumor rumor);

		public static bool operator ==(Node l, Node r)
		{
			if (ReferenceEquals(l, r))
			{
				return true;
			}
			if (l as object == null || r as object == null)
			{
				return false;
			}
			return l.Equals(r);
		}

		public static bool operator !=(Node l, Node r)
		{
			return !(l == r);
		}

		#region Serialization

		public Node(SerializationInfo info, StreamingContext context) { }

		public abstract void GetObjectData
			(SerializationInfo info, StreamingContext context);

		#endregion
	}
}
