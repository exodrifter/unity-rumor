using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// An if conditional statement.
	/// </summary>
	[Serializable]
	public sealed class If : Conditional
	{
		/// <summary>
		/// The chained conditional statement to evaluate if this statement
		/// evaluates as false.
		/// </summary>
		public readonly Conditional elif;

		/// <summary>
		/// Creates a new if block.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		public If(Expression expression, IEnumerable<Node> children)
			: base(expression, children)
		{
			this.elif = null;
		}

		/// <summary>
		/// Creates a new if block with a chained else if statement.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		/// <param name="elif">The chained if statement.</param>
		public If(Expression expression, IEnumerable<Node> children, If elif)
			: base(expression, children)
		{
			this.elif = elif;
		}

		/// <summary>
		/// Creates a new if block with a chained else statement.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		/// <param name="else">The chained else statement.</param>
		public If(Expression expression, IEnumerable<Node> children, Else @else)
			: base(expression, children)
		{
			this.elif = @else;
		}

		public override IEnumerator<RumorYield> Exec(Engine.Rumor rumor)
		{
			var value = Evaluate(rumor);

			if (value.IsBool() && value.AsBool()
				|| value.IsFloat() && value.AsFloat() != 0
				|| value.IsInt() && value.AsInt() != 0
				|| value.IsString() && value.AsString() != "") {

				rumor.EnterBlock(Children);
			}

			yield return null;
		}

		#region Serialization

		public If(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
