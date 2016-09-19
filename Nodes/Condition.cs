using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Enters a block if the conditional evaluates as true.
	/// </summary>
	[Serializable]
	public sealed class Condition : Node
	{
		/// <summary>
		/// The conditional to evaluate.
		/// </summary>
		public readonly Conditional conditional;

		/// <summary>
		/// Creates a new condition node.
		/// </summary>
		/// <param name="conditional">
		/// The conditional to evaluate.
		/// </param>
		public Condition(Conditional conditional)
		{
			this.conditional = conditional;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			return conditional.Exec(rumor);
		}

		#region Serialization

		public Condition(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			conditional = info.GetValue<Conditional>("conditional");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Conditional>("conditional", conditional);
		}

		#endregion
	}
}
