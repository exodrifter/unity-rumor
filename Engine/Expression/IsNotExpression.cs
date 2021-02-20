using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class IsNotExpression : Expression, ISerializable
	{
		internal readonly Expression l;
		internal readonly Expression r;

		public IsNotExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return new BooleanValue(l.Evaluate(scope) != r.Evaluate(scope));
		}

		public override Expression Simplify()
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
					return new IsNotExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as IsNotExpression);
		}

		public bool Equals(IsNotExpression other)
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

		#region Serialization

		public IsNotExpression(SerializationInfo info, StreamingContext context)
		{
			l = info.GetValue<Expression>("l");
			r = info.GetValue<Expression>("r");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("l", l);
			info.AddValue<Expression>("r", r);
		}

		#endregion

		public override string ToString()
		{
			return "(" + l.ToString() + " is not " + r.ToString() + ")";
		}
	}
}
