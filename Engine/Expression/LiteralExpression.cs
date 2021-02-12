namespace Exodrifter.Rumor.Engine
{
	public abstract class LiteralExpression : Expression
	{
		public readonly Value Value;

		public LiteralExpression(Value value)
		{
			Value = value;
		}

		public override Value Evaluate(RumorScope _)
		{
			return Value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as LiteralExpression);
		}

		public bool Equals(LiteralExpression other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Value);
		}

		#endregion

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
