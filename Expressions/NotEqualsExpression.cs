using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an equals operator that is used to check the inequality of
	/// two arguments.
	/// </summary>
	[Serializable]
	public class NotEqualsExpression : OpExpression
	{
		public NotEqualsExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);
			return new BoolValue(!l.Equals(r));
		}

		public override string ToString()
		{
			return left + "!=" + right;
		}

		#region Serialization

		public NotEqualsExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
