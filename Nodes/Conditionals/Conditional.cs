using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// A conditional statement.
	/// </summary>
	[Serializable]
	public abstract class Conditional : Block
	{
		/// <summary>
		/// The expression to evaluate.
		/// </summary>
		private readonly Expression expression;

		/// <summary>
		/// Creates a new conditional.
		/// </summary>
		/// <param name="children">
		/// The children of the conditional statement.
		/// </param>
		public Conditional(Expression expression, IEnumerable<Node> children)
			: base(children)
		{
			this.expression = expression;
		}

		/// <summary>
		/// Returns the value of the expression evaluated against the scope in
		/// the passed Rumor.
		/// </summary>
		/// <param name="rumor">
		/// The rumor to evaluate the expression against.
		/// </param>
		/// <returns>The value of the expression.</returns>
		protected Value Evaluate(Engine.Rumor rumor)
		{
			return expression.Evaluate(rumor);
		}
		
		/// <summary>
		/// Runs the current conditional.
		/// </summary>
		/// <returns>
		/// A IEnumerator that can be used to continue execution of the
		/// conditional.
		/// </returns>
		public abstract IEnumerator<RumorYield> Exec(Engine.Rumor rumor);

		#region Serialization

		public Conditional(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			expression = info.GetValue<Expression>("expression");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("expression", expression);
		}

		#endregion
	}
}
