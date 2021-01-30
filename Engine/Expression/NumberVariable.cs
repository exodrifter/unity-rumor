using System;

namespace Exodrifter.Rumor.Engine
{
	public class NumberVariable : Expression
	{
		private readonly string name;

		public NumberVariable(string name)
		{
			this.name = name;
		}

		public override Value Evaluate(RumorScope scope)
		{
			var value = scope.Get(name);
			if (value == null)
			{
				throw new InvalidOperationException(
					"Variable \"" + name+ "\" has not been defined yet!"
				);
			}
			if (!(value is NumberValue))
			{
				throw new InvalidOperationException(
					"Variable is not a number!"
				);
			}

			return (NumberValue)value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as NumberVariable);
		}

		public bool Equals(NumberVariable other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(name, other.name);
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		#endregion

		public override string ToString()
		{
			return name;
		}
	}
}
