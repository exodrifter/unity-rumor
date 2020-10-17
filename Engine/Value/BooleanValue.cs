namespace Exodrifter.Rumor.Engine
{
	public class BooleanValue : Value
	{
		public bool Value { get { return (bool)InternalValue; } }
		public override ValueType Type => ValueType.Boolean;

		public BooleanValue(bool b) : base(b) { }

		public static BooleanValue operator |(BooleanValue l, BooleanValue r) =>
			new BooleanValue(l.Value | r.Value);
		public static BooleanValue operator &(BooleanValue l, BooleanValue r) =>
			new BooleanValue(l.Value & r.Value);
		public static BooleanValue operator ^(BooleanValue l, BooleanValue r) =>
			new BooleanValue(l.Value ^ r.Value);
		public static BooleanValue operator !(BooleanValue l) =>
			new BooleanValue(!l.Value);

		public static bool operator true(BooleanValue x) => x.Value;
		public static bool operator false(BooleanValue x) => !x.Value;

		public override string ToString()
		{
			return Value ? "true" : "false";
		}
	}
}
