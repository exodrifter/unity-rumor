#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the subtract operators work as expected.
	/// </summary>
	public class SubtractTest
	{
		#region Helpers

		private object Subtract(object l, object r)
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
		public void NullSubtract()
		{
			Assert.AreEqual(null, Subtract(null, null));

			Assert.Throws<InvalidOperationException>(() => Subtract(true, null));
			Assert.Throws<InvalidOperationException>(() => Subtract(null, true));

			Assert.AreEqual(1, Subtract(1, null));
			Assert.AreEqual(-1, Subtract(null, 1));

			Assert.AreEqual(1f, Subtract(1f, null));
			Assert.AreEqual(-1f, Subtract(null, 1f));

			Assert.Throws<InvalidOperationException>(() => Subtract("a", null));
			Assert.Throws<InvalidOperationException>(() => Subtract(null, "a"));

			Assert.Throws<InvalidOperationException>(() => Subtract(new object(), null));
			Assert.Throws<InvalidOperationException>(() => Subtract(null, new object()));
		}

		[Test]
		public void BoolSubtract()
		{
			Assert.Throws<InvalidOperationException>(() => Subtract(true, true));

			Assert.Throws<InvalidOperationException>(() => Subtract(1, true));
			Assert.Throws<InvalidOperationException>(() => Subtract(true, 1));

			Assert.Throws<InvalidOperationException>(() => Subtract(1f, true));
			Assert.Throws<InvalidOperationException>(() => Subtract(true, 1f));

			Assert.Throws<InvalidOperationException>(() => Subtract("a", true));
			Assert.Throws<InvalidOperationException>(() => Subtract(true, "a"));

			Assert.Throws<InvalidOperationException>(() => Subtract(new object(), true));
			Assert.Throws<InvalidOperationException>(() => Subtract(true, new object()));
		}

		[Test]
		public void IntSubtract()
		{
			Assert.AreEqual(0, Subtract(1, 1));
			Assert.AreEqual(0.GetType(), Subtract(1, 1).GetType());

			Assert.AreEqual(0f, Subtract(1f, 1));
			Assert.AreEqual(0f, Subtract(1, 1f));
			Assert.AreEqual(0f.GetType(), Subtract(1, 1f).GetType());
			Assert.AreEqual(0f.GetType(), Subtract(1f, 1).GetType());

			Assert.Throws<InvalidOperationException>(() => Subtract(1, "a"));
			Assert.Throws<InvalidOperationException>(() => Subtract("a", 1));

			Assert.Throws<InvalidOperationException>(() => Subtract(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => Subtract(1, new object()));
		}

		[Test]
		public void FloatSubtract()
		{
			Assert.AreEqual(0f, Subtract(1f, 1f));

			Assert.Throws<InvalidOperationException>(() => Subtract(1.5f, "a"));
			Assert.Throws<InvalidOperationException>(() => Subtract("a", 1.5f));

			Assert.Throws<InvalidOperationException>(() => Subtract(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => Subtract(1f, new object()));
		}

		[Test]
		public void StringSubtract()
		{
			Assert.Throws<InvalidOperationException>(() => Subtract("a", "b"));
			Assert.Throws<InvalidOperationException>(() => Subtract("b", "a"));

			Assert.Throws<InvalidOperationException>(() => Subtract("a", new object()));
			Assert.Throws<InvalidOperationException>(() => Subtract(new object(), "a"));
		}

		[Test]
		public void ObjectSubtract()
		{
			Assert.Throws<InvalidOperationException>(() => Subtract(new object(), new object()));
		}
	}
}

#endif
