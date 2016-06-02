using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Sets the dialog in the rumor state.
	/// </summary>
	[Serializable]
	public sealed class Say : Node, ISerializable
	{
		/// <summary>
		/// The text to replace the dialog with.
		/// </summary>
		private readonly Expression text;

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="text">
		/// The text to replace the dialog with.
		/// </param>
		public Say(string text)
		{
			this.text = new LiteralExpression(text);
		}

		/// <summary>
		/// Creates a new Say node.
		/// </summary>
		/// <param name="text">
		/// The expression to replace the dialog with.
		/// </param>
		public Say(Expression text)
		{
			this.text = text;
		}

		/// <summary>
		/// Evaluates the text of this node in the specified Rumor.
		/// </summary>
		/// <param name="rumor">The Rumor to evaluate against.</param>
		/// <returns>The text.</returns>
		public string EvaluateText(Engine.Rumor rumor)
		{
			return text.Evaluate(rumor.Scope).AsString();
		}

		/// <summary>
		/// Evaluates the text of this node in the specified Scope.
		/// </summary>
		/// <param name="rumor">The Scope to evaluate against.</param>
		/// <returns>The text.</returns>
		public string EvaluateText(Scope scope)
		{
			return text.Evaluate(scope).AsString();
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			rumor.State.SetDialog(EvaluateText(rumor.Scope));
			yield return new ForAdvance();
		}

		#region Serialization

		public Say(SerializationInfo info, StreamingContext context)
		{
			text = (Expression)info.GetValue("text", typeof(Expression));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("text", text, typeof(Expression));
		}

		#endregion
	}
}
