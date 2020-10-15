﻿using NUnit.Framework;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Simple
	{
		#region Char

		[Test]
		public static void CharParserSuccess()
		{
			var state = new State("h", 4, 0);

			var result = Parse.Char('h')(ref state);
			Assert.AreEqual('h', result);
		}

		[Test]
		public static void CharParserIndexSuccess()
		{
			var state = new State("world", 4, 3);

			var result = Parse.Char('l')(ref state);
			Assert.AreEqual('l', result);
		}

		[Test]
		public static void CharParserFail()
		{
			var state = new State("h", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('H')(ref state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "H" }, exception.Expected);
		}

		[Test]
		public static void CharParserIndexFail()
		{
			var state = new State("world", 4, 3);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Char('L')(ref state)
			);
			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(new string[] { "L" }, exception.Expected);
		}

		#endregion

		#region Indented

		[Test]
		public static void ZeroSameOrIndentedSuccess()
		{
			var state = new State("hello world!", 4, 0);

			var result = Parse.SameOrIndented(ref state);
			Assert.AreEqual(1, result);
		}

		[Test]
		public static void SameOrIndentedSuccess()
		{
			var state = new State("    hello world!", 4, 4);

			var result = Parse.SameOrIndented(ref state);
			Assert.AreEqual(5, result);
		}

		[Test]
		public static void SameOrIndentedTabSuccess()
		{
			var state = new State("  \thello world!", 4, 3);

			var result = Parse.SameOrIndented(ref state);
			Assert.AreEqual(4, result);
		}

		[Test]
		public static void SameOrIndentedLineSuccess()
		{
			var state = new State("\n  \thello world!", 4, 4);

			var result = Parse.SameOrIndented(ref state);
			Assert.AreEqual(4, result);
		}

		[Test]
		public static void SameOrIndentedFailure()
		{
			var state = new State("    hello world!", 4, 4)
				.SetIndent()
				.AddIndex(-2);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.SameOrIndented(ref state)
			);
			Assert.AreEqual(2, exception.Index);
			Assert.AreEqual(
				new string[] { "line indented to column 5 or more" },
				exception.Expected
			);
		}

		[Test]
		public static void SameOrIndentedLineFailure()
		{
			var state = new State("\n    hello world!", 4, 5)
				.SetIndent()
				.AddIndex(-2);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.SameOrIndented(ref state)
			);
			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(
				new string[] { "line indented to column 5 or more" },
				exception.Expected
			);
		}

		[Test]
		public static void BlockSuccess()
		{
			var state = new State("  a\n  a\n a", 4, 2);
			state = state.SetIndent();

			var result = Parse.Block(Parse.Char('a'), Parse.SameOrIndented)
				.String()(ref state);
			Assert.AreEqual("aa", result);
		}

		[Test]
		public static void BlockFailure()
		{
			var state = new State("  ab\n  a\n a", 4, 2);
			state = state.SetIndent();

			var exception = Assert.Throws<ParserException>(() =>
				Parse.Block(Parse.Char('a'), Parse.SameOrIndented)
					.String()(ref state)
			);

			Assert.AreEqual(3, exception.Index);
			Assert.AreEqual(
				new string[] { "space" },
				exception.Expected
			);
		}

		#endregion

		#region String

		[Test]
		public static void StringParserSuccess()
		{
			var state = new State("hello world!", 4, 0);

			var result = Parse.String("hello world!")(ref state);
			Assert.AreEqual("hello world!", result);
		}

		[Test]
		public static void StringParserIndexSuccess()
		{
			var state = new State("hello world!", 4, 6);

			var result = Parse.String("world!")(ref state);
			Assert.AreEqual("world!", result);
		}

		[Test]
		public static void StringParserFail()
		{
			var state = new State("hello world!", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("HELLO WORLD!")(ref state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "HELLO WORLD!" }, exception.Expected);
		}

		[Test]
		public static void StringParserLengthFail()
		{
			var state = new State("hello world!", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("HELLO WORLD!!")(ref state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "HELLO WORLD!!" }, exception.Expected);
		}

		[Test]
		public static void StringParserIndexFail()
		{
			var state = new State("hello world!", 4, 6);

			var exception = Assert.Throws<ParserException>(() =>
				Parse.String("WORLD!")(ref state)
			);
			Assert.AreEqual(6, exception.Index);
			Assert.AreEqual(new string[] { "WORLD!" }, exception.Expected);
		}

		[Test]
		public static void StringParserMultiSuccess()
		{
			var state = new State("bar", 4, 0);

			var result = Parse.String("foo", "bar")(ref state);
			Assert.AreEqual("bar", result);
		}

		#endregion
	}
}
