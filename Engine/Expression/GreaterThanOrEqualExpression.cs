namespace Exodrifter.Rumor.Engine
{
	public class GreaterThanOrEqualExpression : Expression
	{
		internal readonly Expression l;
		internal readonly Expression r;

		public GreaterThanOrEqualExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return new BooleanValue(
				l.Evaluate(scope).AsNumber() >= r.Evaluate(scope).AsNumber()
			);
		}

		public override Expression Simplify()
		{
			if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new BooleanLiteral(
					left.Value.AsNumber() >= right.Value.AsNumber()
				);
			}
			else
			{
				var left = l.Simplify();
				var right = r.Simplify();

				if (l == left && r == right)
				{
					return this;
				}
				else
				{
					return new GreaterThanOrEqualExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as GreaterThanOrEqualExpression);
		}

		public bool Equals(GreaterThanOrEqualExpression other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(l, other.l)
				&& Equals(r, other.r);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(l, r);
		}

		#endregion

		public override string ToString()
		{
			return "(" + l.ToString() + " >= " + r.ToString() + ")";
		}
	}
}
