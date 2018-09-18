#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

/// <summary>
/// Tests expressions with string and null as the left and right arguments.
/// </summary>
namespace Exodrifter.Rumor.Test.Expressions
{
	public class StringExpressionTest
	{
		private readonly VariableExpression varA = new VariableExpression("a");
		private readonly VariableExpression varB = new VariableExpression("b");
		private readonly VariableExpression varEmpty = new VariableExpression("empty");
		private readonly VariableExpression varNull = new VariableExpression("null");
		private readonly Scope scope = new Scope();
		private readonly Bindings bindings = new Bindings();

		[SetUp]
		public void Setup()
		{
			scope.SetVar("a", "a");
			scope.SetVar("b", "b");
			scope.SetVar("empty", "");
		}

		public Value Eval(Expression exp)
		{
			return exp.Evaluate(scope, bindings);
		}

		#region String and String

		[Test]
		public void StringPlusString()
		{
			var ae = new AddExpression(varA, varEmpty);
			Assert.AreEqual("a", Eval(ae).AsObject());

			var ab = new AddExpression(varA, varB);
			Assert.AreEqual("ab", Eval(ab).AsObject());

			var ba = new AddExpression(varB, varA);
			Assert.AreEqual("ba", Eval(ba).AsObject());
		}

		[Test]
		public void StringMinusString()
		{
			var ab = new SubtractExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new SubtractExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void StringTimesString()
		{
			var ab = new MultiplyExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new MultiplyExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void StringDividedByString()
		{
			var ab = new DivideExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));

			var ba = new DivideExpression(varB, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(ba));
		}

		[Test]
		public void StringAndString()
		{
			var ee = new BoolAndExpression(varEmpty, varEmpty);
			Assert.AreEqual(false, Eval(ee).AsObject());

			var ae = new BoolAndExpression(varA, varEmpty);
			Assert.AreEqual(false, Eval(ae).AsObject());

			var ea = new BoolAndExpression(varEmpty, varA);
			Assert.AreEqual(false, Eval(ea).AsObject());

			var ab = new BoolAndExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());
		}

