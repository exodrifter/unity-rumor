using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that does nothing.
	/// </summary>
	[Serializable]
	public class NoOpExpression : Expression
	{
		public NoOpExpression()
		{
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as NoOpExpression;
			if (other == null) {
				return false;
			}
			return true;
		}

		public bool Equals(NoOpExpression other)
		{
			if (other == null) {
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region Serialization

		public NoOpExpression
			(SerializationInfo info, StreamingContext context)
		{
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
		}

		#endregion
	}
}
