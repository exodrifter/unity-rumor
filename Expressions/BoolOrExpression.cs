using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a boolean "or" operator.
	/// </summary>
	[Serializable]
	public class BoolOrExpression : OpExpression
	{
		public BoolOrExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);
			return l.BoolOr(r);
		}

		public override string ToString()
		{
			return left + " or " + right;

		}

		#region Serialization

		public BoolOrExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
