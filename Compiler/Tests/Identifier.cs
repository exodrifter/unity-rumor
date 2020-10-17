using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Identifier
	{
		[Test]
		public static void IdentifierAlphaSuccess()
		{
			var state = new ParserState("Alice", 4, 0);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("Alice", result);
		}

		[Test]
		public static void IdentifierNumericSuccess()
		{
			var state = new ParserState("123456789", 4, 0);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("123456789", result);
		}

		[Test]
		public static void IdentifierFailure()
		{
			var state = new ParserState("_foobar", 4, 0);

			var exception = Assert.Throws<ReasonException>(() =>
				Compiler.Identifier(state)
			);
			Assert.AreEqual(
				"parse exception at index 0: expected at least 1 more " +
				"instance(s) of the parser to succeed",
				exception.Message
			);
			Assert.AreEqual(
				"parse exception at index 0: expected alphanumeric character",
				exception.InnerException.Message
			);
		}

		#region Label

		[Test]
		public static void IdentifierLabelSuccess()
		{
			var state = new ParserState("[foobar]", 4, 0);

			var result = Compiler.IdentifierLabel(state);
			Assert.AreEqual("foobar", result);
		}

		[Test]
		public static void IdentifierLabelNumericSuccess()
		{
			var state = new ParserState("[123456789]", 4, 0);

			var result = Compiler.IdentifierLabel(state);
			Assert.AreEqual("123456789", result);
		}

		[Test]
		public static void IdentifierLabelFailure()
		{
			var state = new ParserState("[_foobar]", 4, 0);

			var exception = Assert.Throws<ReasonException>(() =>
				Compiler.IdentifierLabel(state)
			);
			Assert.AreEqual(
				"parse exception at index 1: expected at least 1 more " +
				"instance(s) of the parser to succeed",
				exception.Message
			);
			Assert.AreEqual(
				"parse exception at index 1: expected alphanumeric character",
				exception.InnerException.Message
			);
		}

		[Test]
		public static void IdentifierLabelRepeatFailure()
		{
			var state = new ParserState("[foobar]", 4, 0);
			state.UsedIdentifiers.Add("foobar");

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