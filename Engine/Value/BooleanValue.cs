namespace Exodrifter.Rumor.Engine
{
	public class BooleanValue : Value
	{
		public override ValueType Type => ValueType.Boolean;

		public BooleanValue(bool b) : base(b) { }
	}
}
