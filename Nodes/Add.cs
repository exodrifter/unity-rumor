using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Appends additional dialog to the rumor state.
	/// </summary>
	[Serializable]
	public sealed class Add : Node, ISerializable
	{
		public readonly string text;

		public Add(string text)
		{
			this.text = text;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			string dialog = rumor.State.Dialog;
			if (string.IsNullOrEmpty(dialog)
				|| dialog.EndsWith(" ")
				|| dialog.EndsWith("\t")
				|| dialog.EndsWith("\n")) {

				rumor.State.AddDialog(text);
			}
			else if (!string.IsNullOrEmpty(text)) {
				rumor.State.AddDialog(" " + text);
			}
			yield return new ForAdvance();
		}

		#region Serialization

		public Add(SerializationInfo info, StreamingContext context)
		{
			text = (string)info.GetValue("text", typeof(string));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("text", text, typeof(string));
		}

		#endregion
	}
}
