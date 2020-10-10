using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Simple
	{
		#region Char

		[Test]
		public static void CharParserSuccess()
		{
			var state = new State("h", 0);

			var result = new CharParser('h').Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual('h', result.Value);
		}

		[Test]
		public static void CharParserIndexSuccess()
		{
			var state = new State("world", 3);

			var result = new CharParser('l').Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual('l', result.Value);
		}

		[Test]
		public static void CharParserFail()
		{
			var state = new State("h", 0);

			var result = new CharParser('H').Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.ErrorIndex);
			Assert.AreEqual(new string[] { "H" }, result.Expected);
		}

		[Test]
		public static void CharParserIndexFail()
		{
			var state = new State("world", 3);

			var result = new CharParser('L').Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(3, result.ErrorIndex);
			Assert.AreEqual(new string[] { "L" }, result.Expected);
		}

		#endregion

		#region String

		[Test]
		public static void StringParserSuccess()
		{
			var state = new State("hello world!", 0);

			var result = new StringParser("hello world!").Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("hello world!", result.Value);
		}

		[Test]
		public static void StringParserIndexSuccess()
		{
			var state = new State("hello world!", 6);

			var result = new StringParser("world!").Parse(state);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("world!", result.Value);
		}

		[Test]
		public static void StringParserFail()
		{
			var state = new State("hello world!", 0);

			var result = new StringParser("HELLO WORLD!").Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.ErrorIndex);
			Assert.AreEqual(new string[] { "HELLO WORLD!" }, result.Expected);
		}

		[Test]
		public static void StringParserIndexFail()
		{
			var state = new State("hello world!", 6);

			var result = new StringParser("WORLD!").Parse(state);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(6, result.ErrorIndex);
			Assert.AreEqual(new string[] { "WORLD!" }, result.Expected);
		}

		#endregion
	}
}
