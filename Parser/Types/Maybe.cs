using System;

namespace Exodrifter.Rumor.Parser
{
	/// <summary>
	/// Represents something that may or may not exist.
	/// </summary>
	/// <typeparam name="T">The type to wrap.</typeparam>
	public class Maybe<T>
	{
		public bool HasValue { get; }
		private readonly T value;

		public T Value
		{
			get
			{
				if (HasValue)
				{
					return value;
				}
				else
				{
					throw new InvalidOperationException(
						"Maybe does not have a value."
					);
				}
			}
		}

		public Maybe()
		{
			HasValue = false;
			value = default;
		}

		public Maybe(T value)
		{
			HasValue = true;
			this.value = value;
		}

		/// <summary>
		/// Returns the value in this Maybe or the default value.
		/// </summary>
		public T GetValueOrDefault()
		{
			if (HasValue)
			{
				return value;
			}
			else
			{
				return default;
			}
		}

		/// <summary>
		/// Returns the value in this Maybe or the default value.
		/// </summary>
		/// <param name="other">The default value to use.</param>
		public T GetValueOrDefault(T other)
		{
			if (HasValue)
			{
				return value;
			}
			else
			{
				return other;
			}
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Maybe<T>);
		}

		public bool Equals(Maybe<T> other)
		{
			if (HasValue)
			{
				return HasValue == other.HasValue
					&& Value.Equals(other.Value);
			}

			return HasValue == other.HasValue;
		}

		public override int GetHashCode()
		{
			if (HasValue)
			{
				return 0;
			}
			else
			{
				return Value.GetHashCode();
			}
		}
	}
}