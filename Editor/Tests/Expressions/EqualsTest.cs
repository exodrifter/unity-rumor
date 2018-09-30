#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the equals operators work as expected.
	/// </summary>
	public class EqualsTest
	{
		#region Helpers

		private object Equal(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new EqualsExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Equal

		[Test]
		public void BoolEqualBool()
		{
			Assert.AreEqual(true, Equal(false, false));

			Assert.AreEqual(false, Equal(false, true));
			Assert.AreEqual(false, Equal(true, false));

			Assert.AreEqual(true, Equal(true, true));
		}

		[Test]
		public void BoolEqualInt()
		{
			Assert.AreEqual(false, Equal(0, false));
			Assert.AreEqual(false, Equal(false, 0));

			Assert.AreEqual(false, Equal(0, true));
			Assert.AreEqual(false, Equal(true, 0));

			Assert.AreEqual(false, Equal(1, false));
			Assert.AreEqual(false, Equal(false, 1));

			Assert.AreEqual(false, Equal(1, true));
			Assert.AreEqual(false, Equal(true, 1));
		}

		[Test]
		public void BoolEqualFloat()
		{
			Assert.AreEqual(false, Equal(0f, false));
			Assert.AreEqual(false, Equal(false, 0f));

			Assert.AreEqual(false, Equal(0f, true));
			Assert.AreEqual(false, Equal(true, 0f));

			Assert.AreEqual(false, Equal(1f, false));
			Assert.AreEqual(false, Equal(false, 1f));

			Assert.AreEqual(false, Equal(1f, true));
			Assert.AreEqual(false, Equal(true, 1f));
		}

		[Test]
		public void BoolEqualString()
		{
			Assert.AreEqual(false, Equal("", false));
			Assert.AreEqual(false, Equal(false, ""));

			Assert.AreEqual(false, Equal("", true));
			Assert.AreEqual(false, Equal(true, ""));

			Assert.AreEqual(false, Equal("a", false));
			Assert.AreEqual(false, Equal(false, "a"));

			Assert.AreEqual(false, Equal("a", true));
			Assert.AreEqual(false, Equal(true, "a"));
		}

		[Test]
		public void BoolEqualObject()
		{
			Assert.AreEqual(false, Equal(null, false));
			Assert.AreEqual(false, Equal(false, null));

			Assert.AreEqual(false, Equal(null, true));
			Assert.AreEqual(false, Equal(true, null));

			Assert.AreEqual(false, Equal(new object(), false));
			Assert.AreEqual(false, Equal(false, new object()));

			Assert.AreEqual(false, Equal(new object(), true));
			Assert.AreEqual(false, Equal(true, new object()));
		}

		#endregion

		#region Int Equal

		[Test]
		public void IntEqualInt()
		{
			Assert.AreEqual(true, Equal(0, 0));

			Assert.AreEqual(false, Equal(0, 1));
			Assert.AreEqual(false, Equal(1, 0));

			Assert.AreEqual(false, Equal(1, 0));
			Assert.AreEqual(false, Equal(0, 1));

			Assert.AreEqual(true, Equal(1, 1));
		}

		[Test]
		public void IntEqualFloat()
		{
			Assert.AreEqual(true, Equal(0f, 0));
			Assert.AreEqual(true, Equal(0, 0f));

			Assert.AreEqual(false, Equal(0f, 1));
			Assert.AreEqual(false, Equal(1, 0f));

			Assert.AreEqual(false, Equal(1f, 0));
			Assert.AreEqual(false, Equal(0, 1f));

			Assert.AreEqual(true, Equal(1f, 1));
			Assert.AreEqual(true, Equal(1, 1f));
		}

		[Test]
		public void IntEqualString()
		{
			Assert.AreEqual(false, Equal("", 0));
			Assert.AreEqual(false, Equal(0, ""));

			Assert.AreEqual(false, Equal("", 1));
			Assert.AreEqual(false, Equal(1, ""));

			Assert.AreEqual(false, Equal("a", 0));
			Assert.AreEqual(false, Equal(0, "a"));

			Assert.AreEqual(false, Equal("a", 1));
			Assert.AreEqual(false, Equal(1, "a"));

			Assert.AreEqual(false, Equal("1", 1));
			Assert.AreEqual(false, Equal(1, "1"));
		}

		[Test]
		public void IntEqualObject()
		{
			Assert.AreEqual(false, Equal(null, 0));
			Assert.AreEqual(false, Equal(0, null));

			Assert.AreEqual(false, Equal(null, 1));
			Assert.AreEqual(false, Equal(1, null));

			Assert.AreEqual(false, Equal(new object(), 0));
			Assert.AreEqual(false, Equal(0, new object()));

			Assert.AreEqual(false, Equal(new object(), 1));
			Assert.AreEqual(false, Equal(1, new object()));
		}

		#endregion

		#region Float Equal

		[Test]
		public void FloatEqualFloat()
		{
			Assert.AreEqual(true, Equal(0f, 0f));

			Assert.AreEqual(false, Equal(0f, 1f));
			Assert.AreEqual(false, Equal(1f, 0f));

			Assert.AreEqual(false, Equal(1f, 0f));
			Assert.AreEqual(false, Equal(0f, 1f));

			Assert.AreEqual(true, Equal(1f, 1f));
		}

		[Test]
		public void FloatEqualString()
		{
			Assert.AreEqual(false, Equal("", 0f));
			Assert.AreEqual(false, Equal(0f, ""));

			Assert.AreEqual(false, Equal("", 1f));
			Assert.AreEqual(false, Equal(1f, ""));

			Assert.AreEqual(false, Equal("a", 0f));
			Assert.AreEqual(false, Equal(0f, "a"));

			Assert.AreEqual(false, Equal("a", 1f));
			Assert.AreEqual(false, Equal(1f, "a"));

			Assert.AreEqual(false, Equal("1", 1f));
			Assert.AreEqual(false, Equal(1f, "1"));
		}

		[Test]
		public void FloatEqualObject()
		{
			Assert.AreEqual(false, Equal(null, 0f));
			Assert.AreEqual(false, Equal(0f, null));

			Assert.AreEqual(false, Equal(null, 1f));
			Assert.AreEqual(false, Equal(1f, null));

			Assert.AreEqual(false, Equal(new object(), 0f));
			Assert.AreEqual(false, Equal(0f, new object()));

			Assert.AreEqual(false, Equal(new object(), 1f));
			Assert.AreEqual(false, Equal(1f, new object()));
		}

		#endregion

		#region String Equal

		[Test]
		public void StringEqualString()
		{
			Assert.AreEqual(true, Equal("", ""));

			Assert.AreEqual(false, Equal("", "b"));
			Assert.AreEqual(false, Equal("b", ""));

			Assert.AreEqual(false, Equal("a", ""));
			Assert.AreEqual(false, Equal("", "a"));

			Assert.AreEqual(false, Equal("a", "b"));
			Assert.AreEqual(false, Equal("b", "a"));

			Assert.AreEqual(true, Equal("a", "a"));
		}

		[Test]
		public void StringEqualObject()
		{
			Assert.AreEqual(false, Equal(null, ""));
			Assert.AreEqual(false, Equal("", null));

			Assert.AreEqual(false, Equal(null, "a"));
			Assert.AreEqual(false, Equal("a", null));

			Assert.AreEqual(false, Equal(new object(), ""));
			Assert.AreEqual(false, Equal("", new object()));

			Assert.AreEqual(false, Equal(new object(), "a"));
			Assert.AreEqual(false, Equal("a", new object()));
		}

		#endregion

		#region Object Equal

		[Test]
		public void ObjectEqualObject()
		{
			Assert.AreEqual(true, Equal(null, null));

			Assert.AreEqual(false, Equal(null, new object()));
			Assert.AreEqual(false, Equal(new object(), null));

			Assert.AreEqual(false, Equal(new object(), new object()));

			var obj = new object();
			Assert.AreEqual(true, Equal(obj, obj));
		}

		#endregion
	}
}

#endif
