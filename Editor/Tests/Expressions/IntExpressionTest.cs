#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

/// <summary>
/// Tests expressions with int and null as the left and right arguments.
/// </summary>
namespace Exodrifter.Rumor.Test.Expressions
{
	public class IntExpressionTest
	{
		private readonly VariableExpression varA = new VariableExpression("a");
		private readonly VariableExpression varB = new VariableExpression("b");
		private readonly VariableExpression varNull = new VariableExpression("null");
		private readonly Scope scope = new Scope();
		private readonly Bindings bindings = new Bindings();

		[SetUp]
		public void Setup()
		{
			scope.SetVar("a", 1);
			scope.SetVar("b", 0);
		}

		public Value Eval(Expression exp)
		{
			return exp.Evaluate(scope, bindings);
		}

		#region Int and Int

		[Test]
		public void IntPlusInt()
		{
			var aa = new AddExpression(varA, varA);
			Assert.AreEqual(2, Eval(aa).AsObject());

			var ab = new AddExpression(varA, varB);
			Assert.AreEqual(1, Eval(ab).AsObject());

			var ba = new AddExpression(varB, varA);
			Assert.AreEqual(1, Eval(ba).AsObject());
		}

		[Test]
		public void IntMinusInt()
		{
			var aa = new SubtractExpression(varA, varA);
			Assert.AreEqual(0, Eval(aa).AsObject());

			var ab = new SubtractExpression(varA, varB);
			Assert.AreEqual(1, Eval(ab).AsObject());

			var ba = new SubtractExpression(varB, varA);
			Assert.AreEqual(-1, Eval(ba).AsObject());
		}

		[Test]
		public void IntTimesInt()
		{
			var aa = new MultiplyExpression(varA, varA);
			Assert.AreEqual(1, Eval(aa).AsObject());

			var ab = new MultiplyExpression(varA, varB);
			Assert.AreEqual(0, Eval(ab).AsObject());

			var ba = new MultiplyExpression(varB, varA);
			Assert.AreEqual(0, Eval(ba).AsObject());
		}

		[Test]
		public void IntDividedByInt()
		{
			var aa = new DivideExpression(varA, varA);
			Assert.AreEqual(1, Eval(aa).AsObject());

			var ab = new DivideExpression(varA, varB);
			Assert.Throws<DivideByZeroException>(() => Eval(ab).AsObject());

			var ba = new DivideExpression(varB, varA);
			Assert.AreEqual(0, Eval(ba).AsObject());
		}

		[Test]
		public void IntAndInt()
		{
			var aa = new BoolAndExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new BoolAndExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new BoolAndExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void IntOrInt()
		{
			var aa = new BoolOrExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new BoolOrExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new BoolOrExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void IntXorInt()
		{
			var aa = new BoolXorExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new BoolXorExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new BoolXorExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void IntEqualsInt()
		{
			var aa = new EqualsExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new EqualsExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new EqualsExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void IntNotEqualsInt()
		{
			var aa = new NotEqualsExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new NotEqualsExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new NotEqualsExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void IntGreaterThanInt()
		{
			var aa = new GreaterThanExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new GreaterThanExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new GreaterThanExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void IntGreaterThanOrEqualInt()
		{
			var aa = new GreaterThanOrEqualExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new GreaterThanOrEqualExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new GreaterThanOrEqualExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void IntLessThanInt()
		{
			var aa = new LessThanExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new LessThanExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new LessThanExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void IntLessThanOrEqualInt()
		{
			var aa = new LessThanOrEqualExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new LessThanOrEqualExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new LessThanOrEqualExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		#endregion

		#region Int and Null

		[Test]
		public void IntPlusNull()
		{
			var an = new AddExpression(varA, varNull);
			Assert.AreEqual(1, Eval(an).AsObject());

			var na = new AddExpression(varNull, varA);
			Assert.AreEqual(1, Eval(na).AsObject());
		}

		[Test]
		public void IntMinusNull()
		{
			var an = new SubtractExpression(varA, varNull);
			Assert.AreEqual(1, Eval(an).AsObject());

			var na = new SubtractExpression(varNull, varA);
			Assert.AreEqual(-1, Eval(na).AsObject());
		}

		[Test]
		public void IntTimesNull()
		{
			var an = new MultiplyExpression(varA, varNull);
			Assert.AreEqual(0, Eval(an).AsObject());

			var na = new MultiplyExpression(varNull, varA);
			Assert.AreEqual(0, Eval(na).AsObject());
		}

		[Test]
		public void IntDividedByNull()
		{
			var an = new DivideExpression(varA, varNull);
			Assert.Throws<DivideByZeroException>(() => Eval(an).AsObject());

			var na = new DivideExpression(varNull, varA);
			Assert.AreEqual(0, Eval(na).AsObject());
		}

		[Test]
		public void IntAndNull()
		{
			var an = new BoolAndExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new BoolAndExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void IntOrNull()
		{
			var an = new BoolOrExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new BoolOrExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void IntXorNull()
		{
			var an = new BoolXorExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new BoolXorExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void IntEqualsNull()
		{
			var an = new EqualsExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new EqualsExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void IntNotEqualsNull()
		{
			var an = new NotEqualsExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new NotEqualsExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void IntGreaterThanNull()
		{
			var an = new GreaterThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void IntGreaterThanOrEqualNull()
		{
			var an = new GreaterThanOrEqualExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanOrEqualExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void IntLessThanNull()
		{
			var an = new LessThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void IntLessThanOrEqualNull()
		{
			var an = new LessThanOrEqualExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanOrEqualExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		#endregion
	}
}

#endif
