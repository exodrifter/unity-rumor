#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Engine
{
	/// <summary>
	/// Ensure yield objects operate as expected.
	/// </summary>
	public class YieldTest
	{
		/// <summary>
		/// Ensure yield for advance operates as expected.
		/// </summary>
		[Test]
		public void YieldAdvance()
		{
			var yield = new ForAdvance();
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 0);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);
		}

		/// <summary>
		/// Ensure yield for choice operates as expected.
		/// </summary>
		[Test]
		public void YieldChoice()
		{
			// Choose 1
			var yield = new ForChoice(1, 0, 0);
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 0);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.False(yield.Finished);

			yield.OnChoice();
			Assert.True(yield.Finished);

			yield.OnChoice();
			Assert.True(yield.Finished);

			yield.OnUpdate(null, 0);
			Assert.True(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);

			// Multiple
			yield = new ForChoice(2, 0, 0);
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 0);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.False(yield.Finished);

			yield.OnChoice();
			Assert.False(yield.Finished);

			yield.OnChoice();
			Assert.True(yield.Finished);

			yield.OnChoice();
			Assert.True(yield.Finished);

			yield.OnUpdate(null, 0);
			Assert.True(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);

			// Timed
			yield = new ForChoice(2, 2, 0);
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 1);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 1);
			Assert.True(yield.Finished);

			yield.OnUpdate(null, 1);
			Assert.True(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);

			yield.OnChoice();
			Assert.True(yield.Finished);
		}

		/// <summary>
		/// Ensure yield for seconds operates as expected.
		/// </summary>
		[Test]
		public void YieldSeconds()
		{
			var yield = new ForSeconds(1, true);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.False(yield.Finished);

			yield.OnChoice();
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 0);
			Assert.False(yield.Finished);

			yield.OnUpdate(null, 1);
			Assert.True(yield.Finished);

			yield.OnUpdate(null, 1);
			Assert.True(yield.Finished);

			yield = new ForSeconds(1, false);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);

			yield.OnUpdate(null, 0.5f);
			Assert.True(yield.Finished);

			yield.OnUpdate(null, 1f);
			Assert.True(yield.Finished);
		}
	}
}

#endif
