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

		public override Value Add(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() + value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() + value.AsFloat());
			}
			if (value.IsString()) {
				return new StringValue(AsInt() + value.AsString());
			}
			throw new InvalidOperationException();
		}

		public override Value Subtract(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() - value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() - value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() * value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() * value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() / value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() / value.AsFloat());
			}
			throw new InvalidOperationException();
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