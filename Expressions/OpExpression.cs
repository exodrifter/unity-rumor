﻿using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	public abstract class OpExpression : Expression, ISerializable
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