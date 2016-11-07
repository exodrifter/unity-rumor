using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// A statement is a standalone expression which will be evaluated.
	/// </summary>
	[Serializable]
	public sealed class Statement : Node
	{
		/// <summary>
		/// The expression to evaluate.
		/// </summary>
		public readonly Expression expression;

		/// <summary>
		/// Creates a new Statement node.
		/// </summary>
		/// <param name="text">
		/// The text to append to the dialog.
		/// </param>
		public Statement(Expression expression)
		{
			this.expression = expression;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			expression.Evaluate(rumor);
			yield return null;
		}

		#region Serialization

		public Statement(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			expression = info.GetValue<Expression>("expression");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("expression", expression);
		}

		#endregion
	}
}
