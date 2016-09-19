using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// An elif conditional statement.
	/// </summary>
	[Serializable]
	public sealed class Elif : If
	{
		/// <summary>
		/// Creates a new elif block.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		public Elif(Expression expression, IEnumerable<Node> children)
			: base(expression, children)
		{
		}

		/// <summary>
		/// Creates a new elif block with a chained elif statement.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		/// <param name="elif">The chained if statement.</param>
		public Elif(Expression expression, IEnumerable<Node> children, Elif elif)
			: base(expression, children, elif)
		{
		}

		/// <summary>
		/// Creates a new elif block with a chained else statement.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		/// <param name="else">The chained else statement.</param>
		public Elif(Expression expression, IEnumerable<Node> children, Else @else)
			: base(expression, children, @else)
		{
		}

		#region Serialization

		public Elif(SerializationInfo info, StreamingContext context)
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
