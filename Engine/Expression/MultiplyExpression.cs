namespace Exodrifter.Rumor.Engine
{
	public class MultiplyExpression : Expression<NumberValue>
	{
		private readonly Expression<NumberValue> l;
		private readonly Expression<NumberValue> r;

		public MultiplyExpression(NumberValue l, NumberValue r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public MultiplyExpression(NumberValue l, Expression<NumberValue> r)
		{
			this.l = new LiteralExpression<NumberValue>(l);
			this.r = r;
		}

		public MultiplyExpression(Expression<NumberValue> l, NumberValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<NumberValue>(r);
		}

		public MultiplyExpression
			(Expression<NumberValue> l, Expression<NumberValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override NumberValue Evaluate()
		{
			return l.Evaluate() * r.Evaluate();
		}
	}
}
