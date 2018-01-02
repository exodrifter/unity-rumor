using System.Collections.Generic;
using Exodrifter.Rumor.Nodes;

namespace Exodrifter.Rumor.Language
{
	public static class Compiler
	{
		/// <summary>
		/// Compiles a Rumor script.
		/// </summary>
		public static List<Node> Compile(string script)
		{
			return new Parser().Compile(new Reader(script));
		}
	}
}
