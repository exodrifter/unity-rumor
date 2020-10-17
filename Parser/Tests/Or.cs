using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Or
	{
		/// <summary>
		/// Test what happens when the left parser succeeds.
		/// </summary>
		[Test]
		public static void OrLeftSuccess()
		{
			var state = new State("a", 4, 0);

			var result = Parse.Char('a').Or(Parse.Char('z'))(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the right parser succeeds.
		/// </summary>
		[Test]
		public static void OrRightSuccess()
		{
			var state = new State("z", 4, 0);

			var result = Parse.Char('a').Or(Parse.Char('z'))(state);
			Assert.AreEqual('z', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is empty.
		/// </summary>
		[Test]
		public static void OrEmptyFailure()
		{
			var state = new State("", 4, 0);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Char('a').Or(Parse.Char('z'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "a", "z" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when neither parser will succeed.
		/// </summary>
		[Test]
		public static void OrFailure()
		{
			var state = new State("m", 4, 0);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Char('a').Or(Parse.Char('z'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "a", "z" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}