#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the and operators work as expected.
	/// </summary>
	public class BoolAndTest
	{
		#region Helpers

		private object And(object l, object r)
		{
			var scope = new Scope();
			scope.SetVar("l", l);
			scope.SetVar("r", r);

			var exp = new BoolAndExpression(
				new VariableExpression("l"),
				new VariableExpression("r")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		#region Bool And

		[Test]
		public void BoolAndBool()
		{
			Assert.AreEqual(false, And(false, false));

			Assert.AreEqual(false, And(false, true));
			Assert.AreEqual(false, And(true, false));

			Assert.AreEqual(true, And(true, true));
		}

		[Test]
		public void BoolAndInt()
		{
			Assert.AreEqual(false, And(0, false));
			Assert.AreEqual(false, And(false, 0));

			Assert.AreEqual(false, And(0, true));
			Assert.AreEqual(false, And(true, 0));

			Assert.AreEqual(false, And(1, false));
			Assert.AreEqual(false, And(false, 1));

			Assert.AreEqual(true, And(1, true));
			Assert.AreEqual(true, And(true, 1));
		}

		[Test]
		public void BoolAndFloat()
		{
			Assert.AreEqual(false, And(0f, false));
			Assert.AreEqual(false, And(false, 0f));

			Assert.AreEqual(false, And(0f, true));
			Assert.AreEqual(false, And(true, 0f));

			Assert.AreEqual(false, And(1f, false));
			Assert.AreEqual(false, And(false, 1f));

			Assert.AreEqual(true, And(1f, true));
			Assert.AreEqual(true, And(true, 1f));
		}

		[Test]
		public void BoolAndString()
		{
			Assert.AreEqual(false, And("", false));
			Assert.AreEqual(false, And(false, ""));

			Assert.AreEqual(false, And("", true));
			Assert.AreEqual(false, And(true, ""));

			Assert.AreEqual(false, And("a", false));
			Assert.AreEqual(false, And(false, "a"));

			Assert.AreEqual(true, And("a", true));
			Assert.AreEqual(true, And(true, "a"));
		}

		[Test]
		public void BoolAndObject()
		{
			Assert.AreEqual(false, And(null, false));
			Assert.AreEqual(false, And(false, null));

			Assert.AreEqual(false, And(null, true));
			Assert.AreEqual(false, And(true, null));

			Assert.AreEqual(false, And(new object(), false));
			Assert.AreEqual(false, And(false, new object()));

			Assert.AreEqual(true, And(new object(), true));
			Assert.AreEqual(true, And(true, new object()));
		}

		#endregion

		#region Int And

		[Test]
		public void IntAndInt()
		{
			Assert.AreEqual(false, And(0, 0));
			Assert.AreEqual(false, And(0, 0));

			Assert.AreEqual(false, And(0, 1));
			Assert.AreEqual(false, And(1, 0));

			Assert.AreEqual(false, And(1, 0));
			Assert.AreEqual(false, And(0, 1));

			Assert.AreEqual(true, And(1, 1));
			Assert.AreEqual(true, And(1, 1));
		}

		[Test]
		public void IntAndFloat()
		{
			Assert.AreEqual(false, And(0f, 0));
			Assert.AreEqual(false, And(0, 0f));

			Assert.AreEqual(false, And(0f, 1));
			Assert.AreEqual(false, And(1, 0f));

			Assert.AreEqual(false, And(1f, 0));
			Assert.AreEqual(false, And(0, 1f));

			Assert.AreEqual(true, And(1f, 1));
			Assert.AreEqual(true, And(1, 1f));
		}

		[Test]
		public void IntAndString()
		{
			Assert.AreEqual(false, And("", 0));
			Assert.AreEqual(false, And(0, ""));

			Assert.AreEqual(false, And("", 1));
			Assert.AreEqual(false, And(1, ""));

			Assert.AreEqual(false, And("a", 0));
			Assert.AreEqual(false, And(0, "a"));

			Assert.AreEqual(true, And("a", 1));
			Assert.AreEqual(true, And(1, "a"));
		}

		[Test]
		public void IntAndObject()
		{
			Assert.AreEqual(false, And(null, 0));
			Assert.AreEqual(false, And(0, null));

			Assert.AreEqual(false, And(null, 1));
			Assert.AreEqual(false, And(1, null));

			Assert.AreEqual(false, And(new object(), 0));
			Assert.AreEqual(false, And(0, new object()));

			Assert.AreEqual(true, And(new object(), 1));
			Assert.AreEqual(true, And(1, new object()));
		}

		#endregion

		#region Float And

		[Test]
		public void FloatAndFloat()
		{
			Assert.AreEqual(false, And(0f, 0f));
			Assert.AreEqual(false, And(0f, 0f));

			Assert.AreEqual(false, And(0f, 1f));
			Assert.AreEqual(false, And(1f, 0f));

			Assert.AreEqual(false, And(1f, 0f));
			Assert.AreEqual(false, And(0f, 1f));

			Assert.AreEqual(true, And(1f, 1f));
			Assert.AreEqual(true, And(1f, 1f));
		}

		[Test]
		public void FloatAndString()
		{
			Assert.AreEqual(false, And("", 0f));
			Assert.AreEqual(false, And(0f, ""));

			Assert.AreEqual(false, And("", 1f));
			Assert.AreEqual(false, And(1f, ""));

			Assert.AreEqual(false, And("a", 0f));
			Assert.AreEqual(false, And(0f, "a"));

			Assert.AreEqual(true, And("a", 1f));
			Assert.AreEqual(true, And(1f, "a"));
		}

		[Test]
		public void FloatAndObject()
		{
			Assert.AreEqual(false, And(null, 0f));
			Assert.AreEqual(false, And(0f, null));

			Assert.AreEqual(false, And(null, 1f));
			Assert.AreEqual(false, And(1f, null));

			Assert.AreEqual(false, And(new object(), 0f));
			Assert.AreEqual(false, And(0f, new object()));

			Assert.AreEqual(true, And(new object(), 1f));
			Assert.AreEqual(true, And(1f, new object()));
		}

		#endregion

		#region String And

		[Test]
		public void StringAndString()
		{
			Assert.AreEqual(false, And("", ""));
			Assert.AreEqual(false, And("", ""));

			Assert.AreEqual(false, And("", "b"));
			Assert.AreEqual(false, And("b", ""));

			Assert.AreEqual(false, And("a", ""));
			Assert.AreEqual(false, And("", "a"));

			Assert.AreEqual(true, And("a", "b"));
			Assert.AreEqual(true, And("b", "a"));
		}

		[Test]
		public void StringAndObject()
		{
			Assert.AreEqual(false, And(null, ""));
			Assert.AreEqual(false, And("", null));

			Assert.AreEqual(false, And(null, "a"));
			Assert.AreEqual(false, And("a", null));

			Assert.AreEqual(false, And(new object(), ""));
			Assert.AreEqual(false, And("", new object()));

			Assert.AreEqual(true, And(new object(), "a"));
			Assert.AreEqual(true, And("a", new object()));
		}

		#endregion

		#region Object And
		
		[Test]
		public void ObjectAndObject()
		{
			Assert.AreEqual(false, And(null, null));

			Assert.AreEqual(false, And(null, new object()));
			Assert.AreEqual(false, And(new object(), null));

			Assert.AreEqual(true, And(new object(), new object()));
		}

		#endregion
	}
}

#endif
