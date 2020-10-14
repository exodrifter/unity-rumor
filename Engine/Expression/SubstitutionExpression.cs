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
			if (value is LiteralExpression<BooleanValue>)
			{
				var b = (value as LiteralExpression<BooleanValue>).Value;
				return new LiteralExpression<StringValue>(
					new StringValue(b.ToString())
				);
			}
			else if (value is LiteralExpression<NumberValue>)
			{
				var n = (value as LiteralExpression<NumberValue>).Value;
				return new LiteralExpression<StringValue>(
					new StringValue(n.ToString())
				);
			}
			else if (value is LiteralExpression<StringValue>)
			{
				return value as LiteralExpression<StringValue>;
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
