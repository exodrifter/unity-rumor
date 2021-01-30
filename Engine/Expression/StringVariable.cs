using System;

namespace Exodrifter.Rumor.Engine
{
	public class StringVariable : Expression
	{
		private readonly string name;

		public StringVariable(string name)
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
			if (!(value is StringValue))
			{
				throw new InvalidOperationException(
					"Variable is not a string!"
				);
			}

			return (StringValue)value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as StringVariable);
		}

		public bool Equals(StringVariable other)
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
