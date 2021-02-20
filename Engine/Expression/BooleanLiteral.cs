using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class BooleanLiteral : LiteralExpression, ISerializable
	{
		public BooleanLiteral(BooleanValue value) : base(value) { }
		public BooleanLiteral(bool value) : base(new BooleanValue(value)) { }

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as BooleanLiteral);
		}

		public bool Equals(BooleanLiteral other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Value);
		}

		#endregion

		#region Serialization

		public BooleanLiteral(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		public override string ToString()
		{
			return Value.AsBoolean() ? "true" : "false";
		}
	}
}
