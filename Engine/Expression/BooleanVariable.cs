using System;

namespace Exodrifter.Rumor.Engine
{
	public class BooleanVariable : Expression
	{
		private readonly string name;

		public BooleanVariable(string name)
		{
			this.name = name;
		}

		public override Value Evaluate(RumorScope scope)
		{
			var value = scope.Get(name);
			if (value == null)
			{
				throw new UndefinedVariableException(
					"Variable \"" + name+ "\" has not been defined yet!"
				);
			}
			if (!(value is BooleanValue))
			{
				throw new VariableTypeException(
					"Variable is not a boolean!"
				);
			}

			return (BooleanValue)value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as BooleanVariable);
		}

		public bool Equals(BooleanVariable other)
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
