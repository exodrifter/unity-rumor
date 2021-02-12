namespace Exodrifter.Rumor.Engine
{
	public class SubstitutionExpression : Expression
	{
		private readonly Expression value;

		public SubstitutionExpression(Expression value)
		{
			this.value = value;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return new StringValue(value.Evaluate(scope).ToString());
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
					return new SubstitutionExpression(v).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as SubstitutionExpression);
		}

		public bool Equals(SubstitutionExpression other)
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
			return "{" + value.ToString() + "}";
		}
	}
}
