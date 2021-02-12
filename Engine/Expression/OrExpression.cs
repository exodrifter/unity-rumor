namespace Exodrifter.Rumor.Engine
{
	public class OrExpression : Expression
	{
		private readonly Expression l;
		private readonly Expression r;

		public OrExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return l.Evaluate(scope).AsBoolean() || r.Evaluate(scope).AsBoolean();
		}

		public override Expression Simplify()
		{
			if (l is BooleanLiteral && r is BooleanLiteral)
			{
				var left = l as BooleanLiteral;
				var right = r as BooleanLiteral;

				return new BooleanLiteral(
					left.Value.AsBoolean() || right.Value.AsBoolean()
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
					return new OrExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as OrExpression);
		}

		public bool Equals(OrExpression other)
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
			return "(" + l.ToString() + " or " + r.ToString() + ")";
		}
	}
}
