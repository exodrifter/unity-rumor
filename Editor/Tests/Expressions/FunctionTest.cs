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
			var scope = new Rumor.Engine.Scope();

			bool a0 = false;
			scope.Bind("a0", () => a0 = true);
			scope.CallBinding("a0");
			Assert.IsTrue(a0);

			int a1 = 0;
			scope.Bind("a1", (int p1) => a1 = p1);
			scope.CallBinding("a1", 1);
			Assert.AreEqual(a1, 1);

			int a2 = 0;
			scope.Bind("a2", (int p1, int p2) => { a1 = p1; a2 = p2; });
			scope.CallBinding("a2", 2, 3);
			Assert.AreEqual(a1, 2);
			Assert.AreEqual(a2, 3);

			int a3 = 0;
			scope.Bind("a3", (int p1, int p2, int p3) => {
				a1 = p1; a2 = p2; a3 = p3;
			});
			scope.CallBinding("a3", 3, 4, 5);
			Assert.AreEqual(a1, 3);
			Assert.AreEqual(a2, 4);
			Assert.AreEqual(a3, 5);

			int a4 = 0;
			scope.Bind("a4", (int p1, int p2, int p3, int p4) => {
				a1 = p1; a2 = p2; a3 = p3; a4 = p4;
			});
			scope.CallBinding("a4", 4, 5, 6, 7);
			Assert.AreEqual(a1, 4);
			Assert.AreEqual(a2, 5);
			Assert.AreEqual(a3, 6);
			Assert.AreEqual(a4, 7);
		}

		/// <summary>
		/// Check if <see cref="Func"/> bindings work.
		/// </summary>
		[Test]
		public void Func()
		{
			var scope = new Rumor.Engine.Scope();

			scope.Bind("a0", () => { return true; });
			var a0 = scope.CallBinding("a0");
			Assert.IsTrue((bool)a0);

			scope.Bind("a1", (int p1) => { return p1; });
			var a1 = scope.CallBinding("a1", 1);
			Assert.AreEqual(a1, 1);

			scope.Bind("a2", (int p1, int p2) => { return p1 + p2; });
			var a2 = scope.CallBinding("a2", 1, 2);
			Assert.AreEqual(a2, 3);

			scope.Bind("a3", (int p1, int p2, int p3) => {
				return p1 + p2 + p3;
			});
			var a3 = scope.CallBinding("a3", 1, 2, 3);
			Assert.AreEqual(a3, 6);

			scope.Bind("a4", (int p1, int p2, int p3, int p4) => {
				return p1 + p2 + p3 + p4;
			});
			var a4 = scope.CallBinding("a4", 1, 2, 3, 5);
			Assert.AreEqual(a4, 11);
		}

		[Test]
		public void CallUnmappedBinding()
		{
			var scope = new Rumor.Engine.Scope();
			scope.Bind("foo", () => { return true; });

			Assert.Throws<InvalidOperationException>(
				() => scope.CallBinding("bar")
			);
		}
	}
}
