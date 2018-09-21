#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the greater than operators work as expected.
	/// </summary>
	public class GreaterThanTest
	{
		#region Helpers

		private object GreaterThan(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new GreaterThanExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Greater Than

		[Test]
		public void BoolGreaterThanBool()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, false));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, false));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, true));
		}

		[Test]
		public void BoolGreaterThanInt()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, 0));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(0, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, 0));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(1, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, 1));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(1, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, 1));
		}

		[Test]
		public void BoolGreaterThanFloat()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0f, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, 0f));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(0f, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, 0f));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, 1f));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, 1f));
		}

		[Test]
		public void BoolGreaterThanString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("", true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, "a"));
		}

		[Test]
		public void BoolGreaterThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), false));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(false, new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), true));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(true, new object()));
		}

		#endregion

		#region Int Greater Than

		[Test]
		public void IntGreaterThanInt()
		{
			Assert.AreEqual(false, GreaterThan(0, 0));

			Assert.AreEqual(false, GreaterThan(0, 1));
			Assert.AreEqual(true, GreaterThan(1, 0));

			Assert.AreEqual(false, GreaterThan(1, 1));
		}

		[Test]
		public void IntGreaterThanFloat()
		{
			Assert.AreEqual(false, GreaterThan(0f, 0));
			Assert.AreEqual(false, GreaterThan(0, 0f));

			Assert.AreEqual(false, GreaterThan(0f, 1));
			Assert.AreEqual(true, GreaterThan(1, 0f));

			Assert.AreEqual(true, GreaterThan(1f, 0));
			Assert.AreEqual(false, GreaterThan(0, 1f));

			Assert.AreEqual(false, GreaterThan(1f, 1));
			Assert.AreEqual(false, GreaterThan(1, 1f));
		}

		[Test]
		public void IntGreaterThanString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("", 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1, "a"));
		}

		[Test]
		public void IntGreaterThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), 0));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0, new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1, new object()));
		}

		#endregion

		#region Float Greater Than

		[Test]
		public void FloatGreaterThanFloat()
		{
			Assert.AreEqual(false, GreaterThan(0f, 0f));

			Assert.AreEqual(false, GreaterThan(0f, 1f));
			Assert.AreEqual(true, GreaterThan(1f, 0f));

			Assert.AreEqual(false, GreaterThan(1f, 1f));
		}

		[Test]
		public void FloatGreaterThanString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0f, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("", 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0f, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("1", 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, "1"));
		}

		[Test]
		public void FloatGreaterThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0f, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), 0f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(0f, new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(1f, new object()));
		}

		#endregion

		#region String Greater Than

		[Test]
		public void StringGreaterThanString()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("", "b"));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("b", ""));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", "b"));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("b", "a"));

			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", "a"));
		}

		[Test]
		public void StringGreaterThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, "a"));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), ""));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("", new object()));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), "a"));
			Assert.Throws<InvalidOperationException>(() => GreaterThan("a", new object()));
		}

		#endregion

		#region Object Greater Than

		[Test]
		public void ObjectGreaterThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(null, new object()));
			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), null));

			Assert.Throws<InvalidOperationException>(() => GreaterThan(new object(), new object()));
		}

		#endregion
	}
}

#endif
