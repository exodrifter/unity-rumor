namespace Exodrifter.Rumor.Engine
{
	public class LessThanExpression : Expression<BooleanValue>
	{
		internal readonly Expression<NumberValue> l;
		internal readonly Expression<NumberValue> r;

		public LessThanExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override BooleanValue Evaluate()
		{
			return new BooleanValue(l.Evaluate() < r.Evaluate());
		}

		public override Expression<BooleanValue> Simplify()
		{
			if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new BooleanLiteral(left.Value < right.Value);
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
					return new LessThanExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as LessThanExpression);
		}

		public bool Equals(LessThanExpression other)
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
			return "(" + l.ToString() + " < " + r.ToString() + ")";
		}
	}
}
