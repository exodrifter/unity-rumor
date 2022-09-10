using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class AddChoiceNode : Node, ISerializable
	{
		public string Label { get; }
		public Expression Text { get; }

		public AddChoiceNode(string label, string text)
		{
			Label = label;
			Text = new StringLiteral(text);
		}

		public AddChoiceNode(string label, Expression text)
		{
			Label = label;
			Text = text;
		}

		public override Yield Execute(Rumor rumor)
		{
			rumor.State.AddChoice(
				Label,
				Text.Evaluate(rumor.Scope, rumor.Bindings).AsString().Value
			);
			return null;
		}

		#region Equality

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

			return this.Label == other.Label
				&& this.Text == other.Text;
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode("add", Label, Text);
		}

		#endregion

		#region Serialization

		public AddChoiceNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Label = info.GetValue<string>("label");
			Text = info.GetValue<Expression>("text");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("label", Label);
			info.AddValue<Expression>("text", Text);
		}

		#endregion

		public override string ToString()
		{
			return "choice [" + Label + "] > " + Text;
		}
	}
}
