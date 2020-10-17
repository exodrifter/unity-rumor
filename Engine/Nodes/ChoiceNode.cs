using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class ChoiceNode : Node
	{
		private string Label { get; }
		private Expression<StringValue> Text { get; }

		public ChoiceNode(string label, string text)
		{
			Label = label;
			Text = new StringLiteral(text);
		}

		public ChoiceNode(string label, Expression<StringValue> text)
		{
			Label = label;
			Text = text;
		}

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			rumor.State.AddChoice(Label, Text.Evaluate().Value);
			yield break;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ChoiceNode);
		}

		public bool Equals(ChoiceNode other)
		{
			if (other == null)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			return "choice [" + Label + "] > " + Text;
		}
	}
}
