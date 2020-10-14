namespace Exodrifter.Rumor.Engine
{
	public class ConcatExpression : Expression<StringValue>
	{
		private readonly Expression<StringValue> l;
		private readonly Expression<StringValue> r;

		public ConcatExpression(StringValue l, StringValue r)
		{
			this.l = new LiteralExpression<StringValue>(l);
			this.r = new LiteralExpression<StringValue>(r);
		}

		public ConcatExpression(StringValue l, Expression<StringValue> r)
		{
			this.l = new LiteralExpression<StringValue>(l);
			this.r = r;
		}

		public ConcatExpression(Expression<StringValue> l, StringValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<StringValue>(r);
		}

		public ConcatExpression
			(Expression<StringValue> l, Expression<StringValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override StringValue Evaluate()
		{
			return l.Evaluate() + r.Evaluate();
		}

		public override Expression<StringValue> Simplify()
		{
			if (l is LiteralExpression<StringValue>
				&& r is LiteralExpression<StringValue>)
			{
				var left = l as LiteralExpression<StringValue>;
				var right = r as LiteralExpression<StringValue>;

				return new LiteralExpression<StringValue>(
					left.Value + right.Value
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
					return new ConcatExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as ConcatExpression);
		}

		public bool Equals(ConcatExpression other)
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
			return "(" + l.ToString() + " + " + r.ToString() + ")";
		}
	}
}
