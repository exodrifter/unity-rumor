namespace Exodrifter.Rumor.Engine
{
	public class DivideExpression : Expression<NumberValue>
	{
		private readonly Expression<NumberValue> l;
		private readonly Expression<NumberValue> r;

		public DivideExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override NumberValue Evaluate(RumorScope scope)
		{
			return l.Evaluate(scope) / r.Evaluate(scope);
		}

		public override Expression<NumberValue> Simplify()
		{
			if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new NumberLiteral(left.Value / right.Value);
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
