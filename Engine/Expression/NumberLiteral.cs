using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class NumberLiteral : LiteralExpression, ISerializable
	{
		public NumberLiteral(NumberValue value) : base(value) { }
		public NumberLiteral(double value) : base(new NumberValue(value)) { }

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as NumberLiteral);
		}

		public bool Equals(NumberLiteral other)
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

		public NumberLiteral(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
