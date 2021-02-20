using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public abstract class Node : ISerializable
	{
		public Node() { }

		public abstract Yield Execute(Rumor rumor);

		#region Serialization

		public Node(SerializationInfo info, StreamingContext context) { }

		public abstract void GetObjectData
			(SerializationInfo info, StreamingContext context);

		#endregion
	}
}
