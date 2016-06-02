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

		public override Value Add(IntValue @int)
		{
			return new InvalidOperationException();
		}

		public override Value Add(FloatValue @float)
		{
			return new InvalidOperationException();
		}

		public override Value Add(StringValue @string)
		{
			return new InvalidOperationException();
		}

		public override Value Add(BoolValue @bool)
		{
			return new InvalidOperationException();
		}

		public override Value Subtract(IntValue @int)
		{
			throw new InvalidOperationException();
		}

		public override Value Subtract(FloatValue @float)
		{
			throw new InvalidOperationException();
		}

		public override Value Subtract(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Subtract(BoolValue @bool)
		{
			return new InvalidOperationException();
		}

		public override Value Multiply(IntValue @int)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(FloatValue @float)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(BoolValue @bool)
		{
			return new InvalidOperationException();
		}

		public override Value Divide(IntValue @int)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(FloatValue @float)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(BoolValue @bool)
		{
			return new InvalidOperationException();
		}

		public override string ToString()
		{
			return value.ToString();
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