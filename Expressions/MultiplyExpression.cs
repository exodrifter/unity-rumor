﻿using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an add operator that is used to add two arguments.
	/// </summary>
	[Serializable]
	public class MultiplyExpression : OpExpression
	{
		public MultiplyExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope)
		{
			var l = left.Evaluate(scope);
			var r = right.Evaluate(scope);
			return l.Multiply(r);
		}

		public override string ToString()
		{
			return left + "*" + right;
		}

		#region Serialization

		public MultiplyExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}