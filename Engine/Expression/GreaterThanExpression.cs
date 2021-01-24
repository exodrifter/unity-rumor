namespace Exodrifter.Rumor.Engine
{
	public class GreaterThanExpression : Expression<BooleanValue>
	{
		internal readonly Expression<NumberValue> l;
		internal readonly Expression<NumberValue> r;

		public GreaterThanExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override BooleanValue Evaluate(RumorScope scope)
		{
			return new BooleanValue(l.Evaluate(scope) > r.Evaluate(scope));
		}

		public override Expression<BooleanValue> Simplify()
		{
			if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new BooleanLiteral(left.Value > right.Value);
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
					return new GreaterThanExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as GreaterThanExpression);
		}

		public bool Equals(GreaterThanExpression other)
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
			return "(" + l.ToString() + " > " + r.ToString() + ")";
		}
	}
}
