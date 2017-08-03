using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an addition set operator that is used to assign a variable.
	/// </summary>
	[Serializable]
	public class SetAddExpression : OpExpression
	{
		public SetAddExpression(Expression left, Expression right)
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
			rumor.Scope.SetVar(variable.Name, l.Add(r));
			return r;
		}

		public override string ToString()
		{
			return left + "+=" + right;
		}

		#region Serialization

		public SetAddExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
