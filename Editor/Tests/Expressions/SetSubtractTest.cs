#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the set subtract operators work as expected.
	/// </summary>
	public class SetSubtractTest
	{
		#region Helpers

		private object SetSubtract(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new SetSubtractExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);
			exp.Evaluate(scope, new Bindings());

			return new VariableExpression("l")
				.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullSetSubtract()
		{
			Assert.AreEqual(null, SetSubtract(null, null));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(true, null));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(null, true));

			Assert.AreEqual(1, SetSubtract(1, null));
			Assert.AreEqual(-1, SetSubtract(null, 1));

			Assert.AreEqual(1f, SetSubtract(1f, null));
			Assert.AreEqual(-1f, SetSubtract(null, 1f));

			Assert.Throws<InvalidOperationException>(() => SetSubtract("a", null));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(null, "a"));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(new object(), null));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(null, new object()));
		}

		[Test]
		public void BoolSetSubtract()
		{
			Assert.Throws<InvalidOperationException>(() => SetSubtract(true, true));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(1, true));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(true, 1));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(1f, true));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(true, 1f));

			Assert.Throws<InvalidOperationException>(() => SetSubtract("a", true));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(true, "a"));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(new object(), true));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(true, new object()));
		}

		[Test]
		public void IntSetSubtract()
		{
			Assert.AreEqual(0, SetSubtract(1, 1));
			Assert.AreEqual(0.GetType(), SetSubtract(1, 1).GetType());

			Assert.AreEqual(0f, SetSubtract(1f, 1));
			Assert.AreEqual(0f, SetSubtract(1, 1f));
			Assert.AreEqual(0f.GetType(), SetSubtract(1, 1f).GetType());
			Assert.AreEqual(0f.GetType(), SetSubtract(1f, 1).GetType());

			Assert.Throws<InvalidOperationException>(() => SetSubtract(1, "a"));
			Assert.Throws<InvalidOperationException>(() => SetSubtract("a", 1));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(1, new object()));
		}

		[Test]
		public void FloatSetSubtract()
		{
			Assert.AreEqual(0f, SetSubtract(1f, 1f));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(1.5f, "a"));
			Assert.Throws<InvalidOperationException>(() => SetSubtract("a", 1.5f));

			Assert.Throws<InvalidOperationException>(() => SetSubtract(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(1f, new object()));
		}

		[Test]
		public void StringSetSubtract()
		{
			Assert.Throws<InvalidOperationException>(() => SetSubtract("a", "b"));
			Assert.Throws<InvalidOperationException>(() => SetSubtract("b", "a"));

			Assert.Throws<InvalidOperationException>(() => SetSubtract("a", new object()));
			Assert.Throws<InvalidOperationException>(() => SetSubtract(new object(), "a"));
		}

		[Test]
		public void ObjectSetSubtract()
		{
			Assert.Throws<InvalidOperationException>(() => SetSubtract(new object(), new object()));
		}
	}
}

#endif
