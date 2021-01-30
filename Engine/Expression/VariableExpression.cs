using System;

namespace Exodrifter.Rumor.Engine
{
	public class VariableExpression : Expression
	{
		private readonly string name;

		public VariableExpression(string name)
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

			return value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as VariableExpression);
		}

		public bool Equals(VariableExpression other)
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
