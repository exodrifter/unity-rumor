#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the less than or equal operators work as expected.
	/// </summary>
	public class LessThanOrEqualTest
	{
		#region Helpers

		private object LessThanOrEqual(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new LessThanOrEqualExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Greater Than

		[Test]
		public void BoolLessThanOrEqualBool()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, false));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, false));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, true));
		}

		[Test]
		public void BoolLessThanOrEqualInt()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0, false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, 0));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0, true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, 0));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1, false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, 1));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1, true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, 1));
		}

		[Test]
		public void BoolLessThanOrEqualFloat()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0f, false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, 0f));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0f, true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, 0f));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, 1f));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, 1f));
		}

		[Test]
		public void BoolLessThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, "a"));
		}

		[Test]
		public void BoolLessThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), false));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(false, new object()));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), true));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(true, new object()));
		}

		#endregion

		#region Int Greater Than

		[Test]
		public void IntLessThanOrEqualInt()
		{
			Assert.AreEqual(true, LessThanOrEqual(0, 0));

			Assert.AreEqual(true, LessThanOrEqual(0, 1));
			Assert.AreEqual(false, LessThanOrEqual(1, 0));

			Assert.AreEqual(true, LessThanOrEqual(1, 1));
		}

		[Test]
		public void IntLessThanOrEqualFloat()
		{
			Assert.AreEqual(true, LessThanOrEqual(0f, 0));
			Assert.AreEqual(true, LessThanOrEqual(0, 0f));

			Assert.AreEqual(true, LessThanOrEqual(0f, 1));
			Assert.AreEqual(false, LessThanOrEqual(1, 0f));

			Assert.AreEqual(false, LessThanOrEqual(1f, 0));
			Assert.AreEqual(true, LessThanOrEqual(0, 1f));

			Assert.AreEqual(true, LessThanOrEqual(1f, 1));
			Assert.AreEqual(true, LessThanOrEqual(1, 1f));
		}

		[Test]
		public void IntLessThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", 0));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0, ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", 1));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1, ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", 0));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", 1));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1, "a"));
		}

		[Test]
		public void IntLessThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, 0));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, 1));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), 0));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0, new object()));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), 1));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1, new object()));
		}

		#endregion

		#region Float Greater Than

		[Test]
		public void FloatLessThanOrEqualFloat()
		{
			Assert.AreEqual(true, LessThanOrEqual(0f, 0f));

			Assert.AreEqual(true, LessThanOrEqual(0f, 1f));
			Assert.AreEqual(false, LessThanOrEqual(1f, 0f));

			Assert.AreEqual(true, LessThanOrEqual(1f, 1f));
		}

		[Test]
		public void FloatLessThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", 0f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0f, ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", 1f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", 0f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0f, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", 1f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, "a"));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("1", 1f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, "1"));
		}

		[Test]
		public void FloatLessThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, 0f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0f, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, 1f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), 0f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(0f, new object()));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), 1f));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(1f, new object()));
		}

		#endregion

		#region String Greater Than

		[Test]
		public void StringLessThanOrEqualString()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", ""));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", "b"));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("b", ""));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", ""));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", "a"));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", "b"));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("b", "a"));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", "a"));
		}

		[Test]
		public void StringLessThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, ""));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, "a"));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), ""));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("", new object()));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), "a"));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual("a", new object()));
		}

		#endregion

		#region Object Greater Than

		[Test]
		public void ObjectLessThanOrEqualObject()
		{
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(null, new object()));
			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), null));

			Assert.Throws<InvalidOperationException>(() => LessThanOrEqual(new object(), new object()));
		}

		#endregion
	}
}

#endif
