using Exodrifter.Rumor.Engine;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test
{
	/// <summary>
	/// Makes sure that yield objects operate as expected.
	/// </summary>
	public class YieldTest
	{
		/// <summary>
		/// Makes sure yield for seconds operates as expected.
		/// </summary>
		[Test]
		public void YieldSeconds()
		{
			var yield = new ForSeconds(1);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.False(yield.Finished);

			yield.OnUpdate(0);
			Assert.False(yield.Finished);

			yield.OnUpdate(1);
			Assert.True(yield.Finished);

			yield.OnUpdate(1);
			Assert.True(yield.Finished);
		}

		/// <summary>
		/// Makes sure yield for advance operates as expected.
		/// </summary>
		[Test]
		public void YieldAdvance()
		{
			var yield = new ForAdvance();
			Assert.False(yield.Finished);

			yield.OnUpdate(0);
			Assert.False(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);

			yield.OnAdvance();
			Assert.True(yield.Finished);
		}
	}
}
