namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Represents a value in Rumor.
	/// </summary>
	public abstract class Value
	{
		protected object value { get; }

		public abstract ValueType Type { get; }

		/// <summary>
		/// Creates a new value.
		/// </summary>
		/// <param name="value">The value.</param>
		public Value(object value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			if (value == null)
			{
				return "<null>";
			}
			return value.ToString();
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
			return Equals(value, other.value);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(value);
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
	}
}
