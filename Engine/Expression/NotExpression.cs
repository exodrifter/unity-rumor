using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	public class NotExpression : Expression
	{
		private readonly Expression expression;

		public NotExpression(Expression expression)
		{
			this.expression = expression;
		}

		public override Value Evaluate(RumorScope scope)
		{
			return !expression.Evaluate(scope).AsBoolean();
		}

		public override Expression Simplify()
		{
			if (expression is BooleanLiteral)
			{
				var e = expression as BooleanLiteral;
				return new BooleanLiteral(!e.Value.AsBoolean());
			}
			else
			{
				var e = expression.Simplify();

				if (e == expression)
				{
					return this;
				}
				else
				{
					return new NotExpression(e).Simplify();
				}
			}
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as NotExpression);
		}

		public bool Equals(NotExpression other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(expression, other.expression);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(expression);
		}

		#endregion

		#region Serialization

		public NotExpression(SerializationInfo info, StreamingContext context)
		{
			expression = info.GetValue<Expression>("expression");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("expression", expression);
		}

		#endregion

		public override string ToString()
		{
			return "(not " + expression.ToString() + ")";
		}
	}
}
