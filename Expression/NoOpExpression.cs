using Exodrifter.Rumor.Engine;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that does nothing.
	/// </summary>
	public class NoOpExpression : Expression, ISerializable
	{
		public NoOpExpression()
		{
		}

		public override object Evaluate(Scope scope)
		{
			return null;
		}

		#region Serialization

		public NoOpExpression(SerializationInfo info, StreamingContext context)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}

		#endregion
	}
}
