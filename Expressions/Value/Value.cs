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

		public abstract Value Add(Value value);
		public abstract Value Subtract(Value value);
		public abstract Value Multiply(Value value);
		public abstract Value Divide(Value value);

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
