namespace Exodrifter.Rumor.Engine
{
	public class NumberValue : Value
	{
		public override ValueType Type => ValueType.Number;

		public NumberValue(double n) : base(n) { }

		public static NumberValue operator +(NumberValue n) => n;
		public static NumberValue operator -(NumberValue n) =>
			new NumberValue(-(double)n.InternalValue);

		public static NumberValue operator +(NumberValue l, NumberValue r) =>
			new NumberValue((double)l.InternalValue + (double)r.InternalValue);
		public static NumberValue operator -(NumberValue l, NumberValue r) =>
			new NumberValue((double)l.InternalValue - (double)r.InternalValue);
		public static NumberValue operator *(NumberValue l, NumberValue r) =>
			new NumberValue((double)l.InternalValue * (double)r.InternalValue);
		public static NumberValue operator /(NumberValue l, NumberValue r) =>
			new NumberValue((double)l.InternalValue / (double)r.InternalValue);
	}
}
