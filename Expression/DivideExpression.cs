using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a division operator that is used to divide two arguments.
	/// </summary>
	public class DivideExpression : Expression, ISerializable
	{
		private readonly Expression left;
		private readonly Expression right;

		public DivideExpression(Expression left, Expression right)
		{
			this.left = left;
			this.right = right;
		}

		public override object Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);

			if (l.GetType() == typeof(int) && r.GetType() == typeof(int)) {
				return (int)l / (int)r;
			}
			if (l.GetType() == typeof(int) && r.GetType() == typeof(float)) {
				return (int)l / (float)r;
			}
			if (l.GetType() == typeof(float) && r.GetType() == typeof(int)) {
				return (float)l / (int)r;
			}
			if (l.GetType() == typeof(float) && r.GetType() == typeof(float)) {
				return (float)l / (float)r;
			}

			throw new ArgumentException(
				string.Format("Cannot divide arguments of type {0} and {1}!",
					l.GetType(), r.GetType()));
		}

		public override string ToString()
		{
			return left + "/" + right;
		}

		#region Serialization

		public DivideExpression(SerializationInfo info, StreamingContext context)
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
