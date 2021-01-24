namespace Exodrifter.Rumor.Engine
{
	public class IsNotExpression<T> : Expression<BooleanValue> where T : Value
	{
		internal readonly Expression<T> l;
		internal readonly Expression<T> r;

		public IsNotExpression
			(Expression<T> l, Expression<T> r)
		{
			this.l = l;
			this.r = r;
		}

		public override BooleanValue Evaluate(RumorScope scope)
		{
			return new BooleanValue(l.Evaluate(scope) != r.Evaluate(scope));
		}

		public override Expression<BooleanValue> Simplify()
		{
			if (l is BooleanLiteral && r is BooleanLiteral)
			{
				var left = l as BooleanLiteral;
				var right = r as BooleanLiteral;

				return new BooleanLiteral(left.Value != right.Value);
			}
			else if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new BooleanLiteral(left.Value != right.Value);
			}
			else if (l is StringLiteral && r is StringLiteral)
			{
				var left = l as StringLiteral;
				var right = r as StringLiteral;

				return new BooleanLiteral(left.Value != right.Value);
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
					return new IsNotExpression<T>(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as IsNotExpression<T>);
		}

		public bool Equals(IsNotExpression<T> other)
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
			return "(" + l.ToString() + " is not " + r.ToString() + ")";
		}
	}
}
