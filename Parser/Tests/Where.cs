using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Where
	{
		/// <summary>
		/// Test what happens when the parser will succeed.
		/// </summary>
		[Test]
		public static void WhereSuccess()
		{
			var state = new State("a", 4, 0);

			var result = Parse.AnyChar.Where(ch => ch == 'a', "a")(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the parser will fail.
		/// </summary>
		[Test]
		public static void WhereFailure()
		{
			var state = new State("b", 4, 0);

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.AnyChar.Where(ch => ch == 'a', "a")(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "a" }, exception.Expected);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}
