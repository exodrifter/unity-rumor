namespace Exodrifter.Rumor.Engine
{
	public class StringValue : Value
	{
		public string Value { get { return (string)InternalValue; } }
		public override ValueType Type => ValueType.String;

		public StringValue(string str) : base(str) { }

		public static StringValue operator +(StringValue l, StringValue r) =>
			new StringValue(l.Value + r.Value);
	}
}
