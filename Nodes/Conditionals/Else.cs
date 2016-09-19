using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// An else conditional statement.
	/// </summary>
	[Serializable]
	public sealed class Else : Conditional
	{
		/// <summary>
		/// Creates a new else block.
		/// </summary>
		/// <param name="children">The children of the else.</param>
		public Else(IEnumerable<Node> children)
			: base(null, children)
		{
		}

		public override IEnumerator<RumorYield> Exec(Engine.Rumor rumor)
		{
			rumor.EnterBlock(Children);
			yield return null;
		}

		#region Serialization

		public Else(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
		}

		#endregion
	}
}
