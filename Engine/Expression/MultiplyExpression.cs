namespace Exodrifter.Rumor.Engine
{
	public class MultiplyExpression : Expression<NumberValue>
	{
		private readonly Expression<NumberValue> l;
		private readonly Expression<NumberValue> r;

		public MultiplyExpression(NumberValue l, NumberValue r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public MultiplyExpression(NumberValue l, Expression<NumberValue> r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = r;
		}

		public MultiplyExpression(Expression<NumberValue> l, NumberValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public MultiplyExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}
		public override NumberValue Evaluate()
		{
			return l.Evaluate() * r.Evaluate();
		}

		public override Expression<NumberValue> Simplify()
		{
			if (l is LiteralExpression<NumberValue>
				&& r is LiteralExpression<NumberValue>)
			{
				var left = l as LiteralExpression<NumberValue>;
				var right = r as LiteralExpression<NumberValue>;

				return new LiteralExpression<NumberValue>(
					left.Value * right.Value
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
					return new MultiplyExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as MultiplyExpression);
		}

		public bool Equals(MultiplyExpression other)
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
			return "(" + l.ToString() + " * " + r.ToString() + ")";
		}
	}
}
