namespace Exodrifter.Rumor.Engine
{
	public class LiteralExpression<T> : Expression<T> where T : Value
	{
		public readonly T Value;

		public LiteralExpression(T value)
		{
			Value = value;
		}

		public override T Evaluate()
		{
			return Value;
		}

		public override Expression<T> Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as LiteralExpression<T>);
		}

		public bool Equals(LiteralExpression<T> other)
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
