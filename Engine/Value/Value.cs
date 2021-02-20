using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Represents a value in Rumor.
	/// </summary>
	[Serializable]
	public abstract class Value : ISerializable
	{
		public object InternalValue { get; }

		public abstract ValueType Type { get; }

		/// <summary>
		/// Creates a new value.
		/// </summary>
		/// <param name="value">The value.</param>
		public Value(object value)
		{
			InternalValue = value;
		}

		public BooleanValue AsBoolean()
		{
			return (BooleanValue)this;
		}

		public NumberValue AsNumber()
		{
			return (NumberValue)this;
		}

		public StringValue AsString()
		{
			return (StringValue)this;
		}

		public override string ToString()
		{
			if (InternalValue == null)
			{
				return "<null>";
			}
			return InternalValue.ToString();
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as Value);
		}

		public bool Equals(Value other)
		{
			if (other == null)
			{
				return false;
			}
			return Equals(InternalValue, other.InternalValue);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(InternalValue);
		}

		public static bool operator ==(Value l, Value r)
		{
			if (ReferenceEquals(l, r))
			{
				return true;
			}
			if (l as object == null || r as object == null)
			{
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
			var value = info.GetValue<object>("value");
			var type = info.GetValue<Type>("type");
			InternalValue = Convert.ChangeType(value, type);
		}

		public void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Type>("type", InternalValue.GetType());
			info.AddValue<object>("value", InternalValue);
		}

		#endregion
	}
}
