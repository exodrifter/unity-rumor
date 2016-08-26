using Exodrifter.Rumor.Engine;
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
		public readonly string text;

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
			this.text = text;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.State.AddChoice(text, this.Children);

			// Wait for a choice to be made if we're done adding choices
			if (!(rumor.Next is Choice)) {
				yield return new ForChoice();
			}
		}

		#region Serialization

		public Choice(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			text = (string)info.GetValue("text", typeof(string));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("text", text, typeof(string));
		}

		#endregion
	}
}
