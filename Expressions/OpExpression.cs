using Exodrifter.Rumor.Util;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	public abstract class OpExpression : Expression
	{
		/// <summary>
		/// The left-hand argument for this expression.
		/// </summary>
		public Expression Left { get { return left; } }
		protected readonly Expression left;

		/// <summary>
		/// The right-hand argument for this expression.
		/// </summary>
		public Expression Right { get { return right; } }
		protected readonly Expression right;

		protected OpExpression(Expression left, Expression right)
		{
			this.left = left;
			this.right = right;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as OpExpression;
			if (other == null) {
				return false;
			}
			return other.left == left && other.right == right;
		}

		public bool Equals(OpExpression other)
		{
			if (other == null) {
				return false;
			}
			return other.left == left && other.right == right;
		}

		public override int GetHashCode()
		{
			unchecked {
				int hash = 17;
				hash = hash * 31 + left.GetHashCode();
				hash = hash * 31 + right.GetHashCode();
				return hash;
			}
		}

		#endregion

		#region Serialization

		public OpExpression(SerializationInfo info, StreamingContext context)
		{
			left = info.GetValue<Expression>("left");
			right = info.GetValue<Expression>("right");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("left", left);
			info.AddValue<Expression>("right", right);
		}

		#endregion
	}
}
