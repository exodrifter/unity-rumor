using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Sets the dialog in the rumor state.
	/// </summary>
	[Serializable]
	public sealed class Say : Node, ISerializable
	{
		/// <summary>
		/// The text to replace the dialog with.
		/// </summary>
		public readonly string text;

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="text">
		/// The text to replace the dialog with.
		/// </param>
		public Say(string text)
		{
			this.text = text;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.State.SetDialog(text);
			yield return new ForAdvance();
		}

		#region Serialization

		public Say(SerializationInfo info, StreamingContext context)
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
