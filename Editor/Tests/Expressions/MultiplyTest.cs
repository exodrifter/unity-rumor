#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the multiplication operators work as expected.
	/// </summary>
	public class MultiplyTest
	{
		#region Helpers

		private object Multiply(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new MultiplyExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullMultiply()
		{
			Assert.AreEqual(null, Multiply(null, null));

			Assert.Throws<InvalidOperationException>(() => Multiply(true, null));
			Assert.Throws<InvalidOperationException>(() => Multiply(null, true));

			Assert.AreEqual(0, Multiply(1, null));
			Assert.AreEqual(0, Multiply(null, 1));

			Assert.AreEqual(0, Multiply(1f, null));
			Assert.AreEqual(0, Multiply(null, 1f));

			Assert.Throws<InvalidOperationException>(() => Multiply("a", null));
			Assert.Throws<InvalidOperationException>(() => Multiply(null, "a"));

			Assert.Throws<InvalidOperationException>(() => Multiply(new object(), null));
			Assert.Throws<InvalidOperationException>(() => Multiply(null, new object()));
		}

		[Test]
		public void BoolMultiply()
		{
			Assert.Throws<InvalidOperationException>(() => Multiply(true, true));

			Assert.Throws<InvalidOperationException>(() => Multiply(1, true));
			Assert.Throws<InvalidOperationException>(() => Multiply(true, 1));

			Assert.Throws<InvalidOperationException>(() => Multiply(1f, true));
			Assert.Throws<InvalidOperationException>(() => Multiply(true, 1f));

			Assert.Throws<InvalidOperationException>(() => Multiply("a", true));
			Assert.Throws<InvalidOperationException>(() => Multiply(true, "a"));

			Assert.Throws<InvalidOperationException>(() => Multiply(new object(), true));
			Assert.Throws<InvalidOperationException>(() => Multiply(true, new object()));
		}

		[Test]
		public void IntMultiply()
		{
			Assert.AreEqual(1, Multiply(1, 1));
			Assert.AreEqual(1.GetType(), Multiply(1, 1).GetType());

			Assert.AreEqual(1f, Multiply(1f, 1));
			Assert.AreEqual(1f, Multiply(1, 1f));
			Assert.AreEqual(1f.GetType(), Multiply(1, 1f).GetType());
			Assert.AreEqual(1f.GetType(), Multiply(1f, 1).GetType());

			Assert.Throws<InvalidOperationException>(() => Multiply(1, "a"));
			Assert.Throws<InvalidOperationException>(() => Multiply("a", 1));

			Assert.Throws<InvalidOperationException>(() => Multiply(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => Multiply(1, new object()));
		}

		[Test]
		public void FloatMultiply()
		{
			Assert.AreEqual(1f, Multiply(1f, 1f));
			Assert.AreEqual(1f.GetType(), Multiply(1f, 1f).GetType());

			Assert.Throws<InvalidOperationException>(() => Multiply(1.5f, "a"));
			Assert.Throws<InvalidOperationException>(() => Multiply("a", 1.5f));

			Assert.Throws<InvalidOperationException>(() => Multiply(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => Multiply(1f, new object()));
		}

		[Test]
		public void StringMultiply()
		{
			Assert.Throws<InvalidOperationException>(() => Multiply("a", "a"));

			Assert.Throws<InvalidOperationException>(() => Multiply("a", new object()));
			Assert.Throws<InvalidOperationException>(() => Multiply(new object(), "a"));
		}

		[Test]
		public void ObjectMultiply()
		{
			Assert.Throws<InvalidOperationException>(() => Multiply(new object(), new object()));
		}
	}
}

#endif
