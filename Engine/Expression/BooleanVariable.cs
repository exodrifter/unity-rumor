using System;

namespace Exodrifter.Rumor.Engine
{
	public class BooleanVariable : Expression<BooleanValue>
	{
		private readonly string name;

		public BooleanVariable(string name)
		{
			this.name = name;
		}

		public override BooleanValue Evaluate(RumorScope scope)
		{
			var value = scope.Get(name);
			if (value == null)
			{
				throw new InvalidOperationException(
					"Variable \"" + name+ "\" has not been defined yet!"
				);
			}
			if (!(value is BooleanValue))
			{
				throw new InvalidOperationException(
					"Variable is not a boolean!"
				);
			}

			return (BooleanValue)value;
		}

		public override Expression<BooleanValue> Simplify()
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
