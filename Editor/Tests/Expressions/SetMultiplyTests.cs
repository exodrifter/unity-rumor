#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the set multiplication operators work as expected.
	/// </summary>
	public class SetMultiplyTest
	{
		#region Helpers

		private object SetMultiply(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new SetMultiplyExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);
			exp.Evaluate(scope, new Bindings());

			return new VariableExpression("l")
				.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullSetMultiply()
		{
			Assert.AreEqual(null, SetMultiply(null, null));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(true, null));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(null, true));

			Assert.AreEqual(0, SetMultiply(1, null));
			Assert.AreEqual(0, SetMultiply(null, 1));

			Assert.AreEqual(0, SetMultiply(1f, null));
			Assert.AreEqual(0, SetMultiply(null, 1f));

			Assert.Throws<InvalidOperationException>(() => SetMultiply("a", null));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(null, "a"));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(new object(), null));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(null, new object()));
		}

		[Test]
		public void BoolSetMultiply()
		{
			Assert.Throws<InvalidOperationException>(() => SetMultiply(true, true));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(1, true));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(true, 1));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(1f, true));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(true, 1f));

			Assert.Throws<InvalidOperationException>(() => SetMultiply("a", true));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(true, "a"));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(new object(), true));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(true, new object()));
		}

		[Test]
		public void IntSetMultiply()
		{
			Assert.AreEqual(1, SetMultiply(1, 1));
			Assert.AreEqual(1.GetType(), SetMultiply(1, 1).GetType());

			Assert.AreEqual(1f, SetMultiply(1f, 1));
			Assert.AreEqual(1f, SetMultiply(1, 1f));
			Assert.AreEqual(1f.GetType(), SetMultiply(1, 1f).GetType());
			Assert.AreEqual(1f.GetType(), SetMultiply(1f, 1).GetType());

			Assert.Throws<InvalidOperationException>(() => SetMultiply(1, "a"));
			Assert.Throws<InvalidOperationException>(() => SetMultiply("a", 1));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(1, new object()));
		}

		[Test]
		public void FloatSetMultiply()
		{
			Assert.AreEqual(1f, SetMultiply(1f, 1f));
			Assert.AreEqual(1f.GetType(), SetMultiply(1f, 1f).GetType());

			Assert.Throws<InvalidOperationException>(() => SetMultiply(1.5f, "a"));
			Assert.Throws<InvalidOperationException>(() => SetMultiply("a", 1.5f));

			Assert.Throws<InvalidOperationException>(() => SetMultiply(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(1f, new object()));
		}

		[Test]
		public void StringSetMultiply()
		{
			Assert.Throws<InvalidOperationException>(() => SetMultiply("a", "a"));

			Assert.Throws<InvalidOperationException>(() => SetMultiply("a", new object()));
			Assert.Throws<InvalidOperationException>(() => SetMultiply(new object(), "a"));
		}

		[Test]
		public void ObjectSetMultiply()
		{
			Assert.Throws<InvalidOperationException>(() => SetMultiply(new object(), new object()));
		}
	}
}

#endif
