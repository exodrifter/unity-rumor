#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the set add operators work as expected.
	/// </summary>
	public class SetAddTest
	{
		#region Helpers

		private object SetAdd(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new SetAddExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);
			exp.Evaluate(scope, new Bindings());

			return new VariableExpression("l")
				.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullSetAdd()
		{
			Assert.AreEqual(null, SetAdd(null, null));

			Assert.Throws<InvalidOperationException>(() => SetAdd(true, null));
			Assert.Throws<InvalidOperationException>(() => SetAdd(null, true));

			Assert.AreEqual(1, SetAdd(1, null));
			Assert.AreEqual(1, SetAdd(null, 1));

			Assert.AreEqual(1f, SetAdd(1f, null));
			Assert.AreEqual(1f, SetAdd(null, 1f));

			Assert.AreEqual("a", SetAdd("a", null));
			Assert.AreEqual("a", SetAdd(null, "a"));

			Assert.Throws<InvalidOperationException>(() => SetAdd(new object(), null));
			Assert.Throws<InvalidOperationException>(() => SetAdd(null, new object()));
		}

		[Test]
		public void BoolSetAdd()
		{
			Assert.Throws<InvalidOperationException>(() => SetAdd(true, true));

			Assert.Throws<InvalidOperationException>(() => SetAdd(1, true));
			Assert.Throws<InvalidOperationException>(() => SetAdd(true, 1));

			Assert.Throws<InvalidOperationException>(() => SetAdd(1f, true));
			Assert.Throws<InvalidOperationException>(() => SetAdd(true, 1f));

			Assert.AreEqual("atrue", SetAdd("a", true));
			Assert.AreEqual("truea", SetAdd(true, "a"));

			Assert.Throws<InvalidOperationException>(() => SetAdd(new object(), true));
			Assert.Throws<InvalidOperationException>(() => SetAdd(true, new object()));
		}

		[Test]
		public void IntSetAdd()
		{
			Assert.AreEqual(2, SetAdd(1, 1));
			Assert.AreEqual(2.GetType(), SetAdd(1, 1).GetType());

			Assert.AreEqual(2f, SetAdd(1f, 1));
			Assert.AreEqual(2f, SetAdd(1, 1f));
			Assert.AreEqual(2f.GetType(), SetAdd(1, 1f).GetType());
			Assert.AreEqual(2f.GetType(), SetAdd(1f, 1).GetType());

			Assert.AreEqual("1a", SetAdd(1, "a"));
			Assert.AreEqual("a1", SetAdd("a", 1));

			Assert.Throws<InvalidOperationException>(() => SetAdd(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => SetAdd(1, new object()));
		}

		[Test]
		public void FloatSetAdd()
		{
			Assert.AreEqual(2f, SetAdd(1f, 1f));

			Assert.AreEqual("1.5a", SetAdd(1.5f, "a"));
			Assert.AreEqual("a1.5", SetAdd("a", 1.5f));

			Assert.Throws<InvalidOperationException>(() => SetAdd(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => SetAdd(1f, new object()));
		}

		[Test]
		public void StringSetAdd()
		{
			Assert.AreEqual("ab", SetAdd("a", "b"));
			Assert.AreEqual("ba", SetAdd("b", "a"));

			Assert.AreEqual("aSystem.Object", SetAdd("a", new object()));
			Assert.AreEqual("System.Objecta", SetAdd(new object(), "a"));
		}

		[Test]
		public void ObjectSetAdd()
		{
			Assert.Throws<InvalidOperationException>(() => SetAdd(new object(), new object()));
		}
	}
}

#endif
