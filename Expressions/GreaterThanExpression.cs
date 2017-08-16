using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an less than operator that is used to compare two arguments.
	/// </summary>
	[Serializable]
	public class GreaterThanExpression : OpExpression
	{
		public GreaterThanExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			var l = left.Evaluate(rumor) ?? new ObjectValue(null);
			var r = right.Evaluate(rumor) ?? new ObjectValue(null);
			return l.GreaterThan(r);
		}

		public override string ToString()
		{
			return left + ">" + right;
		}

		#region Serialization

		public GreaterThanExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
