#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the less than operators work as expected.
	/// </summary>
	public class LessThanTest
	{
		#region Helpers

		private object LessThan(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new LessThanExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Less Than

		[Test]
		public void BoolLessThanBool()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(false, false));

			Assert.Throws<InvalidOperationException>(() => LessThan(false, true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, false));

			Assert.Throws<InvalidOperationException>(() => LessThan(true, true));
		}

		[Test]
		public void BoolLessThanInt()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(0, false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, 0));

			Assert.Throws<InvalidOperationException>(() => LessThan(0, true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, 0));

			Assert.Throws<InvalidOperationException>(() => LessThan(1, false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, 1));

			Assert.Throws<InvalidOperationException>(() => LessThan(1, true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, 1));
		}

		[Test]
		public void BoolLessThanFloat()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(0f, false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, 0f));

			Assert.Throws<InvalidOperationException>(() => LessThan(0f, true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, 0f));

			Assert.Throws<InvalidOperationException>(() => LessThan(1f, false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, 1f));

			Assert.Throws<InvalidOperationException>(() => LessThan(1f, true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, 1f));
		}

		[Test]
		public void BoolLessThanString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan("", false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("", true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, "a"));
		}

		[Test]
		public void BoolLessThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(null, false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(null, true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), false));
			Assert.Throws<InvalidOperationException>(() => LessThan(false, new object()));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), true));
			Assert.Throws<InvalidOperationException>(() => LessThan(true, new object()));
		}

		#endregion

		#region Int Less Than

		[Test]
		public void IntLessThanInt()
		{
			Assert.AreEqual(false, LessThan(0, 0));

			Assert.AreEqual(true, LessThan(0, 1));
			Assert.AreEqual(false, LessThan(1, 0));

			Assert.AreEqual(false, LessThan(1, 1));
		}

		[Test]
		public void IntLessThanFloat()
		{
			Assert.AreEqual(false, LessThan(0f, 0));
			Assert.AreEqual(false, LessThan(0, 0f));

			Assert.AreEqual(true, LessThan(0f, 1));
			Assert.AreEqual(false, LessThan(1, 0f));

			Assert.AreEqual(false, LessThan(1f, 0));
			Assert.AreEqual(true, LessThan(0, 1f));

			Assert.AreEqual(false, LessThan(1f, 1));
			Assert.AreEqual(false, LessThan(1, 1f));
		}

		[Test]
		public void IntLessThanString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan("", 0));
			Assert.Throws<InvalidOperationException>(() => LessThan(0, ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("", 1));
			Assert.Throws<InvalidOperationException>(() => LessThan(1, ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", 0));
			Assert.Throws<InvalidOperationException>(() => LessThan(0, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", 1));
			Assert.Throws<InvalidOperationException>(() => LessThan(1, "a"));
		}

		[Test]
		public void IntLessThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(null, 0));
			Assert.Throws<InvalidOperationException>(() => LessThan(0, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(null, 1));
			Assert.Throws<InvalidOperationException>(() => LessThan(1, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), 0));
			Assert.Throws<InvalidOperationException>(() => LessThan(0, new object()));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => LessThan(1, new object()));
		}

		#endregion

		#region Float Less Than

		[Test]
		public void FloatLessThanFloat()
		{
			Assert.AreEqual(false, LessThan(0f, 0f));

			Assert.AreEqual(true, LessThan(0f, 1f));
			Assert.AreEqual(false, LessThan(1f, 0f));

			Assert.AreEqual(false, LessThan(1f, 1f));
		}

		[Test]
		public void FloatLessThanString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan("", 0f));
			Assert.Throws<InvalidOperationException>(() => LessThan(0f, ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("", 1f));
			Assert.Throws<InvalidOperationException>(() => LessThan(1f, ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", 0f));
			Assert.Throws<InvalidOperationException>(() => LessThan(0f, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", 1f));
			Assert.Throws<InvalidOperationException>(() => LessThan(1f, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThan("1", 1f));
			Assert.Throws<InvalidOperationException>(() => LessThan(1f, "1"));
		}

		[Test]
		public void FloatLessThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(null, 0f));
			Assert.Throws<InvalidOperationException>(() => LessThan(0f, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(null, 1f));
			Assert.Throws<InvalidOperationException>(() => LessThan(1f, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), 0f));
			Assert.Throws<InvalidOperationException>(() => LessThan(0f, new object()));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => LessThan(1f, new object()));
		}

		#endregion

		#region String Less Than

		[Test]
		public void StringLessThanString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan("", ""));
			Assert.Throws<InvalidOperationException>(() => LessThan("", ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("", "b"));
			Assert.Throws<InvalidOperationException>(() => LessThan("b", ""));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", ""));
			Assert.Throws<InvalidOperationException>(() => LessThan("", "a"));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", "b"));
			Assert.Throws<InvalidOperationException>(() => LessThan("b", "a"));

			Assert.Throws<InvalidOperationException>(() => LessThan("a", "a"));
		}

		[Test]
		public void StringLessThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(null, ""));
			Assert.Throws<InvalidOperationException>(() => LessThan("", null));

			Assert.Throws<InvalidOperationException>(() => LessThan(null, "a"));
			Assert.Throws<InvalidOperationException>(() => LessThan("a", null));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), ""));
			Assert.Throws<InvalidOperationException>(() => LessThan("", new object()));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), "a"));
			Assert.Throws<InvalidOperationException>(() => LessThan("a", new object()));
		}

		#endregion

		#region Object Less Than

		[Test]
		public void ObjectLessThanObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThan(null, null));

			Assert.Throws<InvalidOperationException>(() => LessThan(null, new object()));
			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), null));

			Assert.Throws<InvalidOperationException>(() => LessThan(new object(), new object()));
		}

		#endregion
	}
}

#endif
