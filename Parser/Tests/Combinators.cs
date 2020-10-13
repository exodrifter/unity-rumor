using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Combinators
	{
		#region Fn

		[Test]
		public static void FnSuccess()
		{
			var state = new State("aaa", 4, 0);

			var result = Parse.Char('a').Many(0)
				.Select(chs => new string(chs.ToArray()))
				(state);
			Assert.AreEqual("aaa", result.Value);
		}

		#endregion

		#region Many

		[Test]
		public static void ManySuccess()
		{
			var state = new State("aaa", 4, 0);

			var result = Parse.Char('a').Many(0)(state);
			Assert.AreEqual(new char[] { 'a', 'a', 'a' }, result.Value);
		}

		[Test]
		public static void Many0Success()
		{
			var state = new State("a", 4, 0);

			var result = Parse.Char('z').Many(0)(state);
			Assert.AreEqual(new char[] { }, result.Value);
		}

		[Test]
		public static void Many1Success()
		{
			var state = new State("a", 4, 0);

			var result = Parse.Char('a').Many(1)(state);
			Assert.AreEqual(new char[] { 'a' }, result.Value);
		}

		[Test]
		public static void ManyFailure()
		{
			var state = new State("a", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('z').Many(1)(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(
				new string[] { "at least 1 more of z" },
				exception.Expected
			);
		}

		#endregion

		#region Or

		[Test]
		public static void OrLeftSuccess()
		{
			var state = new State("a", 4, 0);

			var result = Parse.Char('a').Or(Parse.Char('z'))(state);
			Assert.AreEqual('a', result.Value);
		}

		[Test]
		public static void OrRightSuccess()
		{
			var state = new State("z", 4, 0);

			var result = Parse.Char('a').Or(Parse.Char('z'))(state);
			Assert.AreEqual('z', result.Value);
		}

		[Test]
		public static void OrFailure()
		{
			var state = new State("m", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('a').Or(Parse.Char('z'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "a", "z" }, exception.Expected);
		}

		#endregion

		#region Then

		[Test]
		public static void ThenSuccess()
		{
			var state = new State("ab", 4, 0);

			var result = Parse.Char('a').Then(Parse.Char('b'))(state);
			Assert.AreEqual('b', result.Value);
		}

		[Test]
		public static void ThenLeftFailure()
		{
			var state = new State("ab", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('z').Then(Parse.Char('b'))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "z" }, exception.Expected);
		}

		[Test]
		public static void ThenRightFailure()
		{
			var state = new State("ab", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('a').Then(Parse.Char('z'))(state)
			);
			Assert.AreEqual(1, exception.Index);
			Assert.AreEqual(new string[] { "z" }, exception.Expected);
		}

		#endregion
	}
}
