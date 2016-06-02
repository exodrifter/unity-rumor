using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public abstract class Value : ISerializable
	{
		protected object value;

		/// <summary>
		/// Creates a new value that wraps the specified object.
		/// </summary>
		/// <param name="value">The object to wrap.</param>
		public Value(object value)
		{
			this.value = value;
		}

		/// <summary>
		/// Returns this value as an int.
		/// </summary>
		/// <param name="str">
		/// The int to return if this value is not an int.
		/// </param>
		/// <returns>This value as an int.</returns>
		public int AsInt(int num = 0)
		{
			if (value is int)
				return (int)value;
			return num;
		}

		/// <summary>
		/// Returns this value as a float.
		/// </summary>
		/// <param name="str">
		/// The int to return if this value is not a float.
		/// </param>
		/// <returns>This value as a float.</returns>
		public float AsFloat(float num = 0f)
		{
			if (value is float)
				return (float)value;
			return num;
		}

		/// <summary>
		/// Returns this value as a string.
		/// </summary>
		/// <param name="str">
		/// The string to return if this value is not a string.
		/// </param>
		/// <returns>This value as a string.</returns>
		public string AsString(string str = "")
		{
			if (value is string)
				return (string)value;
			return str;
		}

		#region Operators

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

		#endregion

		#region Equality

		public abstract override bool Equals(object obj);

		public abstract override int GetHashCode();

		public static bool operator ==(Value l, Value r)
		{
			if (ReferenceEquals(l, r)) {
				return true;
			}
			if ((object)l == null || (object)r == null) {
				return false;
			}
			return l.Equals(r);
		}

		public static bool operator !=(Value a, Value b)
		{
			return !(a == b);
		}

		#endregion

		#region Serialization

		public Value(SerializationInfo info, StreamingContext context)
		{
			value = info.GetValue("value", typeof(object));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(object));
		}

		#endregion
	}

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
