using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class Rumor
	{
		/// <summary>
		/// The identifier used for the main entry point of a script.
		/// </summary>
		public const string MainIdentifier = "_main";

		public Dictionary<string, List<Node>> Nodes { get; }

		public Rumor(Dictionary<string, List<Node>> nodes)
		{
			Nodes = nodes;
		}
	}
}