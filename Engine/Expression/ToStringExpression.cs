namespace Exodrifter.Rumor.Engine
{
	public class ToStringExpression : Expression
	{
		private readonly Expression value;

		public ToStringExpression(Expression value)
		{
			this.value = value;
		}

		public override Value Evaluate(RumorScope scope)
		{
			var result = value.Evaluate(scope).InternalValue;
			return new StringValue(result.ToString());
		}

		public override Expression Simplify()
		{
			// Inline literals
			if (value is BooleanLiteral)
			{
				var b = (value as BooleanLiteral).Value;
				return new StringLiteral(b.ToString());
			}
			else if (value is NumberLiteral)
			{
				var n = (value as NumberLiteral).Value;
				return new StringLiteral(n.ToString());
			}
			else if (value is StringLiteral)
			{
				return value as StringLiteral;
			}

			// Attempt more simplification
			else
			{
				var v = value.Simplify();

				// No further simplification can be done
				if (v == value)
				{
					return this;
				}
				else
				{
					return new ToStringExpression(v).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as ToStringExpression);
		}

		public bool Equals(ToStringExpression other)
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

		#endregion

		public override string ToString()
		{
			return "<" + value.ToString() + ">";
		}
	}
}
