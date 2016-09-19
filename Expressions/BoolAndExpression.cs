using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a boolean "and" operator.
	/// </summary>
	[Serializable]
	public class BoolAndExpression : OpExpression
	{
		public BoolAndExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);
			return l.BoolAnd(r);
		}

		public override string ToString()
		{
			return left + " and " + right;

		}

		#region Serialization

		public BoolAndExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
