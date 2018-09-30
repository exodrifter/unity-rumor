#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the xor operators work as expected.
	/// </summary>
	public class BoolXorTest
	{
		#region Helpers

		private object Xor(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new BoolXorExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Xor

		[Test]
		public void BoolXorBool()
		{
			Assert.AreEqual(false, Xor(false, false));

			Assert.AreEqual(true, Xor(false, true));
			Assert.AreEqual(true, Xor(true, false));

			Assert.AreEqual(false, Xor(true, true));
		}

		[Test]
		public void BoolXorInt()
		{
			Assert.AreEqual(false, Xor(0, false));
			Assert.AreEqual(false, Xor(false, 0));

			Assert.AreEqual(true, Xor(0, true));
			Assert.AreEqual(true, Xor(true, 0));

			Assert.AreEqual(true, Xor(1, false));
			Assert.AreEqual(true, Xor(false, 1));

			Assert.AreEqual(false, Xor(1, true));
			Assert.AreEqual(false, Xor(true, 1));
		}

		[Test]
		public void BoolXorFloat()
		{
			Assert.AreEqual(false, Xor(0f, false));
			Assert.AreEqual(false, Xor(false, 0f));

			Assert.AreEqual(true, Xor(0f, true));
			Assert.AreEqual(true, Xor(true, 0f));

			Assert.AreEqual(true, Xor(1f, false));
			Assert.AreEqual(true, Xor(false, 1f));

			Assert.AreEqual(false, Xor(1f, true));
			Assert.AreEqual(false, Xor(true, 1f));
		}

		[Test]
		public void BoolXorString()
		{
			Assert.AreEqual(false, Xor("", false));
			Assert.AreEqual(false, Xor(false, ""));

			Assert.AreEqual(true, Xor("", true));
			Assert.AreEqual(true, Xor(true, ""));

			Assert.AreEqual(true, Xor("a", false));
			Assert.AreEqual(true, Xor(false, "a"));

			Assert.AreEqual(false, Xor("a", true));
			Assert.AreEqual(false, Xor(true, "a"));
		}

		[Test]
		public void BoolXorObject()
		{
			Assert.AreEqual(false, Xor(null, false));
			Assert.AreEqual(false, Xor(false, null));

			Assert.AreEqual(true, Xor(null, true));
			Assert.AreEqual(true, Xor(true, null));

			Assert.AreEqual(true, Xor(new object(), false));
			Assert.AreEqual(true, Xor(false, new object()));

			Assert.AreEqual(false, Xor(new object(), true));
			Assert.AreEqual(false, Xor(true, new object()));
		}

		#endregion

		#region Int Xor

		[Test]
		public void IntXorInt()
		{
			Assert.AreEqual(false, Xor(0, 0));
			Assert.AreEqual(false, Xor(0, 0));

			Assert.AreEqual(true, Xor(0, 1));
			Assert.AreEqual(true, Xor(1, 0));

			Assert.AreEqual(true, Xor(1, 0));
			Assert.AreEqual(true, Xor(0, 1));

			Assert.AreEqual(false, Xor(1, 1));
			Assert.AreEqual(false, Xor(1, 1));
		}

		[Test]
		public void IntXorFloat()
		{
			Assert.AreEqual(false, Xor(0f, 0));
			Assert.AreEqual(false, Xor(0, 0f));

			Assert.AreEqual(true, Xor(0f, 1));
			Assert.AreEqual(true, Xor(1, 0f));

			Assert.AreEqual(true, Xor(1f, 0));
			Assert.AreEqual(true, Xor(0, 1f));

			Assert.AreEqual(false, Xor(1f, 1));
			Assert.AreEqual(false, Xor(1, 1f));
		}

		[Test]
		public void IntXorString()
		{
			Assert.AreEqual(false, Xor("", 0));
			Assert.AreEqual(false, Xor(0, ""));

			Assert.AreEqual(true, Xor("", 1));
			Assert.AreEqual(true, Xor(1, ""));

			Assert.AreEqual(true, Xor("a", 0));
			Assert.AreEqual(true, Xor(0, "a"));

			Assert.AreEqual(false, Xor("a", 1));
			Assert.AreEqual(false, Xor(1, "a"));
		}

		[Test]
		public void IntXorObject()
		{
			Assert.AreEqual(false, Xor(null, 0));
			Assert.AreEqual(false, Xor(0, null));

			Assert.AreEqual(true, Xor(null, 1));
			Assert.AreEqual(true, Xor(1, null));

			Assert.AreEqual(true, Xor(new object(), 0));
			Assert.AreEqual(true, Xor(0, new object()));

			Assert.AreEqual(false, Xor(new object(), 1));
			Assert.AreEqual(false, Xor(1, new object()));
		}

		#endregion

		#region Float Xor

		[Test]
		public void FloatXorFloat()
		{
			Assert.AreEqual(false, Xor(0f, 0f));
			Assert.AreEqual(false, Xor(0f, 0f));

			Assert.AreEqual(true, Xor(0f, 1f));
			Assert.AreEqual(true, Xor(1f, 0f));

			Assert.AreEqual(true, Xor(1f, 0f));
			Assert.AreEqual(true, Xor(0f, 1f));

			Assert.AreEqual(false, Xor(1f, 1f));
			Assert.AreEqual(false, Xor(1f, 1f));
		}

		[Test]
		public void FloatXorString()
		{
			Assert.AreEqual(false, Xor("", 0f));
			Assert.AreEqual(false, Xor(0f, ""));

			Assert.AreEqual(true, Xor("", 1f));
			Assert.AreEqual(true, Xor(1f, ""));

			Assert.AreEqual(true, Xor("a", 0f));
			Assert.AreEqual(true, Xor(0f, "a"));

			Assert.AreEqual(false, Xor("a", 1f));
			Assert.AreEqual(false, Xor(1f, "a"));
		}

		[Test]
		public void FloatXorObject()
		{
			Assert.AreEqual(false, Xor(null, 0f));
			Assert.AreEqual(false, Xor(0f, null));

			Assert.AreEqual(true, Xor(null, 1f));
			Assert.AreEqual(true, Xor(1f, null));

			Assert.AreEqual(true, Xor(new object(), 0f));
			Assert.AreEqual(true, Xor(0f, new object()));

			Assert.AreEqual(false, Xor(new object(), 1f));
			Assert.AreEqual(false, Xor(1f, new object()));
		}

		#endregion

		#region String Xor

		[Test]
		public void StringXorString()
		{
			Assert.AreEqual(false, Xor("", ""));
			Assert.AreEqual(false, Xor("", ""));

			Assert.AreEqual(true, Xor("", "b"));
			Assert.AreEqual(true, Xor("b", ""));

			Assert.AreEqual(true, Xor("a", ""));
			Assert.AreEqual(true, Xor("", "a"));

			Assert.AreEqual(false, Xor("a", "b"));
			Assert.AreEqual(false, Xor("b", "a"));
		}

		[Test]
		public void StringXorObject()
		{
			Assert.AreEqual(false, Xor(null, ""));
			Assert.AreEqual(false, Xor("", null));

			Assert.AreEqual(true, Xor(null, "a"));
			Assert.AreEqual(true, Xor("a", null));

			Assert.AreEqual(true, Xor(new object(), ""));
			Assert.AreEqual(true, Xor("", new object()));

			Assert.AreEqual(false, Xor(new object(), "a"));
			Assert.AreEqual(false, Xor("a", new object()));
		}

		#endregion

		#region Object Xor

		[Test]
		public void ObjectXorObject()
		{
			Assert.AreEqual(false, Xor(null, null));

			Assert.AreEqual(true, Xor(null, new object()));
			Assert.AreEqual(true, Xor(new object(), null));

			Assert.AreEqual(false, Xor(new object(), new object()));
		}

		#endregion
	}
}

#endif
