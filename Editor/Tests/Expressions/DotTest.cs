#if UNITY_EDITOR

using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure dot operators work as expected
	/// </summary>
	public class DotTest
	{
		/// <summary>
		/// Check if member access on objects work.
		/// </summary>
		[Test]
		public void Member()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("o", new Test(1, 2));

			var exp = new DotExpression(
				new VariableExpression("o"),
				new VariableExpression("a")
			);
			Assert.AreEqual(1, exp.Evaluate(rumor).AsInt());

			exp = new DotExpression(
				new VariableExpression("o"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);
		}

		/// <summary>
		/// Check if function access on objects work.
		/// </summary>
		[Test]
		public void Function()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("o", new Test(1, 2));

			var exp = new DotExpression(
				new VariableExpression("o"),
				new FunctionExpression("foo")
			);
			Assert.AreEqual(1, exp.Evaluate(rumor).AsInt());

			exp = new DotExpression(
				new VariableExpression("o"),
				new FunctionExpression("bar")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);
		}

		/// <summary>
		/// Make sure dot operators can only be called on objects.
		/// </summary>
		[Test]
		public void Invalid()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("bool", true);
			rumor.Scope.SetVar("int", 1);
			rumor.Scope.SetVar("float", 1f);
			rumor.Scope.SetVar("string", "str");

			// Member access
			var exp = new DotExpression(
				new VariableExpression("bool"),
				new VariableExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			exp = new DotExpression(
				new VariableExpression("int"),
				new VariableExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			exp = new DotExpression(
				new VariableExpression("float"),
				new VariableExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			exp = new DotExpression(
				new VariableExpression("string"),
				new VariableExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			// Function access
			exp = new DotExpression(
				new VariableExpression("bool"),
				new FunctionExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			exp = new DotExpression(
				new VariableExpression("int"),
				new FunctionExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			exp = new DotExpression(
				new VariableExpression("float"),
				new FunctionExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);

			exp = new DotExpression(
				new VariableExpression("string"),
				new FunctionExpression("foo")
			);
			Assert.Throws<InvalidOperationException>(
				() => exp.Evaluate(rumor)
			);
		}
	}

	class Test
	{
		public int a;
		private int b;

		public Test(int a, int b) { this.a = a; this.b = b; }

		public int foo() { return a; }
		private int bar() { return b; }
	}
}

#endif
