#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure the not operators work as expected.
	/// </summary>
	public class NotTest
	{
		#region Helpers

		private object Not(object x)
		{
			var scope = new Scope();
			scope.SetVar("x", x);

			var exp = new NotExpression(
				new VariableExpression("x")
			);

			return exp.Evaluate(scope, new Bindings()).AsObject();
		}

		#endregion

		[Test]
		public void NotBool()
		{
			Assert.AreEqual(true, Not(false));
			Assert.AreEqual(false, Not(true));
		}

		[Test]
		public void NotInt()
		{
			Assert.AreEqual(true, Not(0));
			Assert.AreEqual(false, Not(1));
		}

		[Test]
		public void NotFloat()
		{
			Assert.AreEqual(true, Not(0f));
			Assert.AreEqual(false, Not(1f));
		}

		[Test]
		public void NotString()
		{
			Assert.AreEqual(true, Not(""));
			Assert.AreEqual(false, Not("a"));
		}

		[Test]
		public void NotObject()
		{
			Assert.AreEqual(true, Not(null));
			Assert.AreEqual(false, Not(new object()));
		}
	}
}

#endif
