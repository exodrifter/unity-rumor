using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ConcatExpression : Expression, ISerializable
	{
		private readonly Expression l;
		private readonly Expression r;

		public ConcatExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope, RumorBindings bindings)
		{
			return l.Evaluate(scope, bindings).AsString() + r.Evaluate(scope, bindings).AsString();
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

				if (left is StringLiteral)
				{
					var leftStr = (string)((StringLiteral)left).Value.InternalValue;
					if (string.IsNullOrEmpty(leftStr)) {
						return right;
					}
				}
				else if (right is StringLiteral)
				{
					var rightStr = (string)((StringLiteral)right).Value.InternalValue;
					if (string.IsNullOrEmpty(rightStr)) {
						return left;
					}
				}
				
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

		#region Serialization

		public ConcatExpression(SerializationInfo info, StreamingContext context)
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
			return "(" + l.ToString() + " + " + r.ToString() + ")";
		}
	}
}
