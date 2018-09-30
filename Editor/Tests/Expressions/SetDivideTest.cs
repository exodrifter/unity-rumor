#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the set divide operators work as expected.
	/// </summary>
	public class SetDivideTest
	{
		#region Helpers

		private object SetDivide(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new SetDivideExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);
			exp.Evaluate(scope, new Bindings());

			return new VariableExpression("l")
				.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullSetDivide()
		{
			Assert.AreEqual(null, SetDivide(null, null));

			Assert.Throws<InvalidOperationException>(() => SetDivide(true, null));
			Assert.Throws<InvalidOperationException>(() => SetDivide(null, true));

			Assert.AreEqual(0, SetDivide(1, null));
			Assert.AreEqual(0, SetDivide(null, 1));

			Assert.AreEqual(0f, SetDivide(1f, null));
			Assert.AreEqual(0f, SetDivide(null, 1f));

			Assert.Throws<InvalidOperationException>(() => SetDivide("a", null));
			Assert.Throws<InvalidOperationException>(() => SetDivide(null, "a"));

			Assert.Throws<InvalidOperationException>(() => SetDivide(new object(), null));
			Assert.Throws<InvalidOperationException>(() => SetDivide(null, new object()));
		}

		[Test]
		public void BoolSetDivide()
		{
			Assert.Throws<InvalidOperationException>(() => SetDivide(true, true));

			Assert.Throws<InvalidOperationException>(() => SetDivide(1, true));
			Assert.Throws<InvalidOperationException>(() => SetDivide(true, 1));

			Assert.Throws<InvalidOperationException>(() => SetDivide(1f, true));
			Assert.Throws<InvalidOperationException>(() => SetDivide(true, 1f));

			Assert.Throws<InvalidOperationException>(() => SetDivide("a", true));
			Assert.Throws<InvalidOperationException>(() => SetDivide(true, "a"));

			Assert.Throws<InvalidOperationException>(() => SetDivide(new object(), true));
			Assert.Throws<InvalidOperationException>(() => SetDivide(true, new object()));
		}

		[Test]
		public void IntSetDivide()
		{
			Assert.AreEqual(1, SetDivide(1, 1));
			Assert.AreEqual(1.GetType(), SetDivide(1, 1).GetType());

			Assert.AreEqual(1f, SetDivide(1f, 1));
			Assert.AreEqual(1f, SetDivide(1, 1f));
			Assert.AreEqual(1f.GetType(), SetDivide(1, 1f).GetType());
			Assert.AreEqual(1f.GetType(), SetDivide(1f, 1).GetType());

			Assert.Throws<InvalidOperationException>(() => SetDivide("a", 1));
			Assert.Throws<InvalidOperationException>(() => SetDivide(1, "a"));

			Assert.Throws<InvalidOperationException>(() => SetDivide(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => SetDivide(1, new object()));
		}

		[Test]
		public void FloatSetDivide()
		{
			Assert.AreEqual(1f, SetDivide(1f, 1f));
			Assert.AreEqual(1f.GetType(), SetDivide(1f, 1f).GetType());

			Assert.Throws<InvalidOperationException>(() => SetDivide("a", 1f));
			Assert.Throws<InvalidOperationException>(() => SetDivide(1f, "a"));

			Assert.Throws<InvalidOperationException>(() => SetDivide(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => SetDivide(1f, new object()));
		}

		[Test]
		public void StringSetDivide()
		{
			Assert.Throws<InvalidOperationException>(() => SetDivide("a", "a"));

			Assert.Throws<InvalidOperationException>(() => SetDivide("a", new object()));
			Assert.Throws<InvalidOperationException>(() => SetDivide(new object(), "a"));
		}

		[Test]
		public void ObjectSetDivide()
		{
			Assert.Throws<InvalidOperationException>(() => SetDivide(new object(), new object()));
		}
	}
}

#endif
