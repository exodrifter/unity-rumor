using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Adds a choice to the rumor state.
	/// </summary>
	[Serializable]
	public class Choice : Node
	{
		/// <summary>
		/// The text to display for this choice.
		/// </summary>
		private readonly Expression text;

		/// <summary>
		/// Creates a new choice
		/// </summary>
		/// <param name="text">
		/// The text to display for this choice.
		/// </param>
		/// <param name="children">
		/// The children for this choice.
		/// </param>
		public Choice(string text, IEnumerable<Node> children)
			: base(children)
		{
			this.text = new LiteralExpression(text);
		}

		/// <summary>
		/// Creates a new choice
		/// </summary>
		/// <param name="text">
		/// The text to display for this choice.
		/// </param>
		/// <param name="children">
		/// The children for this choice.
		/// </param>
		public Choice(Expression text, IEnumerable<Node> children)
			: base(children)
		{
			this.text = text;
		}

		public string EvaluateText(Engine.Rumor rumor)
		{
			if (text == null) {
				return "";
			}
			var value = text.Evaluate(rumor) ?? new ObjectValue(null);
			return (value.AsObject() ?? "").ToString();
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.State.AddChoice(EvaluateText(rumor), this.Children);
			yield return null;
		}

		#region Serialization

		public Choice(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			text = info.GetValue<Expression>("text");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("text", text);
		}

		#endregion
	}
}
