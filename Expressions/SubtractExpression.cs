using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an add operator that is used to add two arguments.
	/// </summary>
	[Serializable]
	public class SubtractExpression : OpExpression
	{
		public SubtractExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			var l = left.Evaluate(scope, bindings) ?? new ObjectValue(null);
			var r = right.Evaluate(scope, bindings) ?? new ObjectValue(null);
			return l.Subtract(r);
		}

		public override string ToString()
		{
			return left + "-" + right;
		}

		#region Serialization

		public SubtractExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
