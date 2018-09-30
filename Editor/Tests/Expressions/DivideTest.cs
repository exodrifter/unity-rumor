#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the add operators work as expected.
	/// </summary>
	public class DivideTest
	{
		#region Helpers

		private object Divide(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new DivideExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullDivide()
		{
			Assert.AreEqual(null, Divide(null, null));

			Assert.Throws<InvalidOperationException>(() => Divide(true, null));
			Assert.Throws<InvalidOperationException>(() => Divide(null, true));

			Assert.AreEqual(0, Divide(1, null));
			Assert.AreEqual(0, Divide(null, 1));

			Assert.AreEqual(0f, Divide(1f, null));
			Assert.AreEqual(0f, Divide(null, 1f));

			Assert.Throws<InvalidOperationException>(() => Divide("a", null));
			Assert.Throws<InvalidOperationException>(() => Divide(null, "a"));

			Assert.Throws<InvalidOperationException>(() => Divide(new object(), null));
			Assert.Throws<InvalidOperationException>(() => Divide(null, new object()));
		}

		[Test]
		public void BoolDivide()
		{
			Assert.Throws<InvalidOperationException>(() => Divide(true, true));

			Assert.Throws<InvalidOperationException>(() => Divide(1, true));
			Assert.Throws<InvalidOperationException>(() => Divide(true, 1));

			Assert.Throws<InvalidOperationException>(() => Divide(1f, true));
			Assert.Throws<InvalidOperationException>(() => Divide(true, 1f));

			Assert.Throws<InvalidOperationException>(() => Divide("a", true));
			Assert.Throws<InvalidOperationException>(() => Divide(true, "a"));

			Assert.Throws<InvalidOperationException>(() => Divide(new object(), true));
			Assert.Throws<InvalidOperationException>(() => Divide(true, new object()));
		}

		[Test]
		public void IntDivide()
		{
			Assert.AreEqual(1, Divide(1, 1));
			Assert.AreEqual(1.GetType(), Divide(1, 1).GetType());

			Assert.AreEqual(1f, Divide(1f, 1));
			Assert.AreEqual(1f, Divide(1, 1f));
			Assert.AreEqual(1f.GetType(), Divide(1, 1f).GetType());
			Assert.AreEqual(1f.GetType(), Divide(1f, 1).GetType());

			Assert.Throws<InvalidOperationException>(() => Divide("a", 1));
			Assert.Throws<InvalidOperationException>(() => Divide(1, "a"));

			Assert.Throws<InvalidOperationException>(() => Divide(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => Divide(1, new object()));
		}

		[Test]
		public void FloatDivide()
		{
			Assert.AreEqual(1f, Divide(1f, 1f));
			Assert.AreEqual(1f.GetType(), Divide(1f, 1f).GetType());

			Assert.Throws<InvalidOperationException>(() => Divide("a", 1f));
			Assert.Throws<InvalidOperationException>(() => Divide(1f, "a"));

			Assert.Throws<InvalidOperationException>(() => Divide(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => Divide(1f, new object()));
		}

		[Test]
		public void StringDivide()
		{
			Assert.Throws<InvalidOperationException>(() => Divide("a", "a"));

			Assert.Throws<InvalidOperationException>(() => Divide("a", new object()));
			Assert.Throws<InvalidOperationException>(() => Divide(new object(), "a"));
		}

		[Test]
		public void ObjectDivide()
		{
			Assert.Throws<InvalidOperationException>(() => Divide(new object(), new object()));
		}
	}
}

#endif
