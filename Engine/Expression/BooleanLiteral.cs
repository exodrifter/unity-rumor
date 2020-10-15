namespace Exodrifter.Rumor.Engine
{
	public class BooleanLiteral : LiteralExpression<BooleanValue>
	{
		public BooleanLiteral(BooleanValue value) : base(value) { }
		public BooleanLiteral(bool value) : base(new BooleanValue(value)) { }

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as BooleanLiteral);
		}

		public bool Equals(BooleanLiteral other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Value);
		}

		#endregion

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
