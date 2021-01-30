namespace Exodrifter.Rumor.Engine
{
	public class ConcatExpression : Expression
	{
		private readonly Expression l;
		private readonly Expression r;

		public ConcatExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return l.Evaluate(scope).AsString() + r.Evaluate(scope).AsString();
		}

		public override Expression Simplify()
		{
			if (l is StringLiteral && r is StringLiteral)
			{
				var left = l as StringLiteral;
				var right = r as StringLiteral;

				return new StringLiteral(
					left.Value.AsString() + right.Value.AsString()
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
