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

		public override Value Add(Value value)
		{
			if (value.IsInt()) {
				return new FloatValue(AsFloat() + value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() + value.AsFloat());
			}
			if (value.IsString()) {
				return new StringValue(AsFloat() + value.AsString());
			}
			throw new InvalidOperationException();
		}

		public override Value Subtract(Value value)
		{
			if (value.IsInt()) {
				return new FloatValue(AsFloat() - value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() - value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			if (value.IsInt()) {
				return new FloatValue(AsFloat() * value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() * value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			if (value.IsInt()) {
				return new FloatValue(AsFloat() / value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() / value.AsFloat());
			}
			throw new InvalidOperationException();
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