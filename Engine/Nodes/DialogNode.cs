namespace Exodrifter.Rumor.Engine
{
	public abstract class DialogNode : Node
	{
		public string Speaker { get; }
		public Expression<StringValue> Dialog { get; }

		public DialogNode(string speaker, string dialog)
		{
			Speaker = speaker;
			Dialog = new StringLiteral(dialog);
		}

		public DialogNode(string speaker, Expression<StringValue> dialog)
		{
			Speaker = speaker;
			Dialog = dialog;
		}
	}
}
