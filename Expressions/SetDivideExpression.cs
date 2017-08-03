using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an subtraction set operator that is used to assign a
	/// variable.
	/// </summary>
	[Serializable]
	public class SetDivideExpression : OpExpression
	{
		public SetDivideExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Engine.Rumor rumor)
		{
			if (left.GetType() != typeof(VariableExpression))
			{
				throw new ArgumentException(
					string.Format("Cannot assign values to type {0}!",
						left.GetType()));
			}

			var variable = left as VariableExpression;
			var l = left.Evaluate(rumor);
			var r = right.Evaluate(rumor);
			rumor.Scope.SetVar(variable.Name, l.Divide(r));
			return r;
		}

		public override string ToString()
		{
			return left + "/=" + right;
		}

		#region Serialization

		public SetDivideExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
