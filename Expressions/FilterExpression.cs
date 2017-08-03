using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a "pipe" or filter operator.
	/// </summary>
	[Serializable]
	public class FilterExpression : OpExpression
	{
		public FilterExpression(Expression left, FunctionExpression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			return ((FunctionExpression)right).Invoke(null, rumor, left);
		}

		public override string ToString()
		{
			return left + " | " + right;
		}

		#region Serialization

		public FilterExpression
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
