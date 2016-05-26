using Exodrifter.Rumor.Engine;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a variable.
	/// </summary>
	public class VariableExpression : Expression
	{
		/// <summary>
		/// The name of the variable
		/// </summary>
		public string Name { get; private set; }

		public VariableExpression(string str)
		{
			Name = str;
		}

		public override object Evaluate(Scope scope)
		{
			return scope.GetVar (Name);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
