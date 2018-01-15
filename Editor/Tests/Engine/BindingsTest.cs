#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Engine
{
	/// <summary>
	/// Ensure function calls work as expected
	/// </summary>
	public class BindingsTest
	{
		/// <summary>
		/// Check if <see cref="Action"/> bindings work.
		/// </summary>
		[Test]
		public void Action()
		{
			var bindings = new Bindings();

			bool a0 = false;
			bindings.Bind("a0", () => a0 = true);
			bindings.CallBinding("a0");
			Assert.IsTrue(a0);

			int a1 = 0;
			bindings.Bind("a1", (int p1) => a1 = p1);
			bindings.CallBinding("a1", 1);
			Assert.AreEqual(1, a1);

			int a2 = 0;
			bindings.Bind("a2", (int p1, int p2) => { a1 = p1; a2 = p2; });
			bindings.CallBinding("a2", 2, 3);
			Assert.AreEqual(2, a1);
			Assert.AreEqual(3, a2);

			int a3 = 0;
			bindings.Bind("a3", (int p1, int p2, int p3) => {
				a1 = p1; a2 = p2; a3 = p3;
			});
			bindings.CallBinding("a3", 3, 4, 5);
			Assert.AreEqual(3, a1);
			Assert.AreEqual(4, a2);
			Assert.AreEqual(5, a3);

			int a4 = 0;
			bindings.Bind("a4", (int p1, int p2, int p3, int p4) => {
				a1 = p1; a2 = p2; a3 = p3; a4 = p4;
			});
			bindings.CallBinding("a4", 4, 5, 6, 7);
			Assert.AreEqual(4, a1);
			Assert.AreEqual(5, a2);
			Assert.AreEqual(6, a3);
			Assert.AreEqual(7, a4);
		}

		/// <summary>
		/// Check if <see cref="Func"/> bindings work.
		/// </summary>
		[Test]
		public void Func()
		{
			var bindings = new Bindings();

			bindings.Bind("a0", () => { return true; });
			var a0 = bindings.CallBinding("a0");
			Assert.IsTrue((bool)a0);

			bindings.Bind("a1", (int p1) => { return p1; });
			var a1 = bindings.CallBinding("a1", 1);
			Assert.AreEqual(1, a1);

			bindings.Bind("a2", (int p1, int p2) => { return p1 + p2; });
			var a2 = bindings.CallBinding("a2", 1, 2);
			Assert.AreEqual(3, a2);

			bindings.Bind("a3", (int p1, int p2, int p3) => {
				return p1 + p2 + p3;
			});
			var a3 = bindings.CallBinding("a3", 1, 2, 3);
			Assert.AreEqual(6, a3);

			bindings.Bind("a4", (int p1, int p2, int p3, int p4) => {
				return p1 + p2 + p3 + p4;
			});
			var a4 = bindings.CallBinding("a4", 1, 2, 3, 5);
			Assert.AreEqual(11, a4);
		}

		[Test]
		public void CallUnmappedBinding()
		{
			var bindings = new Bindings();
			bindings.Bind("foo", () => { return true; });

			Assert.Throws<InvalidOperationException>(
				() => bindings.CallBinding("bar")
			);
		}

		[Test]
		public void BindAgain()
		{
			var bindings = new Bindings();
			bindings.Bind("foo", () => { return true; });

			Assert.Throws<InvalidOperationException>(
				() => bindings.Bind("foo", () => { return false; })
			);
		}
	}
}

#endif
