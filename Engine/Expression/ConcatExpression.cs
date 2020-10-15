namespace Exodrifter.Rumor.Engine
{
	public class ConcatExpression : Expression<StringValue>
	{
		private readonly Expression<StringValue> l;
		private readonly Expression<StringValue> r;

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
			if (l is StringLiteral && r is StringLiteral)
			{
				var left = l as StringLiteral;
				var right = r as StringLiteral;

				return new StringLiteral(left.Value + right.Value);
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
