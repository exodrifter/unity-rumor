namespace Exodrifter.Rumor.Engine
{
	public class DivideExpression : Expression<NumberValue>
	{
		private readonly Expression<NumberValue> l;
		private readonly Expression<NumberValue> r;

		public DivideExpression(NumberValue l, NumberValue r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public DivideExpression(NumberValue l, Expression<NumberValue> r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = r;
		}

		public DivideExpression(Expression<NumberValue> l, NumberValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public DivideExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override NumberValue Evaluate()
		{
			return l.Evaluate() / r.Evaluate();
		}

		public override Expression<NumberValue> Simplify()
		{
			if (l is LiteralExpression<NumberValue>
				&& r is LiteralExpression<NumberValue>)
			{
				var left = l as LiteralExpression<NumberValue>;
				var right = r as LiteralExpression<NumberValue>;

				return new LiteralExpression<NumberValue>(
					left.Value / right.Value
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
					return new DivideExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as DivideExpression);
		}

		public bool Equals(DivideExpression other)
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
			return "(" + l.ToString() + " / " + r.ToString() + ")";
		}
	}
}
