using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a multiplication operator that is used to multiply two
	/// arguments.
	/// </summary>
	public class SubtractExpression : Expression, ISerializable
	{
		private readonly Expression left;
		private readonly Expression right;

		public SubtractExpression(Expression left, Expression right)
		{
			this.left = left;
			this.right = right;
		}

		public override Value Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);
			return l.Subtract(r);
		}

		public override string ToString()
		{
			return left + "-" + right;
		}

		#region Serialization

		public SubtractExpression(SerializationInfo info, StreamingContext context)
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
