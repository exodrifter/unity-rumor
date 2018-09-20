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
	public class AddTest
	{
		#region Helpers

		private object Add(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new AddExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NullPlus()
		{
			Assert.AreEqual(null, Add(null, null));

			Assert.Throws<InvalidOperationException>(() => Add(true, null));
			Assert.Throws<InvalidOperationException>(() => Add(null, true));

			Assert.AreEqual(1, Add(1, null));
			Assert.AreEqual(1, Add(null, 1));

			Assert.AreEqual(1f, Add(1f, null));
			Assert.AreEqual(1f, Add(null, 1f));

			Assert.AreEqual("a", Add("a", null));
			Assert.AreEqual("a", Add(null, "a"));

			Assert.Throws<InvalidOperationException>(() => Add(new object(), null));
			Assert.Throws<InvalidOperationException>(() => Add(null, new object()));
		}

		[Test]
		public void BoolPlus()
		{
			Assert.Throws<InvalidOperationException>(() => Add(true, true));

			Assert.Throws<InvalidOperationException>(() => Add(1, true));
			Assert.Throws<InvalidOperationException>(() => Add(true, 1));

			Assert.Throws<InvalidOperationException>(() => Add(1f, true));
			Assert.Throws<InvalidOperationException>(() => Add(true, 1f));

			Assert.AreEqual("atrue", Add("a", true));
			Assert.AreEqual("truea", Add(true, "a"));

			Assert.Throws<InvalidOperationException>(() => Add(new object(), true));
			Assert.Throws<InvalidOperationException>(() => Add(true, new object()));
		}

		[Test]
		public void IntPlus()
		{
			Assert.AreEqual(2, Add(1, 1));
			Assert.AreEqual(2.GetType(), Add(1, 1).GetType());

			Assert.AreEqual(2f, Add(1f, 1));
			Assert.AreEqual(2f, Add(1, 1f));
			Assert.AreEqual(2f.GetType(), Add(1, 1f).GetType());
			Assert.AreEqual(2f.GetType(), Add(1f, 1).GetType());

			Assert.AreEqual("1a", Add(1, "a"));
			Assert.AreEqual("a1", Add("a", 1));

			Assert.Throws<InvalidOperationException>(() => Add(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => Add(1, new object()));
		}

		[Test]
		public void FloatPlus()
		{
			Assert.AreEqual(2f, Add(1f, 1f));

			Assert.AreEqual("1.5a", Add(1.5f, "a"));
			Assert.AreEqual("a1.5", Add("a", 1.5f));

			Assert.Throws<InvalidOperationException>(() => Add(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => Add(1f, new object()));
		}

		[Test]
		public void StringPlus()
		{
			Assert.AreEqual("ab", Add("a", "b"));
			Assert.AreEqual("ba", Add("b", "a"));

			Assert.AreEqual("aSystem.Object", Add("a", new object()));
			Assert.AreEqual("System.Objecta", Add(new object(), "a"));
		}

		[Test]
		public void ObjectPlus()
		{
			Assert.Throws<InvalidOperationException>(() => Add(new object(), new object()));
		}
	}
}

#endif
