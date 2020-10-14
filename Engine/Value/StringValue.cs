namespace Exodrifter.Rumor.Engine
{
	public class StringValue : Value
	{
		public override ValueType Type => ValueType.String;

		public StringValue(string str) : base(str) { }

		public static StringValue operator +(StringValue l, StringValue r) =>
			new StringValue((string)l.InternalValue + (string)r.InternalValue);
	}
}
