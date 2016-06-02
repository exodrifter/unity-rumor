using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class IntValue : Value
	{
		public IntValue(int value)
			: base(value)
		{
		}

		public override Value Add(IntValue @int)
		{
			return new IntValue(AsInt() + @int.AsInt());
		}

		public override Value Add(FloatValue @float)
		{
			return new FloatValue(AsInt() + @float.AsFloat());
		}

		public override Value Add(StringValue @string)
		{
			return new StringValue(AsInt() + @string.AsString());
		}

		public override Value Add(BoolValue @bool)
		{
			return new InvalidOperationException();
		}

		public override Value Subtract(IntValue @int)
		{
			return new IntValue(AsInt() - @int.AsInt());
		}

		public override Value Subtract(FloatValue @float)
		{
			return new FloatValue(AsInt() - @float.AsFloat());
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
			return new IntValue(AsInt() * @int.AsInt());
		}

		public override Value Multiply(FloatValue @float)
		{
			return new FloatValue(AsInt() * @float.AsFloat());
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
			return new IntValue(AsInt() / @int.AsInt());
		}

		public override Value Divide(FloatValue @float)
		{
			return new FloatValue(AsInt() / @float.AsFloat());
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
			var other = obj as IntValue;
			if (other == null) {
				return false;
			}
			return other.AsInt() == AsInt();
		}

		public bool Equals(IntValue other)
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

		public IntValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}