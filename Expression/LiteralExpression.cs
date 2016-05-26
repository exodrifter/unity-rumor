using Exodrifter.Rumor.Engine;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a literal.
	/// </summary>
	public class LiteralExpression : Expression
	{
		public object Value { get; private set; }

		public LiteralExpression(string str)
		{
			Value = str;
		}

		public LiteralExpression(int num)
		{
			Value = num;
		}

		public LiteralExpression(float num)
		{
			Value = num;
		}

		public override object Evaluate(Scope scope)
		{
			return Value;
		}

		public override string ToString()
		{
			if (Value != null)
				return Value.ToString();
			return "";
		}
	}
}
