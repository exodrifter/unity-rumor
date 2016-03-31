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
	public class Choice : Node, ISerializable
	{
		public readonly string text;

		public Choice(string text, List<Node> children)
			: base(children)
		{
			this.text = text;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.State.AddChoice(text, this.children);

			// Wait for a choice to be made if we're done adding choices
			if (!(rumor.Next is Choice)) {
				yield return new ForChoice();
			}
		}

		#region Serialization

		public Choice(SerializationInfo info, StreamingContext context)
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
