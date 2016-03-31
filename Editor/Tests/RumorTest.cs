using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test
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
			var rumor = new Engine.Rumor(new List<Node>());

			var iter = rumor.Run();
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
			var rumor = new Engine.Rumor(new List<Node>() {
				new Say("a"),
				new Say("b"),
			});

			var iter = rumor.Run();

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
			var rumor = new Engine.Rumor(new List<Node>());

			Assert.DoesNotThrow(rumor.Advance);
			Assert.DoesNotThrow(() => rumor.Update(0));
		}

		/// <summary>
		/// Ensure Rumor objects throw an exception when run is invoked before
		/// the previous run has completed.
		/// </summary>
		[Test]
		public void RumorDisallowMultipleRun()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Say("a")
			});

			rumor.Run().MoveNext();
			Assert.Throws<InvalidOperationException>(
				() => rumor.Run().MoveNext()
			);
		}
	}
}
