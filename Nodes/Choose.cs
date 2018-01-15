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
		public Choose()
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
		public Choose(int number)
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
		public Choose(int number, float seconds)
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
		public Choose(int number, float seconds, int @default)
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
		public Choose(Expression number)
		{
			this.number = number;
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
		public Choose(Expression number, Expression seconds)
		{
			this.number = number;
			this.seconds = seconds;
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
		public Choose(Expression number, Expression seconds, Expression @default)
		{
			this.number = number;
			this.seconds = seconds;
			this.@default = @default;
		}

		public int EvaluateNumber(Engine.Rumor rumor)
		{
			return (number.Evaluate(rumor) ?? new IntValue(1)).AsInt(1);
		}

		public float EvaluateTime(Engine.Rumor rumor)
		{
			return (seconds.Evaluate(rumor) ?? new FloatValue(0)).AsFloat(0);
		}

		public int EvaluateDefault(Engine.Rumor rumor)
		{
			return (@default.Evaluate(rumor) ?? new IntValue(0)).AsInt(0);
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			int number = EvaluateNumber(rumor);
			float time = EvaluateTime(rumor);
			int @default = EvaluateDefault(rumor);

			yield return new ForChoice(number, time, @default);

			rumor.State.ClearChoices();
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
