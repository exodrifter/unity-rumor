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
		#region Helpers

		/// <summary>
		/// Creates a scope with one variable in it.
		/// </summary>
		/// <param name="name">The name of the variable to assign.</param>
		/// <param name="value">The value of the variable to assign.</param>
		/// <returns>A scope with a single variable assigned.</returns>
		public Scope NewScope(string name, object value)
		{
			var scope = new Scope();
			scope.SetVar(name, value);
			return scope;
		}

		private Value Eval(Expression exp, Scope scope)
		{
			scope = scope ?? new Scope();
			return exp.Evaluate(scope, new Bindings());
		}

		private DotExpression DotMember(string l, string r)
		{
			return new DotExpression(
				new VariableExpression(l),
				new VariableExpression(r)
			);
		}

		private DotExpression DotFunction(string l, string r)
		{
			return new DotExpression(
				new VariableExpression(l),
				new FunctionExpression(r)
			);
		}

		#endregion

		#region Null Dot

		[Test]
		public void NullDotMember()
		{
			var exp = DotMember("null", "foobar");
			Assert.Throws<NullReferenceException>(() => Eval(exp, null));
		}

		[Test]
		public void NullDotFunction()
		{
			var exp = DotFunction("null", "foobar");
			Assert.Throws<NullReferenceException>(() => Eval(exp, null));
		}

		#endregion

		#region Bool Dot

		[Test]
		public void BoolDotMember()
		{
			var scope = NewScope("bool", true);

			// `bool` doesn't have any accessible variable members
			var exp = DotMember("bool", "foobar");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));
		}

		[Test]
		public void BoolDotFunction()
		{
			var scope = NewScope("bool", true);

			var exp = DotFunction("bool", "GetHashCode");
			Assert.AreEqual(true.GetHashCode(), Eval(exp, scope).AsObject());
		}

		#endregion

		#region Int Dot

		[Test]
		public void IntDotMember()
		{
			var scope = NewScope("int", 1);

			// `int` doesn't have any accessible variable members
			var exp = DotMember("int", "foobar");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));
		}

		[Test]
		public void IntDotFunction()
		{
			var scope = NewScope("int", 1);

			var exp = DotFunction("int", "GetHashCode");
			Assert.AreEqual(1.GetHashCode(), Eval(exp, scope).AsObject());
		}

		#endregion

		#region Float Dot

		[Test]
		public void FloatDotMember()
		{
			var scope = NewScope("float", 1f);

			// `float` doesn't have any accessible variable members
			var exp = DotMember("float", "foobar");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));
		}

		[Test]
		public void FloatDotFunction()
		{
			var scope = NewScope("float", 1f);

			var exp = DotFunction("float", "GetHashCode");
			Assert.AreEqual(1f.GetHashCode(), Eval(exp, scope).AsObject());
		}

		#endregion

		#region String Dot

		[Test]
		public void StringDotMember()
		{
			var scope = NewScope("string", "abc");

			var exp = DotMember("string", "Length");
			Assert.AreEqual("abc".Length, Eval(exp, scope).AsObject());
		}

		[Test]
		public void StringDotFunction()
		{
			var scope = NewScope("string", "abc");

			var exp = DotFunction("string", "GetHashCode");
			Assert.AreEqual("abc".GetHashCode(), Eval(exp, scope).AsObject());
		}

		#endregion

		#region Object Dot

		class Test
		{
			public int publicInt;
			private int privateInt;

			public Test(int a, int b) { publicInt = a; privateInt = b; }

			public int publicMethod() { return publicInt; }
			private int privateMethod() { return privateInt; }
		}

		[Test]
		public void ObjectDotMember()
		{
			var scope = NewScope("object", new Test(1, 2));

			var exp = DotMember("object", "publicInt");
			Assert.AreEqual(1, Eval(exp, scope).AsObject());

			exp = DotMember("object", "privateInt");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));

			exp = DotMember("object", "imaginaryInt");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));
		}

		[Test]
		public void ObjectDotFunction()
		{
			var scope = NewScope("object", new Test(1, 2));

			var exp = DotFunction("object", "publicMethod");
			Assert.AreEqual(1, Eval(exp, scope).AsObject());

			exp = DotFunction("object", "privateMethod");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));

			exp = DotFunction("object", "imaginaryMethod");
			Assert.Throws<InvalidOperationException>(() => Eval(exp, scope));
		}

		#endregion
	}
}

#endif
