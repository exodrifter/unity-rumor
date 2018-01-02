#if UNITY_EDITOR

using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure function calls work as expected
	/// </summary>
	public class FunctionTest
	{
		/// <summary>
		/// Check if <see cref="Action"/> bindings work.
		/// </summary>
		[Test]
		public void Action()
		{
			var rumor = new Rumor.Engine.Rumor("return");

			bool a0 = false;
			rumor.Bind("a0", () => a0 = true);
			rumor.CallBinding("a0");
			Assert.IsTrue(a0);

			int a1 = 0;
			rumor.Bind("a1", (int p1) => a1 = p1);
			rumor.CallBinding("a1", 1);
			Assert.AreEqual(1, a1);

			int a2 = 0;
			rumor.Bind("a2", (int p1, int p2) => { a1 = p1; a2 = p2; });
			rumor.CallBinding("a2", 2, 3);
			Assert.AreEqual(2, a1);
			Assert.AreEqual(3, a2);

			int a3 = 0;
			rumor.Bind("a3", (int p1, int p2, int p3) => {
				a1 = p1; a2 = p2; a3 = p3;
			});
			rumor.CallBinding("a3", 3, 4, 5);
			Assert.AreEqual(3, a1);
			Assert.AreEqual(4, a2);
			Assert.AreEqual(5, a3);

			int a4 = 0;
			rumor.Bind("a4", (int p1, int p2, int p3, int p4) => {
				a1 = p1; a2 = p2; a3 = p3; a4 = p4;
			});
			rumor.CallBinding("a4", 4, 5, 6, 7);
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
			var rumor = new Rumor.Engine.Rumor("return");

			rumor.Bind("a0", () => { return true; });
			var a0 = rumor.CallBinding("a0");
			Assert.IsTrue((bool)a0);

			rumor.Bind("a1", (int p1) => { return p1; });
			var a1 = rumor.CallBinding("a1", 1);
			Assert.AreEqual(1, a1);

			rumor.Bind("a2", (int p1, int p2) => { return p1 + p2; });
			var a2 = rumor.CallBinding("a2", 1, 2);
			Assert.AreEqual(3, a2);

			rumor.Bind("a3", (int p1, int p2, int p3) => {
				return p1 + p2 + p3;
			});
			var a3 = rumor.CallBinding("a3", 1, 2, 3);
			Assert.AreEqual(6, a3);

			rumor.Bind("a4", (int p1, int p2, int p3, int p4) => {
				return p1 + p2 + p3 + p4;
			});
			var a4 = rumor.CallBinding("a4", 1, 2, 3, 5);
			Assert.AreEqual(11, a4);
		}

		[Test]
		public void CallUnmappedBinding()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Bind("foo", () => { return true; });

			Assert.Throws<InvalidOperationException>(
				() => rumor.CallBinding("bar")
			);
		}
	}
}

#endif
