using Exodrifter.Rumor.Engine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	[Serializable]
	public sealed class Dialog : Node, ISerializable
	{
		public readonly string text;

		public Dialog(string text)
		{
			this.text = text;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			UnityEngine.Debug.Log(text);
			yield return new ForAdvance();
		}

		#region Serialization

		public Dialog(SerializationInfo info, StreamingContext context)
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
