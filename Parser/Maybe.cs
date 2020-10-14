using System;

namespace Exodrifter.Rumor.Parser
{
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
	}
}