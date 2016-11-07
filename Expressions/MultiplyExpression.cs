using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an add operator that is used to add two arguments.
	/// </summary>
	[Serializable]
	public class MultiplyExpression : OpExpression
	{
		public MultiplyExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			var l = left.Evaluate(rumor);
			var r = right.Evaluate(rumor);
			return l.Multiply(r);
		}

		public override string ToString()
		{
			return left + "*" + right;
		}

		#region Serialization

		public MultiplyExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
