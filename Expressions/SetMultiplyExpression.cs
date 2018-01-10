using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an subtraction set operator that is used to assign a
	/// variable.
	/// </summary>
	[Serializable]
	public class SetMultiplyExpression : OpExpression
	{
		public SetMultiplyExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			if (left.GetType() != typeof(VariableExpression))
			{
				throw new ArgumentException(
					string.Format("Cannot assign values to type {0}!",
						left.GetType()));
			}

			var variable = left as VariableExpression;
			var l = left.Evaluate(scope, bindings);
			var r = right.Evaluate(scope, bindings);
			scope.SetVar(variable.Name, l.Multiply(r));
			return r;
		}

		public override string ToString()
		{
			return left + "*=" + right;
		}

		#region Serialization

		public SetMultiplyExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
