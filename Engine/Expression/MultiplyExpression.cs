﻿namespace Exodrifter.Rumor.Engine
{
	public class MultiplyExpression : Expression
	{
		private readonly Expression l;
		private readonly Expression r;

		public MultiplyExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return l.Evaluate(scope).AsNumber() * r.Evaluate(scope).AsNumber();
		}

		public override Expression Simplify()
		{
			if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new NumberLiteral(
					left.Value.AsNumber() * right.Value.AsNumber()
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
