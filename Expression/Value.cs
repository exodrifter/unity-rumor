using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public abstract class Value
	{
		public Value Add(Value value)
		{
			if (value.GetType() == typeof(IntValue)) {
				return Add(value as IntValue);
			}
			if (value.GetType() == typeof(FloatValue)) {
				return Add(value as FloatValue);
			}
			if (value.GetType() == typeof(StringValue)) {
				return Add(value as StringValue);
			}
			throw new InvalidOperationException();
		}

		public abstract Value Add(IntValue @int);
		public abstract Value Add(FloatValue @float);
		public abstract Value Add(StringValue @string);

		public Value Subtract(Value value)
		{
			if (value.GetType() == typeof(IntValue)) {
				return Subtract(value as IntValue);
			}
			if (value.GetType() == typeof(FloatValue)) {
				return Subtract(value as FloatValue);
			}
			if (value.GetType() == typeof(StringValue)) {
				return Subtract(value as StringValue);
			}
			throw new InvalidOperationException();
		}

		public abstract Value Subtract(IntValue @int);
		public abstract Value Subtract(FloatValue @float);
		public abstract Value Subtract(StringValue @string);

		public Value Divide(Value value)
		{
			if (value.GetType() == typeof(IntValue)) {
				return Divide(value as IntValue);
			}
			if (value.GetType() == typeof(FloatValue)) {
				return Divide(value as FloatValue);
			}
			if (value.GetType() == typeof(StringValue)) {
				return Divide(value as StringValue);
			}
			throw new InvalidOperationException();
		}

		public abstract Value Divide(IntValue @int);
		public abstract Value Divide(FloatValue @float);
		public abstract Value Divide(StringValue @string);

		public Value Multiply(Value value)
		{
			if (value.GetType() == typeof(IntValue)) {
				return Multiply(value as IntValue);
			}
			if (value.GetType() == typeof(FloatValue)) {
				return Multiply(value as FloatValue);
			}
			if (value.GetType() == typeof(StringValue)) {
				return Multiply(value as StringValue);
			}
			throw new InvalidOperationException();
		}

		public abstract Value Multiply(IntValue @int);
		public abstract Value Multiply(FloatValue @float);
		public abstract Value Multiply(StringValue @string);
	}

	[Serializable]
	public class IntValue : Value, ISerializable
	{
		/// <summary>
		/// The int this value wraps.
		/// </summary>
		public int Value { get { return value; } }
		private readonly int value;

		public IntValue(int value)
		{
			this.value = value;
		}

		public override Value Add(IntValue @int)
		{
			return new IntValue(value + @int.Value);
		}

		public override Value Add(FloatValue @float)
		{
			return new FloatValue(value + @float.Value);
		}

		public override Value Add(StringValue @string)
		{
			return new StringValue(value + @string.Value);
		}

		public override Value Subtract(IntValue @int)
		{
			return new IntValue(value - @int.Value);
		}

		public override Value Subtract(FloatValue @float)
		{
			return new FloatValue(value - @float.Value);
		}

		public override Value Subtract(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(IntValue @int)
		{
			return new IntValue(value * @int.Value);
		}

		public override Value Multiply(FloatValue @float)
		{
			return new FloatValue(value * @float.Value);
		}

		public override Value Multiply(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(IntValue @int)
		{
			return new IntValue(value / @int.Value);
		}

		public override Value Divide(FloatValue @float)
		{
			return new FloatValue(value / @float.Value);
		}

		public override Value Divide(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as IntValue;
			if (other == null) {
				return false;
			}
			return other.value == value;
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

		public static bool operator ==(IntValue l, IntValue r)
		{
			if (ReferenceEquals(l, r)) {
				return true;
			}
			if ((object)l == null ^ (object)r == null) {
				return false;
			}
			return l.value == r.value;
		}

		public static bool operator !=(IntValue a, IntValue b)
		{
			return !(a == b);
		}

		#endregion

		#region Serialization

		public IntValue(SerializationInfo info, StreamingContext context)
		{
			value = (int)info.GetValue("value", typeof(int));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(int));
		}

		#endregion
	}

	[Serializable]
	public class FloatValue : Value, ISerializable
	{
		/// <summary>
		/// The float this value wraps.
		/// </summary>
		public float Value { get { return value; } }
		private readonly float value;

		public FloatValue(float value)
		{
			this.value = value;
		}

		public override Value Add(IntValue @int)
		{
			return new FloatValue(value + @int.Value);
		}

		public override Value Add(FloatValue @float)
		{
			return new FloatValue(value + @float.Value);
		}

		public override Value Add(StringValue @string)
		{
			return new StringValue(value + @string.Value);
		}

		public override Value Subtract(IntValue @int)
		{
			return new FloatValue(value - @int.Value);
		}

		public override Value Subtract(FloatValue @float)
		{
			return new FloatValue(value - @float.Value);
		}

		public override Value Subtract(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(IntValue @int)
		{
			return new FloatValue(value * @int.Value);
		}

		public override Value Multiply(FloatValue @float)
		{
			return new FloatValue(value * @float.Value);
		}

		public override Value Multiply(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(IntValue @int)
		{
			return new FloatValue(value / @int.Value);
		}

		public override Value Divide(FloatValue @float)
		{
			return new FloatValue(value / @float.Value);
		}

		public override Value Divide(StringValue @string)
		{
			throw new InvalidOperationException();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as FloatValue;
			if (other == null) {
				return false;
			}
			return other.value == value;
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

		public static bool operator ==(FloatValue l, FloatValue r)
		{
			if (ReferenceEquals(l, r)) {
				return true;
			}
			if ((object)l == null ^ (object)r == null) {
				return false;
			}
			return l.value == r.value;
		}

		public static bool operator !=(FloatValue a, FloatValue b)
		{
			return !(a == b);
		}

		#endregion

		#region Serialization

		public FloatValue(SerializationInfo info, StreamingContext context)
		{
			value = (float)info.GetValue("value", typeof(float));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(float));
		}

		#endregion
	}

	[Serializable]
	public class StringValue : Value, ISerializable
	{
		/// <summary>
		/// The string this value wraps.
		/// </summary>
		public string Value { get { return value; } }
		private readonly string value;

		public StringValue(string value)
		{
			this.value = value;
		}

		public override Value Add(IntValue @int)
		{
			return new StringValue(value + @int.Value);
		}

		public override Value Add(FloatValue @float)
		{
			return new StringValue(value + @float.Value);
		}

		public override Value Add(StringValue @string)
		{
			return new StringValue(value + @string.Value);
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

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as StringValue;
			if (other == null) {
				return false;
			}
			return other.value == value;
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

		public static bool operator ==(StringValue l, StringValue r)
		{
			if (ReferenceEquals(l, r)) {
				return true;
			}
			if ((object)l == null ^ (object)r == null) {
				return false;
			}
			return l.value == r.value;
		}

		public static bool operator !=(StringValue a, StringValue b)
		{
			return !(a == b);
		}

		#endregion

		#region Serialization

		public StringValue(SerializationInfo info, StreamingContext context)
		{
			value = (string)info.GetValue("value", typeof(string));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(string));
		}

		#endregion
	}
}
