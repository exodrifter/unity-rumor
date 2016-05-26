using Exodrifter.Rumor.Engine;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that does nothing.
	/// </summary>
	public class NoOpExpression : Expression
	{
		public NoOpExpression()
		{
		}

		public override object Evaluate(Scope scope)
		{
			return null;
		}
	}
}
