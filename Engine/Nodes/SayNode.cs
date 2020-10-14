namespace Exodrifter.Rumor.Engine
{
	public class SayNode
	{
		public string Speaker { get; }
		public Expression<StringValue> Dialog { get; }

		public SayNode(string speaker, string dialog)
		{
			Speaker = speaker;
			Dialog = new LiteralExpression<StringValue>(
				new StringValue(dialog)
			);
		}

		public SayNode(string speaker, Expression<StringValue> dialog)
		{
			Speaker = speaker;
			Dialog = dialog;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as SayNode);
		}

		public bool Equals(SayNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Speaker == other.Speaker
				&& Dialog == other.Dialog;
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Speaker, Dialog);
		}

		public override string ToString()
		{
			return Speaker + ": " + Dialog;
		}
	}
}
