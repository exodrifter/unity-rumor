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
		/// If true, ignore advances until the pause is over.
		/// </summary>
		public readonly bool cantSkip;

		/// <summary>
		/// Creates a new pause node.
		/// </summary>
		/// <param name="seconds">
		/// The number of seconds to pause for.
		/// </param>
		/// <param name="canSkip">
		/// True if an advance will NOT skip the pause.
		/// </param>
		public Pause(float seconds, bool cantSkip)
		{
			this.seconds = new LiteralExpression(seconds);
			this.cantSkip = cantSkip;
		}

		/// <summary>
		/// Creates a new pause node.
		/// </summary>
		/// <param name="seconds">
		/// The number of seconds to pause for.
		/// </param>
		public Pause(Expression expression, bool cantSkip)
		{
			this.seconds = expression;
			this.cantSkip = cantSkip;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			var value = seconds.Evaluate(rumor) ?? new ObjectValue(null);

			if (value.AsFloat() > 0) {
				yield return new ForSeconds(value.AsFloat(), cantSkip);
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
			cantSkip = info.GetValue<bool>("cantSkip");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("seconds", seconds);
			info.AddValue<bool>("cantSkip", cantSkip);
		}

		#endregion
	}
}
