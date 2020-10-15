namespace Exodrifter.Rumor.Engine
{
	public class AndExpression : Expression<BooleanValue>
	{
		private readonly Expression<BooleanValue> l;
		private readonly Expression<BooleanValue> r;

		public AndExpression(BooleanValue l, BooleanValue r)
		{
			this.l = new LiteralExpression<BooleanValue>(l);
			this.r = new LiteralExpression<BooleanValue>(r);
		}

		public AndExpression(BooleanValue l, Expression<BooleanValue> r)
		{
			this.l = new LiteralExpression<BooleanValue>(l);
			this.r = r;
		}

		public AndExpression(Expression<BooleanValue> l, BooleanValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<BooleanValue>(r);
		}

		public AndExpression
			(Expression<BooleanValue> l, Expression<BooleanValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override BooleanValue Evaluate()
		{
			return l.Evaluate() && r.Evaluate();
		}

		public override Expression<BooleanValue> Simplify()
		{
			if (l is LiteralExpression<BooleanValue>
				&& r is LiteralExpression<BooleanValue>)
			{
				var left = l as LiteralExpression<BooleanValue>;
				var right = r as LiteralExpression<BooleanValue>;

				return new LiteralExpression<BooleanValue>(
					left.Value && right.Value
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
					return new AndExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as AndExpression);
		}

		public bool Equals(AndExpression other)
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
			return "(" + l.ToString() + " and " + r.ToString() + ")";
		}
	}
}
