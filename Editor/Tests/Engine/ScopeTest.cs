#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Engine
{
	/// <summary>
	/// Ensure Scope objects operate as expected.
	/// </summary>
	public class ScopeTest
	{
		#region Events

		[Test]
		public void OnClearTest()
		{
			var success = false;
			var scope = new Scope();
			scope.OnSet += (name, value) => Assert.Fail();
			scope.OnClear += () =>
			{
				success = true;
			};

			scope.ClearVars();
			Assert.IsTrue(success);
		}

		[Test]
		public void OnSetTest()
		{
			var scope = new Scope();
			scope.OnClear += () => Assert.Fail();
			scope.OnSet += (name, value) =>
			{
				Assert.AreEqual("foo", name);
				Assert.IsTrue(value.IsBool());
				Assert.IsTrue(value.AsBool());
			};

			scope.SetVar("foo", true);
		}

		#endregion

		#region Setters

		[Test]
		public void SetBoolTest()
		{
			var scope = new Scope();

			scope.SetVar("foo", true);
			Assert.IsTrue(scope.GetVar("foo").IsBool());
			Assert.IsTrue(scope.GetVar("foo").AsBool());

			scope.SetVar("foo", false);
			Assert.IsTrue(scope.GetVar("foo").IsBool());
			Assert.IsFalse(scope.GetVar("foo").AsBool());
		}

		[Test]
		public void SetIntTest()
		{
			var scope = new Scope();

			scope.SetVar("foo", 1);
			Assert.IsTrue(scope.GetVar("foo").IsInt());
			Assert.AreEqual(1, scope.GetVar("foo").AsInt());

			scope.SetVar("foo", -100);
			Assert.IsTrue(scope.GetVar("foo").IsInt());
			Assert.AreEqual(-100, scope.GetVar("foo").AsInt());
		}

		[Test]
		public void SetFloatTest()
		{
			var scope = new Scope();

			scope.SetVar("foo", 1.0f);
			Assert.IsTrue(scope.GetVar("foo").IsFloat());
			Assert.AreEqual(1.0f, scope.GetVar("foo").AsFloat());

			scope.SetVar("foo", -100.0f);
			Assert.IsTrue(scope.GetVar("foo").IsFloat());
			Assert.AreEqual(-100.0f, scope.GetVar("foo").AsFloat());
		}

		[Test]
		public void SetStringTest()
		{
			var scope = new Scope();

			scope.SetVar("foo", "bar");
			Assert.IsTrue(scope.GetVar("foo").IsString());
			Assert.AreEqual("bar", scope.GetVar("foo").AsString());
		}

		[Test]
		public void SetObjectTest()
		{
			var scope = new Scope();

			var obj = new object();
			scope.SetVar("foo", obj);
			Assert.IsTrue(scope.GetVar("foo").IsObject());
			Assert.AreEqual(obj, scope.GetVar("foo").AsObject());

			scope.SetVar("foo", (object)true);
			Assert.IsTrue(scope.GetVar("foo").IsBool());
			Assert.IsTrue(scope.GetVar("foo").AsBool());

			scope.SetVar("foo", (object)1);
			Assert.IsTrue(scope.GetVar("foo").IsInt());
			Assert.AreEqual(1, scope.GetVar("foo").AsInt());

			scope.SetVar("foo", (object)1.0f);
			Assert.IsTrue(scope.GetVar("foo").IsFloat());
			Assert.AreEqual(1.0f, scope.GetVar("foo").AsFloat());

			scope.SetVar("foo", (object)"bar");
			Assert.IsTrue(scope.GetVar("foo").IsString());
			Assert.AreEqual("bar", scope.GetVar("foo").AsString());
		}

		#endregion
	}
}

#endif
