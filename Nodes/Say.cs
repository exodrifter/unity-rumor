using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Sets the dialog in the rumor state.
	/// </summary>
	[Serializable]
	public class Say : Node
	{
		/// <summary>
		/// The speaker to associate with the dialog.
		/// </summary>
		public Expression speaker;

		/// <summary>
		/// The text to replace the dialog with.
		/// </summary>
		private readonly Expression text;

		/// <summary>
		/// True if the dialog should auto-advance itself.
		/// </summary>
		private readonly bool noWait;

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="text">
		/// The text to replace the dialog with.
		/// </param>
		/// <param name="noWait">
		/// True if the dialog should auto-advance itself.
		/// </param>
		public Say(string text, bool noWait = false)
		{
			this.speaker = null;
			this.text = new LiteralExpression(text);
			this.noWait = noWait;
		}

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="text">
		/// The expression to replace the dialog with.
		/// </param>
		/// <param name="noWait">
		/// True if the dialog should auto-advance itself.
		/// </param>
		public Say(Expression text, bool noWait = false)
		{
			this.speaker = null;
			this.text = text;
			this.noWait = noWait;
		}

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="speaker">
		/// The speaker to associate with the dialog.
		/// </param>
		/// <param name="text">
		/// The text to replace the dialog with.
		/// </param>
		/// <param name="noWait">
		/// True if the dialog should auto-advance itself.
		/// </param>
		public Say(object speaker, string text, bool noWait = false)
		{
			this.speaker = new LiteralExpression(speaker);
			this.text = new LiteralExpression(text);
			this.noWait = noWait;
		}

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="speaker">
		/// The speaker to associate with the dialog.
		/// </param>
		/// <param name="text">
		/// The expression to replace the dialog with.
		/// </param>
		/// <param name="noWait">
		/// True if the dialog should auto-advance itself.
		/// </param>
		public Say(Expression speaker, Expression text, bool noWait = false)
		{
			this.speaker = speaker;
			this.text = text;
			this.noWait = noWait;
		}

		/// <summary>
		/// Evaluates the speaker of this node in the specified Rumor.
		/// </summary>
		/// <param name="rumor">The Rumor to evaluate against.</param>
		/// <returns>The speaker.</returns>
		public object EvaluateSpeaker(Engine.Rumor rumor)
		{
			if (speaker == null) {
				return rumor.Scope.DefaultSpeaker;
			}
			var ret = speaker.Evaluate(rumor) ?? new ObjectValue(null);
			return ret.AsObject() ?? rumor.Scope.DefaultSpeaker;
		}

		/// <summary>
		/// Evaluates the text of this node in the specified Rumor.
		/// </summary>
		/// <param name="rumor">The Rumor to evaluate against.</param>
		/// <returns>The text.</returns>
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
			var speaker = EvaluateSpeaker(rumor);
			var text = EvaluateText(rumor);
			rumor.State.SetDialog(speaker, text);

			if (!noWait)
			{
				yield return new ForAdvance();
			}
		}

		#region Serialization

		public Say(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			speaker = info.GetValue<Expression>("speaker");
			text = info.GetValue<Expression>("text");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("speaker", speaker);
			info.AddValue<Expression>("text", text);
		}

		#endregion
	}
}
