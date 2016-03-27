using Exodrifter.Rumor.Engine;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Nodes
{
	public class Dialog : Node
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
	}
}
