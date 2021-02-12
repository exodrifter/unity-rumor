using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Identifier
	{
		[Test]
		public static void IdentifierAlphaSuccess()
		{
			var state = new ParserState("Alice", 4);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("Alice", result);
		}

		[Test]
		public static void IdentifierNumericFailure()
		{
			var state = new ParserState("123456789", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Compiler.Identifier(state)
			);
			Assert.AreEqual(
				"parse exception at index 0: expected letter",
				exception.Message
			);
		}

		[Test]
		public static void IdentifierFailure()
		{
			var state = new ParserState("_foobar", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Compiler.Identifier(state)
			);
			Assert.AreEqual(
				"parse exception at index 0: expected letter",
				exception.Message
			);
		}

		#region Label

		[Test]
		public static void IdentifierLabelSuccess()
		{
			var state = new ParserState("[foobar]", 4, new RumorParserState());

			var result = Compiler.IdentifierLabel(state);
			Assert.AreEqual("foobar", result);
		}

		[Test]
		public static void IdentifierLabelNumericFailure()
		{
			var state = new ParserState("[123456789]", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Compiler.IdentifierLabel(state)
			);
			Assert.AreEqual(
				"parse exception at index 1: expected letter",
				exception.Message
			);
		}

		[Test]
		public static void IdentifierLabelFailure()
		{
			var state = new ParserState("[_foobar]", 4);

			var exception = Assert.Throws<ExpectedException>(() =>
				Compiler.IdentifierLabel(state)
			);
			Assert.AreEqual(
				"parse exception at index 1: expected letter",
				exception.Message
			);
		}

		[Test]
		public static void IdentifierLabelRepeatFailure()
		{
			var userState = new RumorParserState();
			userState.UsedIdentifiers.Add("foobar");

			var state = new ParserState("[foobar]", 4, userState);

			var exception = Assert.Throws<ReasonException>(() =>
				Compiler.IdentifierLabel(state)
			);
			Assert.AreEqual(
				"parse exception at index 0: the identifier \"foobar\" has " +
				"already been used!",
				exception.Message
			);
		}

		#endregion
	}
}