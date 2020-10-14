namespace Exodrifter.Rumor.Engine
{
	public class ConcatExpression : Expression<StringValue>
	{
		private readonly Expression<StringValue> l;
		private readonly Expression<StringValue> r;

		public ConcatExpression(StringValue l, StringValue r)
		{
			this.l = new LiteralExpression<StringValue>(l);
			this.r = new LiteralExpression<StringValue>(r);
		}

		public ConcatExpression(StringValue l, Expression<StringValue> r)
		{
			this.l = new LiteralExpression<StringValue>(l);
			this.r = r;
		}

		public ConcatExpression(Expression<StringValue> l, StringValue r)
		{
			this.l = l;
			this.r = new LiteralExpression<StringValue>(r);
		}

		public ConcatExpression
			(Expression<StringValue> l, Expression<StringValue> r)
		{
			this.l = l;
			this.r = r;
		}

		public override StringValue Evaluate()
		{
			return l.Evaluate() + r.Evaluate();
		}
	}
}
