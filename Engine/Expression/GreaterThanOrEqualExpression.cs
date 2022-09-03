﻿using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class GreaterThanOrEqualExpression : Expression, ISerializable
	{
		internal readonly Expression l;
		internal readonly Expression r;

		public GreaterThanOrEqualExpression(Expression l, Expression r)
		{
			this.l = l;
			this.r = r;
		}

		public override Value Evaluate(RumorScope scope, RumorBindings bindings)
		{
			return new BooleanValue(
				l.Evaluate(scope, bindings).AsNumber() >= r.Evaluate(scope, bindings).AsNumber()
			);
		}

		public override Expression Simplify()
		{
			if (l is NumberLiteral && r is NumberLiteral)
			{
				var left = l as NumberLiteral;
				var right = r as NumberLiteral;

				return new BooleanLiteral(
					left.Value.AsNumber() >= right.Value.AsNumber()
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
					return new GreaterThanOrEqualExpression(left, right).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as GreaterThanOrEqualExpression);
		}

		public bool Equals(GreaterThanOrEqualExpression other)
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

		public GreaterThanOrEqualExpression
			(SerializationInfo info, StreamingContext context)
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
			return "(" + l.ToString() + " >= " + r.ToString() + ")";
		}
	}
}
