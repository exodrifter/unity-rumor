using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Combinators
	{
		#region Many

		[Test]
		public static void ManySuccess()
		{
			var state = new State("aaa", 4, 0);

			var result = Parse.Char('a').Many(0)(ref state);
			Assert.AreEqual(new char[] { 'a', 'a', 'a' }, result);
		}

		[Test]
		public static void Many0Success()
		{
			var state = new State("a", 4, 0);

			var result = Parse.Char('z').Many(0)(ref state);
			Assert.AreEqual(new char[] { }, result);
		}

		[Test]
		public static void Many1Success()
		{
			var state = new State("a", 4, 0);

			var result = Parse.Char('a').Many(1)(ref state);
			Assert.AreEqual(new char[] { 'a' }, result);
		}

		[Test]
		public static void ManyFailure()
		{
			var state = new State("a", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('z').Many(1)(ref state)
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

			var result = Parse.Char('a').Or(Parse.Char('z'))(ref state);
			Assert.AreEqual('a', result);
		}

		[Test]
		public static void OrRightSuccess()
		{
			var state = new State("z", 4, 0);

			var result = Parse.Char('a').Or(Parse.Char('z'))(ref state);
			Assert.AreEqual('z', result);
		}

		[Test]
		public static void OrFailure()
		{
			var state = new State("m", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('a').Or(Parse.Char('z'))(ref state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "a", "z" }, exception.Expected);
		}

		#endregion

		#region Rollback

		/// <summary>
		/// These test if the parser rolls back successfully when trying other
		/// options.
		/// </summary>
		[Test]
		public static void RollbackSuccess()
		{
			var state = new State("az", 4, 0);

			var result = Parse.Char('a').Then(Parse.Char('b'))
				.Or(Parse.Char('a').Then(Parse.Char('z')))(ref state);
			Assert.AreEqual('z', result);
		}

		#endregion

		#region Then

		[Test]
		public static void ThenSuccess()
		{
			var state = new State("ab", 4, 0);

			var result = Parse.Char('a').Then(Parse.Char('b'))(ref state);
			Assert.AreEqual('b', result);
		}

		[Test]
		public static void ThenLeftFailure()
		{
			var state = new State("ab", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('z').Then(Parse.Char('b'))(ref state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "z" }, exception.Expected);
		}

		[Test]
		public static void ThenRightFailure()
		{
			var state = new State("ab", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('a').Then(Parse.Char('z'))(ref state)
			);
			Assert.AreEqual(1, exception.Index);
			Assert.AreEqual(new string[] { "z" }, exception.Expected);
		}

		#endregion
	}
}
