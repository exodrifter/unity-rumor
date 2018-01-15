using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an set operator that is used to assign a variable.
	/// </summary>
	[Serializable]
	public class SetExpression : OpExpression
	{
		public SetExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			if (left.GetType() != typeof(VariableExpression)) {
				throw new ArgumentException(
					string.Format("Cannot assign values to type {0}!",
						left.GetType()));
			}

			var l = left as VariableExpression;
			var r = right.Evaluate(scope, bindings);
			scope.SetVar(l.Name, r);
			return r;
		}

		public override string ToString()
		{
			return left + "=" + right;
		}

		#region Serialization

		public SetExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
