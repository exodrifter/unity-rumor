﻿using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Comparison
	{
		#region Is

		[Test]
		public static void IsBooleanSuccess()
		{
			var state = new ParserState("false is false", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void IsBooleanFailure()
		{
			var state = new ParserState("true is false", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		[Test]
		public static void IsNumberSuccess()
		{
			var state = new ParserState("4 is 4", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void IsNumberFailure()
		{
			var state = new ParserState("4 is 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		[Test]
		public static void IsStringSuccess()
		{
			var state = new ParserState("\"a\" is \"a\"", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void IsStringFailure()
		{
			var state = new ParserState("\"a\" is \"b\"", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		#endregion

		#region Is Not

		[Test]
		public static void IsNotBooleanSuccess()
		{
			var state = new ParserState("true is not false", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void IsNotBooleanFailure()
		{
			var state = new ParserState("false is not false", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		[Test]
		public static void IsNotNumberSuccess()
		{
			var state = new ParserState("4 is not 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void IsNotNumberFailure()
		{
			var state = new ParserState("4 is not 4", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		[Test]
		public static void IsNotStringSuccess()
		{
			var state = new ParserState("\"a\" is not \"b\"", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void IsNotStringFailure()
		{
			var state = new ParserState("\"a\" is not \"a\"", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		#endregion

		#region Greater

		[Test]
		public static void GreaterSuccess()
		{
			var state = new ParserState("5 > 4", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void GreaterFailure()
		{
			var state = new ParserState("4 > 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		#endregion

		#region Greater Or Equal

		[Test]
		public static void GreaterOrEqualSuccess()
		{
			var state = new ParserState("5 >= 4", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void GreaterOrEqualExactSuccess()
		{
			var state = new ParserState("5 >= 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void GreaterOrEqualFailure()
		{
			var state = new ParserState("4 >= 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		#endregion

		#region Less

		[Test]
		public static void LessSuccess()
		{
			var state = new ParserState("4 < 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void LessFailure()
		{
			var state = new ParserState("5 < 4", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		#endregion

		#region Less Or Equal

		[Test]
		public static void LessOrEqualSuccess()
		{
			var state = new ParserState("4 <= 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void LessOrEqualExactSuccess()
		{
			var state = new ParserState("5 <= 5", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(true), result.Evaluate());
		}

		[Test]
		public static void LessOrEqualFailure()
		{
			var state = new ParserState("5 <= 4", 4, 0);

			var result = Compiler.Comparison(state);
			Assert.AreEqual(new BooleanValue(false), result.Evaluate());
		}

		#endregion
	}
}
