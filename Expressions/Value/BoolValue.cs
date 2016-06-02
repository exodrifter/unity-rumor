using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class BoolValue : Value, ISerializable
	{
		public BoolValue(bool value)
			: base(value)
		{
		}

		public override Value Add(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value Subtract(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			throw new InvalidOperationException();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as BoolValue;
			if (other == null) {
				return false;
			}
			return other.AsString() == AsString();
		}

		public bool Equals(BoolValue other)
		{
			if (other == null) {
				return false;
			}
			return other.value == value;
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		#endregion

		#region Serialization

		public BoolValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}