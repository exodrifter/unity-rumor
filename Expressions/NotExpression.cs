using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a boolean "not" operator.
	/// </summary>
	[Serializable]
	public class NotExpression : OpExpression
	{
		public NotExpression(Expression right)
			: base(new NoOpExpression(), right)
		{
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			var r = right.Evaluate(scope, bindings);
			if (r == null) {
				return new BoolValue(true);
			}
			return r.Not();
		}

		public override string ToString()
		{
			return "!" + right;

		}

		#region Serialization

		public NotExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
