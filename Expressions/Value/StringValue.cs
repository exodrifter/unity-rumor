using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class StringValue : Value, ISerializable
	{
		public StringValue(string value)
			: base(value)
		{
		}

		public override Value Add(IntValue @int)
		{
			return new StringValue(AsString() + @int.AsInt());
		}

		public override Value Add(FloatValue @float)
		{
			return new StringValue(AsString() + @float.AsFloat());
		}

		public override Value Add(StringValue @string)
		{
			return new StringValue(AsString() + @string.AsString());
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

		public override string ToString()
		{
			return value.ToString();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as StringValue;
			if (other == null) {
				return false;
			}
			return other.AsString() == AsString();
		}

		public bool Equals(StringValue other)
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

		public StringValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}