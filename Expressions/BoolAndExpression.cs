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

		public override Value Evaluate(Engine.Rumor rumor)
		{
			var l = left.Evaluate(rumor);
			var r = right.Evaluate(rumor);
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
