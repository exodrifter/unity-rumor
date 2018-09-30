#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the or operators work as expected.
	/// </summary>
	public class BoolOrTest
	{
		#region Helpers

		private object Or(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new BoolOrExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Or

		[Test]
		public void BoolOrBool()
		{
			Assert.AreEqual(false, Or(false, false));

			Assert.AreEqual(true, Or(false, true));
			Assert.AreEqual(true, Or(true, false));

			Assert.AreEqual(true, Or(true, true));
		}

		[Test]
		public void BoolOrInt()
		{
			Assert.AreEqual(false, Or(0, false));
			Assert.AreEqual(false, Or(false, 0));

			Assert.AreEqual(true, Or(0, true));
			Assert.AreEqual(true, Or(true, 0));

			Assert.AreEqual(true, Or(1, false));
			Assert.AreEqual(true, Or(false, 1));

			Assert.AreEqual(true, Or(1, true));
			Assert.AreEqual(true, Or(true, 1));
		}

		[Test]
		public void BoolOrFloat()
		{
			Assert.AreEqual(false, Or(0f, false));
			Assert.AreEqual(false, Or(false, 0f));

			Assert.AreEqual(true, Or(0f, true));
			Assert.AreEqual(true, Or(true, 0f));

			Assert.AreEqual(true, Or(1f, false));
			Assert.AreEqual(true, Or(false, 1f));

			Assert.AreEqual(true, Or(1f, true));
			Assert.AreEqual(true, Or(true, 1f));
		}

		[Test]
		public void BoolOrString()
		{
			Assert.AreEqual(false, Or("", false));
			Assert.AreEqual(false, Or(false, ""));

			Assert.AreEqual(true, Or("", true));
			Assert.AreEqual(true, Or(true, ""));

			Assert.AreEqual(true, Or("a", false));
			Assert.AreEqual(true, Or(false, "a"));

			Assert.AreEqual(true, Or("a", true));
			Assert.AreEqual(true, Or(true, "a"));
		}

		[Test]
		public void BoolOrObject()
		{
			Assert.AreEqual(false, Or(null, false));
			Assert.AreEqual(false, Or(false, null));

			Assert.AreEqual(true, Or(null, true));
			Assert.AreEqual(true, Or(true, null));

			Assert.AreEqual(true, Or(new object(), false));
			Assert.AreEqual(true, Or(false, new object()));

			Assert.AreEqual(true, Or(new object(), true));
			Assert.AreEqual(true, Or(true, new object()));
		}

		#endregion

		#region Int Or

		[Test]
		public void IntOrInt()
		{
			Assert.AreEqual(false, Or(0, 0));
			Assert.AreEqual(false, Or(0, 0));

			Assert.AreEqual(true, Or(0, 1));
			Assert.AreEqual(true, Or(1, 0));

			Assert.AreEqual(true, Or(1, 0));
			Assert.AreEqual(true, Or(0, 1));

			Assert.AreEqual(true, Or(1, 1));
			Assert.AreEqual(true, Or(1, 1));
		}

		[Test]
		public void IntOrFloat()
		{
			Assert.AreEqual(false, Or(0f, 0));
			Assert.AreEqual(false, Or(0, 0f));

			Assert.AreEqual(true, Or(0f, 1));
			Assert.AreEqual(true, Or(1, 0f));

			Assert.AreEqual(true, Or(1f, 0));
			Assert.AreEqual(true, Or(0, 1f));

			Assert.AreEqual(true, Or(1f, 1));
			Assert.AreEqual(true, Or(1, 1f));
		}

		[Test]
		public void IntOrString()
		{
			Assert.AreEqual(false, Or("", 0));
			Assert.AreEqual(false, Or(0, ""));

			Assert.AreEqual(true, Or("", 1));
			Assert.AreEqual(true, Or(1, ""));

			Assert.AreEqual(true, Or("a", 0));
			Assert.AreEqual(true, Or(0, "a"));

			Assert.AreEqual(true, Or("a", 1));
			Assert.AreEqual(true, Or(1, "a"));
		}

		[Test]
		public void IntOrObject()
		{
			Assert.AreEqual(false, Or(null, 0));
			Assert.AreEqual(false, Or(0, null));

			Assert.AreEqual(true, Or(null, 1));
			Assert.AreEqual(true, Or(1, null));

			Assert.AreEqual(true, Or(new object(), 0));
			Assert.AreEqual(true, Or(0, new object()));

			Assert.AreEqual(true, Or(new object(), 1));
			Assert.AreEqual(true, Or(1, new object()));
		}

		#endregion

		#region Float Or

		[Test]
		public void FloatOrFloat()
		{
			Assert.AreEqual(false, Or(0f, 0f));
			Assert.AreEqual(false, Or(0f, 0f));

			Assert.AreEqual(true, Or(0f, 1f));
			Assert.AreEqual(true, Or(1f, 0f));

			Assert.AreEqual(true, Or(1f, 0f));
			Assert.AreEqual(true, Or(0f, 1f));

			Assert.AreEqual(true, Or(1f, 1f));
			Assert.AreEqual(true, Or(1f, 1f));
		}

		[Test]
		public void FloatOrString()
		{
			Assert.AreEqual(false, Or("", 0f));
			Assert.AreEqual(false, Or(0f, ""));

			Assert.AreEqual(true, Or("", 1f));
			Assert.AreEqual(true, Or(1f, ""));

			Assert.AreEqual(true, Or("a", 0f));
			Assert.AreEqual(true, Or(0f, "a"));

			Assert.AreEqual(true, Or("a", 1f));
			Assert.AreEqual(true, Or(1f, "a"));
		}

		[Test]
		public void FloatOrObject()
		{
			Assert.AreEqual(false, Or(null, 0f));
			Assert.AreEqual(false, Or(0f, null));

			Assert.AreEqual(true, Or(null, 1f));
			Assert.AreEqual(true, Or(1f, null));

			Assert.AreEqual(true, Or(new object(), 0f));
			Assert.AreEqual(true, Or(0f, new object()));

			Assert.AreEqual(true, Or(new object(), 1f));
			Assert.AreEqual(true, Or(1f, new object()));
		}

		#endregion

		#region String Or

		[Test]
		public void StringOrString()
		{
			Assert.AreEqual(false, Or("", ""));
			Assert.AreEqual(false, Or("", ""));

			Assert.AreEqual(true, Or("", "b"));
			Assert.AreEqual(true, Or("b", ""));

			Assert.AreEqual(true, Or("a", ""));
			Assert.AreEqual(true, Or("", "a"));

			Assert.AreEqual(true, Or("a", "b"));
			Assert.AreEqual(true, Or("b", "a"));
		}

		[Test]
		public void StringOrObject()
		{
			Assert.AreEqual(false, Or(null, ""));
			Assert.AreEqual(false, Or("", null));

			Assert.AreEqual(true, Or(null, "a"));
			Assert.AreEqual(true, Or("a", null));

			Assert.AreEqual(true, Or(new object(), ""));
			Assert.AreEqual(true, Or("", new object()));

			Assert.AreEqual(true, Or(new object(), "a"));
			Assert.AreEqual(true, Or("a", new object()));
		}

		#endregion

		#region Object Or

		[Test]
		public void ObjectOrObject()
		{
			Assert.AreEqual(false, Or(null, null));

			Assert.AreEqual(true, Or(null, new object()));
			Assert.AreEqual(true, Or(new object(), null));

			Assert.AreEqual(true, Or(new object(), new object()));
		}

		#endregion
	}
}

#endif