		[Test]
		public void StringOrString()
		{
			var ee = new BoolOrExpression(varEmpty, varEmpty);
			Assert.AreEqual(false, Eval(ee).AsObject());

			var ae = new BoolOrExpression(varA, varEmpty);
			Assert.AreEqual(true, Eval(ae).AsObject());

			var ea = new BoolOrExpression(varEmpty, varA);
			Assert.AreEqual(true, Eval(ea).AsObject());

			var ab = new BoolOrExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());
		}

		[Test]
		public void StringXorString()
		{
			var ee = new BoolXorExpression(varEmpty, varEmpty);
			Assert.AreEqual(false, Eval(ee).AsObject());

			var ae = new BoolXorExpression(varA, varEmpty);
			Assert.AreEqual(true, Eval(ae).AsObject());

			var ea = new BoolXorExpression(varEmpty, varA);
			Assert.AreEqual(true, Eval(ea).AsObject());

			var ab = new BoolXorExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());
		}

		[Test]
		public void StringEqualsString()
		{
			var ee = new EqualsExpression(varEmpty, varEmpty);
			Assert.AreEqual(true, Eval(ee).AsObject());

			var aa = new EqualsExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ab = new EqualsExpression(varA, varB);
			Assert.AreEqual(false, Eval(ab).AsObject());

			var ba = new EqualsExpression(varB, varA);
			Assert.AreEqual(false, Eval(ba).AsObject());
		}

		[Test]
		public void StringNotEqualsString()
		{
			var ee = new NotEqualsExpression(varEmpty, varEmpty);
			Assert.AreEqual(false, Eval(ee).AsObject());

			var aa = new NotEqualsExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ab = new NotEqualsExpression(varA, varB);
			Assert.AreEqual(true, Eval(ab).AsObject());

			var ba = new NotEqualsExpression(varB, varA);
			Assert.AreEqual(true, Eval(ba).AsObject());
		}

		[Test]
		public void StringGreaterThanString()
		{
			var ee = new GreaterThanExpression(varEmpty, varEmpty);
			Assert.Throws<InvalidOperationException>(() => Eval(ee));

			var aa = new GreaterThanExpression(varA, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(aa));

			var ab = new GreaterThanExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));
		}

		[Test]
		public void StringGreaterThanOrEqualString()
		{
			var ee = new GreaterThanOrEqualExpression(varEmpty, varEmpty);
			Assert.Throws<InvalidOperationException>(() => Eval(ee));

			var aa = new GreaterThanOrEqualExpression(varA, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(aa));

			var ab = new GreaterThanOrEqualExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));
		}

		[Test]
		public void StringLessThanString()
		{
			var ee = new LessThanExpression(varEmpty, varEmpty);
			Assert.Throws<InvalidOperationException>(() => Eval(ee));

			var aa = new LessThanExpression(varA, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(aa));

			var ab = new LessThanExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));
		}

		[Test]
		public void StringLessThanOrEqualString()
		{
			var ee = new LessThanOrEqualExpression(varEmpty, varEmpty);
			Assert.Throws<InvalidOperationException>(() => Eval(ee));

			var aa = new LessThanOrEqualExpression(varA, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(aa));

			var ab = new LessThanOrEqualExpression(varA, varB);
			Assert.Throws<InvalidOperationException>(() => Eval(ab));
		}

		#endregion

		#region String and Null

		[Test]
		public void StringPlusNull()
		{
			var an = new AddExpression(varA, varNull);
			Assert.AreEqual("a", Eval(an).AsObject());

			var na = new AddExpression(varNull, varA);
			Assert.AreEqual("a", Eval(na).AsObject());
		}

		[Test]
		public void StringMinusNull()
		{
			var an = new SubtractExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new SubtractExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void StringTimesNull()
		{
			var an = new MultiplyExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new MultiplyExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void StringDividedByNull()
		{
			var an = new DivideExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new DivideExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void StringAndNull()
		{
			var an = new BoolAndExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new BoolAndExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void StringOrNull()
		{
			var an = new BoolOrExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new BoolOrExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void StringXorNull()
		{
			var an = new BoolXorExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new BoolXorExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void StringEqualsNull()
		{
			var aa = new EqualsExpression(varA, varA);
			Assert.AreEqual(true, Eval(aa).AsObject());

			var ae = new EqualsExpression(varA, varEmpty);
			Assert.AreEqual(false, Eval(ae).AsObject());

			var an = new EqualsExpression(varA, varNull);
			Assert.AreEqual(false, Eval(an).AsObject());

			var na = new EqualsExpression(varNull, varA);
			Assert.AreEqual(false, Eval(na).AsObject());
		}

		[Test]
		public void StringNotEqualsNull()
		{
			var aa = new NotEqualsExpression(varA, varA);
			Assert.AreEqual(false, Eval(aa).AsObject());

			var ae = new NotEqualsExpression(varA, varEmpty);
			Assert.AreEqual(true, Eval(ae).AsObject());

			var an = new NotEqualsExpression(varA, varNull);
			Assert.AreEqual(true, Eval(an).AsObject());

			var na = new NotEqualsExpression(varNull, varA);
			Assert.AreEqual(true, Eval(na).AsObject());
		}

		[Test]
		public void StringGreaterThanNull()
		{
			var an = new GreaterThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void StringGreaterThanOrEqualNull()
		{
			var an = new GreaterThanOrEqualExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new GreaterThanOrEqualExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void StringLessThanNull()
		{
			var an = new LessThanExpression(varA, varNull);
			Assert.Throws<InvalidOperationException>(() => Eval(an));

			var na = new LessThanExpression(varNull, varA);
			Assert.Throws<InvalidOperationException>(() => Eval(na));
		}

		[Test]
		public void StringLessThanOrEqualNull()
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
