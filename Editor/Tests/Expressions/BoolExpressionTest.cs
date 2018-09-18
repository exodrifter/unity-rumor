#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

/// <summary>
/// Tests expressions with boolean and null as the left and right arguments.
/// </summary>
namespace Exodrifter.Rumor.Test.Expressions
{
	public class BoolExpressionTest
	{
		private readonly VariableExpression varA = new VariableExpression("a");
		private readonly VariableExpression varB = new VariableExpression("b");
		private readonly VariableExpression varNull = new VariableExpression("null");
		private readonly Scope scope = new Scope();
		private readonly Bindings bindings = new Bindings();

		[SetUp]
		public void Setup()
		{
			scope.SetVar("a", true);
			scope.SetVar("b", false);
		}

		public Value Eval(Expression exp)
		{
			return exp.Evaluate(scope, bindings);
		}

		#region Bool and Bool

		[Test]
		public void BoolPlusBool()
		{
			var ab = new AddExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new AddExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolMinusBool()
		{
			var ab = new SubtractExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new SubtractExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolTimesBool()
		{
			var ab = new MultiplyExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new MultiplyExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolDividedByBool()
		{
			var ab = new DivideExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new DivideExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolAndBool()
		{
			var ab = new BoolAndExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new BoolAndExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void BoolOrBool()
		{
			var ab = new BoolOrExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new BoolOrExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void BoolXorBool()
		{
			var ab = new BoolXorExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new BoolXorExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void BoolEqualsBool()
		{
			var aa = new EqualsExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new EqualsExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());
		}

		[Test]
		public void BoolNotEqualsBool()
		{
			var aa = new NotEqualsExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new NotEqualsExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());
		}

		[Test]
		public void BoolGreaterThanBool()
		{
			var ab = new GreaterThanExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new GreaterThanExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolGreaterThanOrEqualBool()
		{
			var ab = new GreaterThanOrEqualExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new GreaterThanOrEqualExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolLessThanBool()
		{
			var ab = new LessThanExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new LessThanExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void BoolLessThanOrEqualBool()
		{
			var ab = new LessThanOrEqualExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new LessThanOrEqualExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		#endregion

		#region Bool and Null

		[Test]
		public void BoolPlusNull()
		{
			var an = new AddExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new AddExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolMinusNull()
		{
			var an = new SubtractExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new SubtractExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolTimesNull()
		{
			var an = new MultiplyExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new MultiplyExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolDividedByNull()
		{
			var an = new DivideExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new DivideExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolAndNull()
		{
			var an = new BoolAndExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new BoolAndExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void BoolOrNull()
		{
			var an = new BoolOrExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new BoolOrExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void BoolXorNull()
		{
			var an = new BoolXorExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new BoolXorExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void BoolEqualsNull()
		{
			var an = new EqualsExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new EqualsExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void BoolNotEqualsNull()
		{
			var an = new NotEqualsExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new NotEqualsExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void BoolGreaterThanNull()
		{
			var an = new GreaterThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolGreaterThanOrEqualNull()
		{
			var an = new GreaterThanOrEqualExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanOrEqualExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolLessThanNull()
		{
			var an = new LessThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void BoolLessThanOrEqualNull()
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
