using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Combinators
	{
		#region Many

		[Test]
		public static void ManySuccess()
		{
			var state = new State("aaa");

			var result = new CharParser('a').Many(0).Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(new char[] { 'a', 'a', 'a' }, result.Value);
		}

		[Test]
		public static void Many0Success()
		{
			var state = new State("a");

			var result = new CharParser('z').Many(0).Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(new char[] { }, result.Value);
		}

		[Test]
		public static void Many1Success()
		{
			var state = new State("a");

			var result = new CharParser('a').Many(1).Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(new char[] { 'a' }, result.Value);
		}

		[Test]
		public static void ManyFailure()
		{
			var state = new State("a");

			var result = new CharParser('z').Many(1).Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.ErrorIndex);
			Assert.AreEqual(
				new string[] { "at least 1 more of z" },
				result.Expected
			);
		}

		#endregion

		#region Or

		[Test]
		public static void OrLeftSuccess()
		{
			var state = new State("a", 0);

			var result = new CharParser('a')
				.Or(new CharParser('z'))
				.Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual('a', result.Value);
		}

		[Test]
		public static void OrRightSuccess()
		{
			var state = new State("z", 0);

			var result = new CharParser('a')
				.Or(new CharParser('z'))
				.Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual('z', result.Value);
		}

		[Test]
		public static void OrFailure()
		{
			var state = new State("m", 0);

			var result = new CharParser('a')
				.Or(new CharParser('z'))
				.Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(new string[] { "a", "z" }, result.Expected);
		}

		#endregion

		#region Then

		[Test]
		public static void ThenSuccess()
		{
			var state = new State("ab", 0);

			var result = new CharParser('a')
				.Then(new CharParser('b'))
				.Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual('b', result.Value);
		}

		[Test]
		public static void ThenLeftFailure()
		{
			var state = new State("ab", 0);

			var result = new CharParser('z')
				.Then(new CharParser('b'))
				.Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.ErrorIndex);
			Assert.AreEqual(new string[] { "z" }, result.Expected);
		}

		[Test]
		public static void ThenRightFailure()
		{
			var state = new State("ab", 0);

			var result = new CharParser('a')
				.Then(new CharParser('z'))
				.Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(1, result.ErrorIndex);
			Assert.AreEqual(new string[] { "z" }, result.Expected);
		}

		#endregion
	}
}
