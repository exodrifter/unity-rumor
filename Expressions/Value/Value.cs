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

		#region Conversion

		/// <summary>
		/// Returns true if this value is an integer.
		/// </summary>
		/// <returns>True if this value is an integer.</returns>
		public bool IsInt()
		{
			return value is int;
		}

		/// <summary>
		/// Returns true if this value is a float.
		/// </summary>
		/// <returns>True if this value is a float.</returns>
		public bool IsFloat()
		{
			return value is float;
		}

		/// <summary>
		/// Returns true if this value is a string.
		/// </summary>
		/// <returns>True if this value is a string.</returns>
		public bool IsString()
		{
			return value is string;
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

		#endregion

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
}
