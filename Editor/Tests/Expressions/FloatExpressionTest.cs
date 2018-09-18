#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

/// <summary>
/// Tests expressions with float and null as the left and right arguments.
/// </summary>
namespace Exodrifter.Rumor.Test.Expressions
{
	public class FloatExpressionTest
	{
		private readonly VariableExpression varA = new VariableExpression("a");
		private readonly VariableExpression varB = new VariableExpression("b");
		private readonly VariableExpression varNull = new VariableExpression("null");
		private readonly Scope scope = new Scope();
		private readonly Bindings bindings = new Bindings();

		[SetUp]
		public void Setup()
		{
			scope.SetVar("a", 1f);
			scope.SetVar("b", 0f);
		}

		public Value Eval(Expression exp)
		{
			return exp.Evaluate(scope, bindings);
		}

		#region Float and Float

		[Test]
		public void FloatPlusFloat()
		{
			var aa = new AddExpression(varA, varA);
			Assert.AreEqual(2f, Eval(aa).AsObject());

			var ab = new AddExpression(varA, varB);
			Assert.AreEqual(1f, Eval(ab).AsObject());

			var ba = new AddExpression(varB, varA);
			Assert.AreEqual(1f, Eval(ba).AsObject());
		}

		[Test]
		public void FloatMinusFloat()
		{
			var aa = new AddExpression(varA, varA);
			Assert.AreEqual(0f, Eval(aa).AsObject());

			var ab = new SubtractExpression(varA, varB);
			Assert.AreEqual(1f, Eval(ab).AsObject());

			var ba = new SubtractExpression(varB, varA);
			Assert.AreEqual(-1f, Eval(ba).AsObject());
		}

		[Test]
		public void FloatTimesFloat()
		{
			var aa = new MultiplyExpression(varA, varA);
			Assert.AreEqual(1f, Eval(aa).AsObject());

			var ab = new MultiplyExpression(varA, varB);
			Assert.AreEqual(0f, Eval(ab).AsObject());

			var ba = new MultiplyExpression(varB, varA);
			Assert.AreEqual(0f, Eval(ba).AsObject());
		}

		[Test]
		public void FloatDividedByFloat()
		{
			var aa = new DivideExpression(varA, varA);
			Assert.AreEqual(1f, Eval(aa).AsObject());

			var ab = new DivideExpression(varA, varB);
			Assert.Throws<DivideByZeroException>(() => Eval(ab).AsObject());

			var ba = new DivideExpression(varB, varA);
			Assert.AreEqual(0f, Eval(ba).AsObject());
		}

		[Test]
		public void FloatAndFloat()
		{
			var aa = new BoolAndExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new BoolAndExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new BoolAndExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void FloatOrFloat()
		{
			var aa = new BoolOrExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new BoolOrExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new BoolOrExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void FloatXorFloat()
		{
			var aa = new BoolXorExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new BoolXorExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new BoolXorExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void FloatEqualsFloat()
		{
			var aa = new EqualsExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new EqualsExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new EqualsExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void FloatNotEqualsFloat()
		{
			var aa = new NotEqualsExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new NotEqualsExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new NotEqualsExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void FloatGreaterThanFloat()
		{
			var aa = new GreaterThanExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new GreaterThanExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new GreaterThanExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void FloatGreaterThanOrEqualFloat()
		{
			var aa = new GreaterThanOrEqualExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new GreaterThanOrEqualExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new GreaterThanOrEqualExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void FloatLessThanFloat()
		{
			var aa = new LessThanExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new LessThanExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new LessThanExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void FloatLessThanOrEqualFloat()
		{
			var aa = new LessThanOrEqualExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new LessThanOrEqualExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new LessThanOrEqualExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		#endregion

		#region Float and Null

		[Test]
		public void FloatPlusNull()
		{
			var an = new AddExpression(varA, varNull);
			Assert.AreEqual(1f, Eval(an).AsObject());

			var na = new AddExpression(varNull, varA);
			Assert.AreEqual(1f, Eval(na).AsObject());
		}

		[Test]
		public void FloatMinusNull()
		{
			var an = new SubtractExpression(varA, varNull);
			Assert.AreEqual(1f, Eval(an).AsObject());

			var na = new SubtractExpression(varNull, varA);
			Assert.AreEqual(-1f, Eval(na).AsObject());
		}

		[Test]
		public void FloatTimesNull()
		{
			var an = new MultiplyExpression(varA, varNull);
			Assert.AreEqual(0f, Eval(an).AsObject());

			var na = new MultiplyExpression(varNull, varA);
			Assert.AreEqual(0f, Eval(na).AsObject());
		}

		[Test]
		public void FloatDividedByNull()
		{
			var an = new DivideExpression(varA, varNull);
			Assert.Throws<DivideByZeroException>(() => Eval(an).AsObject());

			var na = new DivideExpression(varNull, varA);
			Assert.AreEqual(0f, Eval(na).AsObject());
		}

		[Test]
		public void FloatAndNull()
		{
			var an = new BoolAndExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsBool());

			var na = new BoolAndExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsBool());
		}

		[Test]
		public void FloatOrNull()
		{
			var an = new BoolOrExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsBool());

			var na = new BoolOrExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsBool());
		}

		[Test]
		public void FloatXorNull()
		{
			var an = new BoolXorExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsBool());

			var na = new BoolXorExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsBool());
		}

		[Test]
		public void FloatEqualsNull()
		{
			var an = new EqualsExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsBool());

			var na = new EqualsExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsBool());
		}

		[Test]
		public void FloatNotEqualsNull()
		{
			var an = new NotEqualsExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsBool());

			var na = new NotEqualsExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsBool());
		}

		[Test]
		public void FloatGreaterThanNull()
		{
			var an = new GreaterThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void FloatGreaterThanOrEqualNull()
		{
			var an = new GreaterThanOrEqualExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanOrEqualExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void FloatLessThanNull()
		{
			var an = new LessThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void FloatLessThanOrEqualNull()
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
