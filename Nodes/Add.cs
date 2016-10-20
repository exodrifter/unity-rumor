using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Appends additional dialog to the rumor state.
	/// </summary>
	[Serializable]
	public sealed class Add : Say
	{
		/// <summary>
		/// Creates a new Add node.
		/// </summary>
		/// <param name="text">
		/// The text to append the dialog with.
		/// </param>
		public Add(string text) : base(text) {}

		/// <summary>
		/// Creates a new Add node.
		/// </summary>
		/// <param name="text">
		/// The expression to append the dialog with.
		/// </param>
		public Add(Expression text) : base(text) {}

		/// <summary>
		/// Creates a new Add node.
		/// </summary>
		/// <param name="speaker">
		/// The speaker to associate with the dialog.
		/// </param>
		/// <param name="text">
		/// The text to append the dialog with.
		/// </param>
		public Add(Expression speaker, string text) : base(speaker, text) {}

		/// <summary>
		/// Creates a new Add node.
		/// </summary>
		/// <param name="speaker">
		/// The speaker to associate with the dialog.
		/// </param>
		/// <param name="text">
		/// The expression to append the dialog with.
		/// </param>
		public Add(Expression speaker, Expression text) : base(speaker, text) {}

		/// <summary>
		/// Creates a new Add node.
		/// </summary>
		/// <param name="speaker">
		/// The speaker to associate with the dialog.
		/// </param>
		/// <param name="text">
		/// The text to append the dialog with.
		/// </param>
		public Add(object speaker, string text) : base(speaker, text) {}

		/// <summary>
		/// Creates a new Add node.
		/// </summary>
		/// <param name="speaker">
		/// The speaker to associate with the dialog.
		/// </param>
		/// <param name="text">
		/// The expression to append the dialog with.
		/// </param>
		public Add(object speaker, Expression text) : base(speaker, text) {}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			var dialog = rumor.State.Dialog;
			string text = EvaluateText(rumor);

			if (string.IsNullOrEmpty(dialog[speaker])
				|| dialog[speaker].EndsWith(" ")
				|| dialog[speaker].EndsWith("\t")
				|| dialog[speaker].EndsWith("\n")) {

				rumor.State.AddDialog(speaker, text);
			}
			else if (!string.IsNullOrEmpty(text)) {
				rumor.State.AddDialog(speaker, " " + text);
			}
			yield return new ForAdvance();
		}

		#region Serialization

		public Add(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		#endregion
	}
}
