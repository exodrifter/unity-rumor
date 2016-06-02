using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class FloatValue : Value, ISerializable
	{
		public FloatValue(float value)
			: base(value)
		{
		}

		public override Value Add(IntValue @int)
		{
			return new FloatValue(AsFloat() + @int.AsInt());
		}

		public override Value Add(FloatValue @float)
		{
			return new FloatValue(AsFloat() + @float.AsFloat());
		}

		public override Value Add(StringValue @string)
		{
			return new StringValue(AsFloat() + @string.AsString());
		}

		public override Value Subtract(IntValue @int)
		{
			return new FloatValue(AsFloat() - @int.AsInt());
		}

		public override Value Subtract(FloatValue @float)
		{
			return new FloatValue(AsFloat() - @float.AsFloat());
		}

		public override Value Subtract(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(IntValue @int)
		{
			return new FloatValue(AsFloat() * @int.AsInt());
		}

		public override Value Multiply(FloatValue @float)
		{
			return new FloatValue(AsFloat() * @float.AsFloat());
		}

		public override Value Multiply(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(IntValue @int)
		{
			return new FloatValue(AsFloat() / @int.AsInt());
		}

		public override Value Divide(FloatValue @float)
		{
			return new FloatValue(AsFloat() / @float.AsFloat());
		}

		public override Value Divide(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override string ToString()
		{
			return value.ToString();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as FloatValue;
			if (other == null) {
				return false;
			}
			return other.AsFloat() == AsFloat();
		}

		public bool Equals(FloatValue other)
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

		public FloatValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}