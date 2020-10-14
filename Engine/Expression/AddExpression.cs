namespace Exodrifter.Rumor.Engine
{
	public class AddExpression : Expression<NumberValue>
	{
		private readonly Expression<NumberValue> l;
		private readonly Expression<NumberValue> r;

		public AddExpression(NumberValue l, NumberValue r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public AddExpression(NumberValue l, Expression<NumberValue> r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = r;
		}

		public AddExpression(Expression<NumberValue> l, NumberValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public AddExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override NumberValue Evaluate()
		{
			return l.Evaluate() + r.Evaluate();
		}
	}
}
