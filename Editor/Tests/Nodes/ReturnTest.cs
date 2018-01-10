#if UNITY_EDITOR

using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Nodes
{
	/// <summary>
	/// Ensure Call nodes operate as expected.
	/// </summary>
	public class ReturnTest
	{
		/// <summary>
		/// Ensure a single return call works as expected.
		/// </summary>
		[Test]
		public void Return()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Return()
			});

			rumor.Start().MoveNext();
			Assert.IsTrue(rumor.Finished);
		}

		/// <summary>
		/// Ensure a return inside a jump works as expected.
		/// </summary>
		[Test]
		public void ReturnInJump()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Jump("start"),
				new Label("foo", new List<Node>() {
					new Say("Something to prevent infinite loop"),
					new Return()
				}),
				new Label("start", new List<Node>() {
					new Jump("foo")
				}),
			});
			
			var yield = rumor.Start();
			yield.MoveNext();
			rumor.Advance();
			yield.MoveNext();
			Assert.IsFalse(rumor.Finished);
		}

		/// <summary>
		/// Ensure a return inside a call works as expected.
		/// </summary>
		[Test]
		public void ReturnInCall()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Jump("start"),
				new Label("foo", new List<Node>() {
					new Say("Something to prevent infinite loop"),
					new Return()
				}),
				new Label("start", new List<Node>() {
					new Call("foo")
				}),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			rumor.Advance();
			yield.MoveNext();
			Assert.IsTrue(rumor.Finished);
		}
	}
}

#endif
