using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Pauses execution for a certain amount of time.
	/// </summary>
	[Serializable]
	public sealed class Pause : Node
	{
		/// <summary>
		/// The number of seconds to pause for.
		/// </summary>
		public readonly Expression seconds;

		/// <summary>
		/// Creates a new pause node.
		/// </summary>
		/// <param name="seconds">
		/// The number of seconds to pause for.
		/// </param>
		public Pause(float seconds)
		{
			this.seconds = new LiteralExpression(seconds);
		}

		/// <summary>
		/// Creates a new pause node.
		/// </summary>
		/// <param name="seconds">
		/// The number of seconds to pause for.
		/// </param>
		public Pause(Expression expression)
		{
			this.seconds = expression;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			var value = seconds.Evaluate(rumor) ?? new ObjectValue(null);

			if (value.AsFloat() > 0) {
				yield return new ForSeconds(value.AsFloat());
			}
			else {
				yield return new ForAdvance();
			}
		}

		#region Serialization

		public Pause(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			seconds = info.GetValue<Expression>("seconds");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("seconds", seconds);
		}

		#endregion
	}
}
