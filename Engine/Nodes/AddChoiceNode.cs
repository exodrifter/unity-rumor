using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class AddChoiceNode : Node
	{
		private string Label { get; }
		private Expression<StringValue> Text { get; }

		public AddChoiceNode(string label, string text)
		{
			Label = label;
			Text = new StringLiteral(text);
		}

		public AddChoiceNode(string label, Expression<StringValue> text)
		{
			Label = label;
			Text = text;
		}

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			rumor.State.AddChoice(Label, Text.Evaluate(rumor.Scope).Value);
			yield break;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as AddChoiceNode);
		}

		public bool Equals(AddChoiceNode other)
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
