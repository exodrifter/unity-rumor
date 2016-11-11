using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an add operator that is used to add two arguments.
	/// </summary>
	[Serializable]
	public class AddExpression : OpExpression
	{
		public AddExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			var l = left.Evaluate(rumor) ?? new ObjectValue(null);
			var r = right.Evaluate(rumor) ?? new ObjectValue(null);

			return l.Add(r);
		}

		public override string ToString()
		{
			return left + "+" + right;
		}

		#region Serialization

		public AddExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
