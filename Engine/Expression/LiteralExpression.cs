namespace Exodrifter.Rumor.Engine
{
	public class LiteralExpression<T> : Expression<T>
	{
		private readonly T value;

		public LiteralExpression(T value)
		{
			this.value = value;
		}

		public override T Evaluate()
		{
			return value;
		}
	}
}
