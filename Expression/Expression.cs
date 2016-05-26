using Exodrifter.Rumor.Engine;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// An expression is a set of operations that returns a result.
	/// </summary>
	public abstract class Expression
	{
		/// <summary>
		/// Evaluates this expression.
		/// </summary>
		/// <param name="scope">The current execution scope.</param>
		/// <returns>The result of this expression when evaluated.</returns>
		public abstract object Evaluate(Scope scope);
	}
}
