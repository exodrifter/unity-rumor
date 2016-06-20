using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Conditional statement for entering a block.
	/// </summary>
	[Serializable]
	public sealed class If : Node, ISerializable
	{
		/// <summary>
		/// The text to append to the dialog.
		/// </summary>
		public readonly Expression expression;

		/// <summary>
		/// Creates a new if node.
		/// </summary>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="children">The children of the if.</param>
		public If(Expression expression, IEnumerable<Node> children)
			: base(children)
		{
			this.expression = expression;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			var value = expression.Evaluate(rumor.Scope);

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
			: base((List<Node>)info.GetValue("children", typeof(List<Node>)))
		{
			expression = (Expression)info.GetValue("expression", typeof(Expression));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("children", Children, typeof(List<Node>));
			info.AddValue("expression", expression, typeof(Expression));
		}

		#endregion
	}
}
