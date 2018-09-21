#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the not equals operators work as expected.
	/// </summary>
	public class NotEqualsTest
	{
		#region Helpers

		private object NotEquals(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new NotEqualsExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool Not Equals

		[Test]
		public void BoolNotEqualsBool()
		{
			Assert.AreEqual(false, NotEquals(false, false));

			Assert.AreEqual(true, NotEquals(false, true));
			Assert.AreEqual(true, NotEquals(true, false));

			Assert.AreEqual(false, NotEquals(true, true));
		}

		[Test]
		public void BoolNotEqualsInt()
		{
			Assert.AreEqual(true, NotEquals(0, false));
			Assert.AreEqual(true, NotEquals(false, 0));

			Assert.AreEqual(true, NotEquals(0, true));
			Assert.AreEqual(true, NotEquals(true, 0));

			Assert.AreEqual(true, NotEquals(1, false));
			Assert.AreEqual(true, NotEquals(false, 1));

			Assert.AreEqual(true, NotEquals(1, true));
			Assert.AreEqual(true, NotEquals(true, 1));
		}

		[Test]
		public void BoolNotEqualsFloat()
		{
			Assert.AreEqual(true, NotEquals(0f, false));
			Assert.AreEqual(true, NotEquals(false, 0f));

			Assert.AreEqual(true, NotEquals(0f, true));
			Assert.AreEqual(true, NotEquals(true, 0f));

			Assert.AreEqual(true, NotEquals(1f, false));
			Assert.AreEqual(true, NotEquals(false, 1f));

			Assert.AreEqual(true, NotEquals(1f, true));
			Assert.AreEqual(true, NotEquals(true, 1f));
		}

		[Test]
		public void BoolNotEqualsString()
		{
			Assert.AreEqual(true, NotEquals("", false));
			Assert.AreEqual(true, NotEquals(false, ""));

			Assert.AreEqual(true, NotEquals("", true));
			Assert.AreEqual(true, NotEquals(true, ""));

			Assert.AreEqual(true, NotEquals("a", false));
			Assert.AreEqual(true, NotEquals(false, "a"));

			Assert.AreEqual(true, NotEquals("a", true));
			Assert.AreEqual(true, NotEquals(true, "a"));
		}

		[Test]
		public void BoolNotEqualsObject()
		{
			Assert.AreEqual(true, NotEquals(null, false));
			Assert.AreEqual(true, NotEquals(false, null));

			Assert.AreEqual(true, NotEquals(null, true));
			Assert.AreEqual(true, NotEquals(true, null));

			Assert.AreEqual(true, NotEquals(new object(), false));
			Assert.AreEqual(true, NotEquals(false, new object()));

			Assert.AreEqual(true, NotEquals(new object(), true));
			Assert.AreEqual(true, NotEquals(true, new object()));
		}

		#endregion

		#region Int Not Equals

		[Test]
		public void IntNotEqualsInt()
		{
			Assert.AreEqual(false, NotEquals(0, 0));

			Assert.AreEqual(true, NotEquals(0, 1));
			Assert.AreEqual(true, NotEquals(1, 0));

			Assert.AreEqual(true, NotEquals(1, 0));
			Assert.AreEqual(true, NotEquals(0, 1));

			Assert.AreEqual(false, NotEquals(1, 1));
		}

		[Test]
		public void IntNotEqualsFloat()
		{
			Assert.AreEqual(false, NotEquals(0f, 0));
			Assert.AreEqual(false, NotEquals(0, 0f));

			Assert.AreEqual(true, NotEquals(0f, 1));
			Assert.AreEqual(true, NotEquals(1, 0f));

			Assert.AreEqual(true, NotEquals(1f, 0));
			Assert.AreEqual(true, NotEquals(0, 1f));

			Assert.AreEqual(false, NotEquals(1f, 1));
			Assert.AreEqual(false, NotEquals(1, 1f));
		}

		[Test]
		public void IntNotEqualsString()
		{
			Assert.AreEqual(true, NotEquals("", 0));
			Assert.AreEqual(true, NotEquals(0, ""));

			Assert.AreEqual(true, NotEquals("", 1));
			Assert.AreEqual(true, NotEquals(1, ""));

			Assert.AreEqual(true, NotEquals("a", 0));
			Assert.AreEqual(true, NotEquals(0, "a"));

			Assert.AreEqual(true, NotEquals("a", 1));
			Assert.AreEqual(true, NotEquals(1, "a"));

			Assert.AreEqual(true, NotEquals("1", 1));
			Assert.AreEqual(true, NotEquals(1, "1"));
		}

		[Test]
		public void IntNotEqualsObject()
		{
			Assert.AreEqual(true, NotEquals(null, 0));
			Assert.AreEqual(true, NotEquals(0, null));

			Assert.AreEqual(true, NotEquals(null, 1));
			Assert.AreEqual(true, NotEquals(1, null));

			Assert.AreEqual(true, NotEquals(new object(), 0));
			Assert.AreEqual(true, NotEquals(0, new object()));

			Assert.AreEqual(true, NotEquals(new object(), 1));
			Assert.AreEqual(true, NotEquals(1, new object()));
		}

		#endregion

		#region Float Not Equals

		[Test]
		public void FloatNotEqualsFloat()
		{
			Assert.AreEqual(false, NotEquals(0f, 0f));

			Assert.AreEqual(true, NotEquals(0f, 1f));
			Assert.AreEqual(true, NotEquals(1f, 0f));

			Assert.AreEqual(true, NotEquals(1f, 0f));
			Assert.AreEqual(true, NotEquals(0f, 1f));

			Assert.AreEqual(false, NotEquals(1f, 1f));
		}

		[Test]
		public void FloatNotEqualsString()
		{
			Assert.AreEqual(true, NotEquals("", 0f));
			Assert.AreEqual(true, NotEquals(0f, ""));

			Assert.AreEqual(true, NotEquals("", 1f));
			Assert.AreEqual(true, NotEquals(1f, ""));

			Assert.AreEqual(true, NotEquals("a", 0f));
			Assert.AreEqual(true, NotEquals(0f, "a"));

			Assert.AreEqual(true, NotEquals("a", 1f));
			Assert.AreEqual(true, NotEquals(1f, "a"));

			Assert.AreEqual(true, NotEquals("1", 1f));
			Assert.AreEqual(true, NotEquals(1f, "1"));
		}

		[Test]
		public void FloatNotEqualsObject()
		{
			Assert.AreEqual(true, NotEquals(null, 0f));
			Assert.AreEqual(true, NotEquals(0f, null));

			Assert.AreEqual(true, NotEquals(null, 1f));
			Assert.AreEqual(true, NotEquals(1f, null));

			Assert.AreEqual(true, NotEquals(new object(), 0f));
			Assert.AreEqual(true, NotEquals(0f, new object()));

			Assert.AreEqual(true, NotEquals(new object(), 1f));
			Assert.AreEqual(true, NotEquals(1f, new object()));
		}

		#endregion

		#region String Not Equals

		[Test]
		public void StringNotEqualsString()
		{
			Assert.AreEqual(false, NotEquals("", ""));

			Assert.AreEqual(true, NotEquals("", "b"));
			Assert.AreEqual(true, NotEquals("b", ""));

			Assert.AreEqual(true, NotEquals("a", ""));
			Assert.AreEqual(true, NotEquals("", "a"));

			Assert.AreEqual(true, NotEquals("a", "b"));
			Assert.AreEqual(true, NotEquals("b", "a"));

			Assert.AreEqual(false, NotEquals("a", "a"));
		}

		[Test]
		public void StringNotEqualsObject()
		{
			Assert.AreEqual(true, NotEquals(null, ""));
			Assert.AreEqual(true, NotEquals("", null));

			Assert.AreEqual(true, NotEquals(null, "a"));
			Assert.AreEqual(true, NotEquals("a", null));

			Assert.AreEqual(true, NotEquals(new object(), ""));
			Assert.AreEqual(true, NotEquals("", new object()));

			Assert.AreEqual(true, NotEquals(new object(), "a"));
			Assert.AreEqual(true, NotEquals("a", new object()));
		}

		#endregion

		#region Object Not Equals

		[Test]
		public void ObjectNotEqualsObject()
		{
			Assert.AreEqual(false, NotEquals(null, null));

			Assert.AreEqual(true, NotEquals(null, new object()));
			Assert.AreEqual(true, NotEquals(new object(), null));

			Assert.AreEqual(true, NotEquals(new object(), new object()));

			var obj = new object();
			Assert.AreEqual(false, NotEquals(obj, obj));
		}

		#endregion
	}
}

#endif
