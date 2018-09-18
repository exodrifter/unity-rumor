#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

/// <summary>
/// Tests expressions with null as the left and right arguments.
/// </summary>
namespace Exodrifter.Rumor.Test.Expressions
{
	public class NullExpressionTest
	{
		private readonly VariableExpression varA = new VariableExpression("a");
		private readonly VariableExpression varB = new VariableExpression("b");
		private readonly Scope scope = new Scope();
		private readonly Bindings bindings = new Bindings();

		public Value Eval(Expression exp)
		{
			return exp.Evaluate(scope, bindings);
		}

		#region Null and Null

		[Test]
		public void NullPlusNull()
		{
			var an = new AddExpression(varA, varB);
			Assert.AreEqual(null, Eval(an).AsObject());

			var na = new AddExpression(varB, varA);
			Assert.AreEqual(null, Eval(na).AsObject());
		}

		[Test]
		public void NullMinusNull()
		{
			var an = new SubtractExpression(varA, varB);
			Assert.AreEqual(null, Eval(an).AsObject());

			var na = new SubtractExpression(varB, varA);
			Assert.AreEqual(null, Eval(na).AsObject());
		}

		[Test]
		public void NullTimesNull()
		{
			var an = new MultiplyExpression(varA, varB);
			Assert.AreEqual(null, Eval(an).AsObject());

			var na = new MultiplyExpression(varB, varA);
			Assert.AreEqual(null, Eval(na).AsObject());
		}

		[Test]
		public void NullDividedByNull()
		{
			var an = new DivideExpression(varA, varB);
			Assert.AreEqual(null, Eval(an).AsObject());

			var na = new DivideExpression(varB, varA);
			Assert.AreEqual(null, Eval(na).AsObject());
		}

		[Test]
		public void NullAndNull()
		{
			var an = new BoolAndExpression(varA, varB);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new BoolAndExpression(varB, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void NullOrNull()
		{
			var an = new BoolOrExpression(varA, varB);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new BoolOrExpression(varB, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void NullXorNull()
		{
			var an = new BoolXorExpression(varA, varB);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new BoolXorExpression(varB, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void NullEqualsNull()
		{
			var an = new EqualsExpression(varA, varB);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new EqualsExpression(varB, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void NullNotEqualsNull()
		{
			var an = new NotEqualsExpression(varA, varB);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new NotEqualsExpression(varB, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void NullGreaterThanNull()
		{
			var an = new GreaterThanExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void NullGreaterThanOrEqualNull()
		{
			var an = new GreaterThanOrEqualExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanOrEqualExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void NullLessThanNull()
		{
			var an = new LessThanExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void NullLessThanOrEqualNull()
		{
			var an = new LessThanOrEqualExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanOrEqualExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		#endregion
	}
}

#endif
