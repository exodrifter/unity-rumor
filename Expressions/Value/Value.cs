using Exodrifter.Rumor.Util;
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

		public override string ToString()
		{
			if (value == null) {
				return "<null>";
			}
			return value.ToString();
		}

		#region Conversion

		/// <summary>
		/// Returns true if this value is a bool.
		/// </summary>
		/// <returns>True if this value is a bool.</returns>
		public bool IsBool()
		{
			return value is bool;
		}

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
		/// Returns true if this value is not a boolean, integer, float, or
		/// string.
		/// </summary>
		/// <returns>
		/// True if this value is not a boolean, integer, float, or string.
		/// </returns>
		public bool IsObject()
		{
			return !IsBool() && !IsInt() && !IsFloat() && !IsString();
		}

		/// <summary>
		/// Returns this value as a bool.
		/// </summary>
		/// <param name="val">
		/// The bool to return if this value is not a bool.
		/// </param>
		/// <returns>This value as a bool.</returns>
		public bool AsBool(bool val = false)
		{
			if (value is bool)
				return (bool)value;
			return val;
		}

		/// <summary>
		/// Returns this value as an int.
		/// </summary>
		/// <param name="num">
		/// The int to return if this value is not an int.
		/// </param>
		/// <returns>This value as an int.</returns>
		public int AsInt(int num = 0)
		{
			if (value is int)
				return (int)value;
			if (value is float)
				return (int)(float)value;
			return num;
		}

		/// <summary>
		/// Returns this value as a float.
		/// </summary>
		/// <param name="num">
		/// The float to return if this value is not a float.
		/// </param>
		/// <returns>This value as a float.</returns>
		public float AsFloat(float num = 0f)
		{
			if (value is float)
				return (float)value;
			if (value is int)
				return (float)(int)value;
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

		/// <summary>
		/// Returns this value as an object.
		/// </summary>
		/// <returns>This value as an object.</returns>
		public object AsObject()
		{
			return value;
		}

		/// <summary>
		/// Converts an object into the best-matching value.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>This object as a value.</returns>
		public static Value Covert(object value)
		{
			if (value == null) {
				return new ObjectValue(value);
			}

			if (typeof(int) == value.GetType()) {
				return new IntValue((int)value);
			}
			if (typeof(float) == value.GetType()) {
				return new FloatValue((float)value);
			}
			if (typeof(double) == value.GetType()) {
				return new FloatValue((float)value);
			}
			if (typeof(string) == value.GetType()) {
				return new StringValue((string)value);
			}
			if (typeof(bool) == value.GetType()) {
				return new BoolValue((bool)value);
			}
			return new ObjectValue(value);
		}

		#endregion

		#region Operators

		public abstract Value Not();

		public abstract Value Add(Value value);
		public abstract Value Subtract(Value value);
		public abstract Value Multiply(Value value);
		public abstract Value Divide(Value value);

		public abstract Value LessThan(Value value);
		public abstract Value GreaterThan(Value value);
		public abstract Value EqualTo(Value value);

		public abstract Value BoolAnd(Value value);
		public abstract Value BoolOr(Value value);
		public Value BoolXor(Value value)
		{
			var and = BoolAnd(value).AsBool();
			var or = BoolOr(value).AsBool();
			return new BoolValue(!and && or);
		}

		#endregion

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as Value;
			if (other == null) {
				return false;
			}
			return Equals(value, other.value);
		}

		public bool Equals(Value other)
		{
			if (other == null) {
				return false;
			}
			return Equals(value, other.value);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

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

		public static bool operator !=(Value l, Value r)
		{
			return !(l == r);
		}

		#endregion

		#region Serialization

		public Value(SerializationInfo info, StreamingContext context)
		{
			value = info.GetValue<object>("value");
			var type = info.GetValue<Type>("type");

			// Make sure the type is set correctly
			value = Convert.ChangeType(value, type);
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Type>("type", value.GetType());
			info.AddValue<object>("value", value);
		}

		#endregion
	}
}
