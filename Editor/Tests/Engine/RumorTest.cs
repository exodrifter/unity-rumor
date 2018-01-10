#if UNITY_EDITOR

using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Engine
{
	/// <summary>
	/// Ensure Rumor objects operate as expected.
	/// </summary>
	public class RumorTest
	{
		/// <summary>
		/// Ensure empty Rumor objects operate without exceptions.
		/// </summary>
		[Test]
		public void EmptyRumor()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>());

			var iter = rumor.Start();
			iter.MoveNext();

			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);

			Assert.DoesNotThrow(() => rumor.Update(0));
			Assert.DoesNotThrow(rumor.Advance);
		}

		/// <summary>
		/// Ensure a simple Rumor object can operate without exceptions.
		/// </summary>
		[Test]
		public void SimpleRumor()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Say("a"),
				new Say("b"),
			});

			var iter = rumor.Start();

			Assert.False(rumor.Started);
			Assert.False(rumor.Finished);

			iter.MoveNext();

			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);

			Assert.DoesNotThrow(() => rumor.Update(0));
			Assert.DoesNotThrow(rumor.Advance);
			iter.MoveNext();

			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);

			Assert.DoesNotThrow(() => rumor.Update(0));
			Assert.DoesNotThrow(rumor.Advance);
			iter.MoveNext();

			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);
		}

		/// <summary>
		/// Ensure Rumor objects do not throw exceptions when events are
		/// passed to it while it is not running.
		/// </summary>
		[Test]
		public void RumorEventWithoutStart()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>());

			Assert.DoesNotThrow(rumor.Advance);
			Assert.DoesNotThrow(() => rumor.Update(0));
		}

		/// <summary>
		/// Ensure Rumor jump methods can be called if it has not yet been
		/// started.
		/// </summary>
		[Test]
		public void RumorJumpWithoutStart()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Say("a"),
				new Label("test", new List<Node> (){
					new Say("b"),
				}),
			});
			Assert.DoesNotThrow(() => rumor.JumpToLabel("test"));
			Assert.IsFalse(rumor.Started);
			Assert.IsFalse(rumor.Running);
			Assert.IsFalse(rumor.Finished);

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("b", (rumor.Current as Say).EvaluateText(rumor));
			Assert.IsTrue(rumor.Started);
			Assert.IsTrue(rumor.Running);
			Assert.IsFalse(rumor.Finished);
		}

		/// <summary>
		/// Ensure Rumor call methods can be called if it has not yet been
		/// started.
		/// </summary>
		[Test]
		public void RumorCallWithoutStart()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Say("a"),
				new Label("test", new List<Node> (){
					new Say("b"),
				}),
			});
			Assert.DoesNotThrow(() => rumor.CallLabel("test"));
			Assert.IsFalse(rumor.Started);
			Assert.IsFalse(rumor.Running);
			Assert.IsFalse(rumor.Finished);

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("b", (rumor.Current as Say).EvaluateText(rumor));
			Assert.IsTrue(rumor.Started);
			Assert.IsTrue(rumor.Running);
			Assert.IsFalse(rumor.Finished);
		}
	}
}

#endif
