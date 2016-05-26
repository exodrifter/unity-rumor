using Exodrifter.Rumor.Engine;
using System;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a multiplication operator that is used to multiply two
	/// arguments.
	/// </summary>
	public class SubtractExpression : Expression
	{
		private readonly Expression left;
		private readonly Expression right;

		public SubtractExpression(Expression left, Expression right)
		{
			this.left = left;
			this.right = right;
		}

		public override object Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);

			if (l.GetType() == typeof(int) && r.GetType() == typeof(int)) {
				return (int)l - (int)r;
			}
			if (l.GetType() == typeof(int) && r.GetType() == typeof(float)) {
				return (int)l - (float)r;
			}
			if (l.GetType() == typeof(float) && r.GetType() == typeof(int)) {
				return (float)l - (int)r;
			}
			if (l.GetType() == typeof(float) && r.GetType() == typeof(float)) {
				return (float)l - (float)r;
			}

			throw new ArgumentException(
				string.Format("Cannot subtract arguments of type {0} and {1}!",
					l.GetType(), r.GetType()));
		}

		public override string ToString()
		{
			return left + "-" + right;
		}
	}
}
