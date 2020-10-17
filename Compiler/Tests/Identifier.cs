using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Identifier
	{
		[Test]
		public static void IdentifierAlphaSuccess()
		{
			var state = new State("Alice", 4, 0);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("Alice", result);
		}

		[Test]
		public static void IdentifierNumericSuccess()
		{
			var state = new State("123456789", 4, 0);

			var result = Compiler.Identifier(state);
			Assert.AreEqual("123456789", result);
		}

		[Test]
		public static void IdentifierFailure()
		{
			var state = new State("_foobar", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Compiler.Identifier(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "at least 1 more of alphanumeric character" }, exception.Expected);
		}

		#region Label

		[Test]
		public static void IdentifierLabelSuccess()
		{
			var state = new State("[foobar]", 4, 0);

			var result = Compiler.IdentifierLabel(state);
			Assert.AreEqual("foobar", result);
		}

		[Test]
		public static void IdentifierLabelNumericSuccess()
		{
			var state = new State("[123456789]", 4, 0);

			var result = Compiler.IdentifierLabel(state);
			Assert.AreEqual("123456789", result);
		}

		[Test]
		public static void IdentifierLabelFailure()
		{
			var state = new State("[_foobar]", 4, 0);

			var exception = Assert.Throws<ParserException>(() =>
				Compiler.IdentifierLabel(state)
			);
			Assert.AreEqual(1, exception.Index);
			Assert.AreEqual(new string[] { "at least 1 more of alphanumeric character" }, exception.Expected);
		}

		[Test]
		public static void IdentifierLabelRepeatFailure()
		{
			var state = new State("[foobar]", 4, 0);
			state.UsedIdentifiers.Add("foobar");

			var exception = Assert.Throws<ParserException>(() =>
				Compiler.IdentifierLabel(state)
			);
			Assert.AreEqual(0, exception.Index);
			Assert.AreEqual(new string[] { "identifier" }, exception.Expected);
		}

		#endregion
	}
}