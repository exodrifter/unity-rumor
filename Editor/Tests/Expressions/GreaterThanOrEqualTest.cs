#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the greater than or equal operators work as expected.
	/// </summary>
	public class GreaterThanOrEqualTest
	{
		#region Helpers

		private object GreaterThanOrEqual(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new GreaterThanOrEqualExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Greater Than

		[Test]
		public void BoolGreaterThanOrEqualBool()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, false));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, false));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, true));
		}

		[Test]
		public void BoolGreaterThanOrEqualInt()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, 0));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, 0));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, 1));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, 1));
		}

		[Test]
		public void BoolGreaterThanOrEqualFloat()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0f, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, 0f));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0f, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, 0f));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, 1f));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, 1f));
		}

		[Test]
		public void BoolGreaterThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, "a"));
		}

		[Test]
		public void BoolGreaterThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), false));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(false, new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), true));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(true, new object()));
		}

		#endregion

		#region Int Greater Than

		[Test]
		public void IntGreaterThanOrEqualInt()
		{
			Assert.AreEqual(true, GreaterThanOrEqual(0, 0));

			Assert.AreEqual(false, GreaterThanOrEqual(0, 1));
			Assert.AreEqual(true, GreaterThanOrEqual(1, 0));

			Assert.AreEqual(true, GreaterThanOrEqual(1, 1));
		}

		[Test]
		public void IntGreaterThanOrEqualFloat()
		{
			Assert.AreEqual(true, GreaterThanOrEqual(0f, 0));
			Assert.AreEqual(true, GreaterThanOrEqual(0, 0f));

			Assert.AreEqual(false, GreaterThanOrEqual(0f, 1));
			Assert.AreEqual(true, GreaterThanOrEqual(1, 0f));

			Assert.AreEqual(true, GreaterThanOrEqual(1f, 0));
			Assert.AreEqual(false, GreaterThanOrEqual(0, 1f));

			Assert.AreEqual(true, GreaterThanOrEqual(1f, 1));
			Assert.AreEqual(true, GreaterThanOrEqual(1, 1f));
		}

		[Test]
		public void IntGreaterThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1, "a"));
		}

		[Test]
		public void IntGreaterThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0, new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1, new object()));
		}

		#endregion

		#region Float Greater Than

		[Test]
		public void FloatGreaterThanOrEqualFloat()
		{
			Assert.AreEqual(true, GreaterThanOrEqual(0f, 0f));

			Assert.AreEqual(false, GreaterThanOrEqual(0f, 1f));
			Assert.AreEqual(true, GreaterThanOrEqual(1f, 0f));

			Assert.AreEqual(true, GreaterThanOrEqual(1f, 1f));
		}

		[Test]
		public void FloatGreaterThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0f, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0f, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("1", 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, "1"));
		}

		[Test]
		public void FloatGreaterThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0f, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(0f, new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(1f, new object()));
		}

		#endregion

		#region String Greater Than

		[Test]
		public void StringGreaterThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", "b"));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("b", ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", "b"));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("b", "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", "a"));
		}

		[Test]
		public void StringGreaterThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, "a"));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("", new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), "a"));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual("a", new object()));
		}

		#endregion

		#region Object Greater Than

		[Test]
		public void ObjectGreaterThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(null, new object()));
			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), null));

			Assert.Throws<InvalidOperationException>(() => GreaterThanOrEqual(new object(), new object()));
		}

		#endregion
	}
}

#endif
