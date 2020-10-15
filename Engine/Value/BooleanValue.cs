namespace Exodrifter.Rumor.Engine
{
	public class BooleanValue : Value
	{
		public override ValueType Type => ValueType.Boolean;

		public BooleanValue(bool b) : base(b) { }

		public static BooleanValue operator |(BooleanValue l, BooleanValue r) =>
			new BooleanValue((bool)l.InternalValue | (bool)r.InternalValue);

		public static BooleanValue operator &(BooleanValue l, BooleanValue r) =>
			new BooleanValue((bool)l.InternalValue & (bool)r.InternalValue);

		public static BooleanValue operator ^(BooleanValue l, BooleanValue r) =>
			new BooleanValue((bool)l.InternalValue ^ (bool)r.InternalValue);

		public static bool operator true(BooleanValue x)
		{
			return (bool)x.InternalValue;
		}

		public static bool operator false(BooleanValue x)
		{
			return !(bool)x.InternalValue;
		}
	}
}
