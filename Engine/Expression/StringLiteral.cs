using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class StringLiteral : LiteralExpression, ISerializable
	{
		public StringLiteral(StringValue value) : base(value) { }
		public StringLiteral(string value) : base(new StringValue(value)) { }

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as StringLiteral);
		}

		public bool Equals(StringLiteral other)
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

		public StringLiteral(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
