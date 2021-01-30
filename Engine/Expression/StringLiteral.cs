namespace Exodrifter.Rumor.Engine
{
	public class StringLiteral : LiteralExpression
	{
		public StringLiteral(StringValue value) : base(value) { }
		public StringLiteral(string value) : base(new StringValue(value)) { }

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as StringLiteral);
		}

		public bool Equals(StringLiteral other)
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
