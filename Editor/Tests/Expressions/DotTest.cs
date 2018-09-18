#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure dot operators work as expected.
	/// </summary>
	public class DotTest
	{
		/// <summary>
		/// Check if member access on objects work.
		/// </summary>
		[Test]
		public void ObjectDotMember()
		{
			var scope = new Scope();
			var bindings = new Bindings();
			scope.SetVar("o", new Test(1, 2));

			var exp = new DotExpression(
				new VariableExpression("o"),
				new VariableExpression("publicInt")
			);
			Assert.AreEqual(1, exp.Evaluate(scope, bindings).AsObject());

			exp = new DotExpression(
				new VariableExpression("o"),
				new VariableExpression("privateInt")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(scope, bindings)
			);

			exp = new DotExpression(
				new VariableExpression("o"),
				new VariableExpression("imaginaryInt")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(scope, bindings)
			);
		}

		/// <summary>
		/// Check if function access on objects work.
		/// </summary>
		[Test]
		public void ObjectDotFunction()
		{
			var scope = new Scope();
			var bindings = new Bindings();
			scope.SetVar("o", new Test(1, 2));

			var exp = new DotExpression(
				new VariableExpression("o"),
				new FunctionExpression("publicMethod")
			);
			Assert.AreEqual(1, exp.Evaluate(scope, bindings).AsObject());

			exp = new DotExpression(
				new VariableExpression("o"),
				new FunctionExpression("privateMethod")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(scope, bindings)
			);

			exp = new DotExpression(
				new VariableExpression("o"),
				new FunctionExpression("imaginaryMethod")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(scope, bindings)
			);
		}

		/// <summary>
		/// Check that member access on primitives works.
		/// </summary>
		[Test]
		public void PrimitiveDotMember()
		{
			var scope = new Scope();
			var bindings = new Bindings();
			scope.SetVar("string", "str");

			var exp = new DotExpression(
				new VariableExpression("string"),
				new VariableExpression("Length")
			);
			Assert.AreEqual(3, exp.Evaluate(scope, bindings).AsObject());
		}

		[Test]
		public void PrimitiveDotFunction()
		{
			var scope = new Scope();
			var bindings = new Bindings();
			scope.SetVar("bool", true);
			scope.SetVar("int", 1);
			scope.SetVar("float", 1f);
			scope.SetVar("string", "str");

			var exp = new DotExpression(
				new VariableExpression("bool"),
				new FunctionExpression("GetType")
			);
			Assert.AreEqual(typeof(bool), exp.Evaluate(scope, bindings).AsObject());

			exp = new DotExpression(
				new VariableExpression("int"),
				new FunctionExpression("GetType")
			);
			Assert.AreEqual(typeof(int), exp.Evaluate(scope, bindings).AsObject());

			exp = new DotExpression(
				new VariableExpression("float"),
				new FunctionExpression("GetType")
			);
			Assert.AreEqual(typeof(float), exp.Evaluate(scope, bindings).AsObject());

			exp = new DotExpression(
				new VariableExpression("string"),
				new FunctionExpression("GetType")
			);
			Assert.AreEqual(typeof(string), exp.Evaluate(scope, bindings).AsObject());
		}

		/// <summary>
		/// Check that member access on null always returns null.
		/// </summary>
		[Test]
		public void NullDot()
		{
			var scope = new Scope();
			var bindings = new Bindings();

			var exp = new DotExpression(
				new VariableExpression("null"),
				new VariableExpression("foo")
			);
			Assert.AreEqual(null, exp.Evaluate(scope, bindings).AsObject());

			exp = new DotExpression(
				new VariableExpression("null"),
				new FunctionExpression("foo")
			);
			Assert.AreEqual(null, exp.Evaluate(scope, bindings).AsObject());
		}
	}

	class Test
	{
		public int publicInt;
		private int privateInt;

		public Test(int a, int b) { publicInt = a; privateInt = b; }

		public int publicMethod() { return publicInt; }
		private int privateMethod() { return privateInt; }
	}
}

#endif
