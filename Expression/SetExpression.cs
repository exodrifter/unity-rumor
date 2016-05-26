using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an set operator that is used to assign a variable.
	/// </summary>
	public class SetExpression : Expression, ISerializable
	{
		private readonly Expression left;
		private readonly Expression right;

		public SetExpression(Expression left, Expression right)
		{
			this.left = left;
			this.right = right;
		}

		public override object Evaluate(Scope scope)
		{
			if (left.GetType() != typeof(VariableExpression)) {
				throw new ArgumentException(
					string.Format("Cannot assign values to type {0}!",
						left.GetType()));
			}

			var l = left as VariableExpression;
			var r = right.Evaluate(scope);
			scope.SetVar(l.Name, r);
			return r;
		}

		public override string ToString()
		{
			return left + "+" + right;
		}

		#region Serialization

		public SetExpression(SerializationInfo info, StreamingContext context)
		{
			left = (Expression)info.GetValue("left", typeof(Expression));
			right = (Expression)info.GetValue("right", typeof(Expression));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("left", left, typeof(Expression));
			info.AddValue("right", right, typeof(Expression));
		}

		#endregion
	}
}
