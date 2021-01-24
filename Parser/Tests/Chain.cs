using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Parser.Tests
{
	public static class Chain
	{
		/// <summary>
		/// Test the expected use case for this parser.
		/// </summary>
		[Test]
		public static void ChainL1Success()
		{
			var state = new ParserState("a,z,b", 4);
			Func<char, char, char> fn = (l, r) => {
				if (l > r)
				{
					return l;
				}
				else
				{
					return r;
				}
			};

			var result = Parse.Letter
				.ChainL1(Parse.Char(',').Then(fn))(state);
			Assert.AreEqual('z', result);

			Assert.AreEqual(5, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the input is empty
		/// </summary>
		[Test]
		public static void ChainL1EmptyFailure()
		{
			var state = new ParserState("", 4);
			Func<char, char, char> fn = (l, r) => {
				if (l > r)
				{
					return l;
				}
				else
				{
					return r;
				}
			};

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Letter.ChainL1(Parse.Char(',').Then(fn))(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(
				new string[] { "letter" },
				exception.Expected
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when there are no operators
		/// </summary>
		[Test]
		public static void ChainL1OneSuccess()
		{
			var state = new ParserState("a", 4);
			Func<char, char, char> fn = (l, r) => {
				if (l > r)
				{
					return l;
				}
				else
				{
					return r;
				}
			};

			var result = Parse.Letter
				.ChainL1(Parse.Char(',').Then(fn))(state);
			Assert.AreEqual('a', result);

			Assert.AreEqual(1, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the operator parser fails.
		/// </summary>
		[Test]
		public static void ChainL1OpFailure()
		{
			var state = new ParserState("a,b.z", 4);
			Func<char, char, char> fn = (l, r) => {
				if (l > r)
				{
					return l;
				}
				else
				{
					return r;
				}
			};

			var result = Parse.Letter
				.ChainL1(Parse.Char(',').Then(fn))(state);
			Assert.AreEqual('b', result);

			Assert.AreEqual(3, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the value parser fails.
		/// </summary>
		[Test]
		public static void ChainL1ValueFailure()
		{
			var state = new ParserState("a,z,0", 4);
			Func<char, char, char> fn = (l, r) => {
				if (l > r)
				{
					return l;
				}
				else
				{
					return r;
				}
			};

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Letter.ChainL1(Parse.Char(',').Then(fn))(state)
			);
			Assert.AreEqual(4, exception.Index);
			Assert.AreEqual(
				new string[] { "letter" },
				exception.Expected
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}

		/// <summary>
		/// Test what happens when the operator is missing a value
		/// </summary>
		[Test]
		public static void ChainL1MissingValueFailure()
		{
			var state = new ParserState("a,z,", 4);
			Func<char, char, char> fn = (l, r) => {
				if (l > r)
				{
					return l;
				}
				else
				{
					return r;
				}
			};

			var exception = Assert.Throws<ExpectedException>(() =>
				Parse.Letter.ChainL1(Parse.Char(',').Then(fn))(state)
			);
			Assert.AreEqual(4, exception.Index);
			Assert.AreEqual(
				new string[] { "letter" },
				exception.Expected
			);

			Assert.AreEqual(0, state.Index);
			Assert.AreEqual(0, state.IndentIndex);
		}
	}
}