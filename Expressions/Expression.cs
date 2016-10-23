using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// An expression is a set of operations that returns a result.
	/// </summary>
	[Serializable]
	public abstract class Expression : ISerializable
	{
		public Expression() { }

		/// <summary>
		/// Evaluates this expression.
		/// </summary>
		/// <param name="scope">The current execution scope.</param>
		/// <returns>The result of this expression when evaluated.</returns>
		public abstract Value Evaluate(Scope scope);

		#region Equality

		public abstract override bool Equals(object obj);

		public abstract override int GetHashCode();

		public static bool operator ==(Expression l, Expression r)
		{
			if (ReferenceEquals(l, r)) {
				return true;
			}
			if ((object)l == null || (object)r == null) {
				return false;
			}
			return l.Equals(r);
		}

		public static bool operator !=(Expression a, Expression b)
		{
			return !(a == b);
		}

		#endregion

		#region Serialization

		public Expression(SerializationInfo info, StreamingContext context)
		{
		}

		public abstract void GetObjectData
			(SerializationInfo info, StreamingContext context);

		#endregion
	}
}
