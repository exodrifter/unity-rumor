using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Adds a choose to the rumor state.
	/// </summary>
	[Serializable]
	public class Choose : Node
	{
		/// <summary>
		/// The number of items to choose.
		/// </summary>
		private readonly Expression number;

		/// <summary>
		/// The amount of time in seconds to make each choice. If the amount of
		/// time is less than or equal to zero, then the user may take as long
		/// as they wish.
		/// </summary>
		private readonly Expression seconds;

		/// <summary>
		/// The index of the choice to pick when time runs out.
		/// </summary>
		private readonly Expression @default;

		/// <summary>
		/// Creates a new choose.
		/// </summary>
		/// <param name="number">
		/// The number of choices to pick.
		/// </param>
		/// <param name="children">
		/// The children for this choose.
		/// </param>
		public Choose(IEnumerable<Node> children)
			: base(children)
		{
			this.number = new LiteralExpression(1);
			this.seconds = new LiteralExpression(0);
			this.@default = new LiteralExpression(0);
		}

		/// <summary>
		/// Creates a new choose.
		/// </summary>
		/// <param name="number">
		/// The number of choices to pick.
		/// </param>
		/// <param name="children">
		/// The children for this choose.
		/// </param>
		public Choose(int number, IEnumerable<Node> children)
			: base(children)
		{
			this.number = new LiteralExpression(number);
			this.seconds = new LiteralExpression(0);
			this.@default = new LiteralExpression(0);
		}

		/// <summary>
		/// Creates a new choose.
		/// </summary>
		/// <param name="number">
		/// The number of choices to pick.
		/// </param>
		/// <param name="seconds">
		/// The amount of time in seconds to make each choice.
		/// </param>
		/// <param name="children">
		/// The children for this choose.
		/// </param>
		public Choose(int number, float seconds, IEnumerable<Node> children)
			: base(children)
		{
			this.number = new LiteralExpression(number);
			this.seconds = new LiteralExpression(seconds);
			this.@default = new LiteralExpression(0);
		}

		/// <summary>
		/// Creates a new choose.
		/// </summary>
		/// <param name="number">
		/// The number of choices to pick.
		/// </param>
		/// <param name="seconds">
		/// The amount of time in seconds to make each choice.
		/// </param>
		/// <param name="@default">
		/// The index of the choice to pick when time runs out.
		/// </param>
		/// <param name="children">
		/// The children for this choose.
		/// </param>
		public Choose(int number, float seconds, int @default, IEnumerable<Node> children)
			: base(children)
		{
			this.number = new LiteralExpression(number);
			this.seconds = new LiteralExpression(seconds);
			this.@default = new LiteralExpression(@default);
		}

		/// <summary>
		/// Creates a new choose.
		/// </summary>
		/// <param name="number">
		/// The number of choices to pick.
		/// </param>
		/// <param name="number">
		/// The amount of time in seconds to make each choice.
		/// </param>
		/// <param name="@default">
		/// The index of the choice to pick when time runs out.
		/// </param>
		/// <param name="children">
		/// The children for this choose.
		/// </param>
		public Choose(Expression number, Expression seconds, Expression @default, IEnumerable<Node> children)
			: base(children)
		{
			this.number = number;
			this.seconds = seconds;
			this.@default = @default;
		}

		public int EvaluateNumber(Engine.Rumor rumor)
		{
			return number.Evaluate(rumor).AsInt(1);
		}

		public float EvaluateTime(Engine.Rumor rumor)
		{
			return seconds.Evaluate(rumor).AsFloat(0);
		}

		public int EvaluateDefault(Engine.Rumor rumor)
		{
			return @default.Evaluate(rumor).AsInt(0);
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			int number = EvaluateNumber(rumor);
			float time = EvaluateTime(rumor);
			int @default = EvaluateDefault(rumor);

			yield return new ForChoice(number, time, @default);
		}

		#region Serialization

		public Choose(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			number = info.GetValue<Expression>("number");
			seconds = info.GetValue<Expression>("time");
			@default = info.GetValue<Expression>("default");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<Expression>("number", number);
			info.AddValue<Expression>("time", seconds);
			info.AddValue<Expression>("default", @default);
		}

		#endregion
	}
}
