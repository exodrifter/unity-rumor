using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a boolean "xor" operator.
	/// </summary>
	[Serializable]
	public class BoolXorExpression : OpExpression
	{
		public BoolXorExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			var l = left.Evaluate(rumor) ?? new ObjectValue(null);
			var r = right.Evaluate(rumor) ?? new ObjectValue(null);
			return l.BoolXor(r);
		}

		public override string ToString()
		{
			return left + " xor " + right;

		}

		#region Serialization

		public BoolXorExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
