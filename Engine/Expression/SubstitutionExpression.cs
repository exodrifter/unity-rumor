namespace Exodrifter.Rumor.Engine
{
	public class SubstitutionExpression<T> : Expression<StringValue>
		where T : Value
	{
		private readonly Expression<T> value;

		public SubstitutionExpression(Expression<T> value)
		{
			this.value = value;
		}

		public override StringValue Evaluate()
		{
			return new StringValue(value.Evaluate().ToString());
		}

		public override Expression<StringValue> Simplify()
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
					return new SubstitutionExpression<T>(v).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as SubstitutionExpression<T>);
		}

		public bool Equals(SubstitutionExpression<T> other)
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
