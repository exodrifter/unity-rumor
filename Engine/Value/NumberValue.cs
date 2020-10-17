namespace Exodrifter.Rumor.Engine
{
	public class NumberValue : Value
	{
		public double Value { get { return (double)InternalValue; } }
		public override ValueType Type => ValueType.Number;

		public NumberValue(double n) : base(n) { }

		public static NumberValue operator +(NumberValue n) => n;
		public static NumberValue operator -(NumberValue n) =>
			new NumberValue(-n.Value);

		public static NumberValue operator +(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value + r.Value);
		public static NumberValue operator -(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value - r.Value);
		public static NumberValue operator *(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value * r.Value);
		public static NumberValue operator /(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value / r.Value);
	}
}
