using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an less than operator that is used to compare two arguments.
	/// </summary>
	[Serializable]
	public class LessThanOrEqualExpression : OpExpression
	{
		public LessThanOrEqualExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			var l = left.Evaluate(rumor) ?? new ObjectValue(null);
			var r = right.Evaluate(rumor) ?? new ObjectValue(null);
			return new BoolValue(l.LessThan(r).AsBool() || l.Equals(r));
		}

		public override string ToString()
		{
			return left + "<=" + right;
		}

		#region Serialization

		public LessThanOrEqualExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
