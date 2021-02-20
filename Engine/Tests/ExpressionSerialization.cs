using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class ExpressionSerialization
	{
		#region Comparison

		[Test]
		public static void SerializeIsExpression()
		{
			var v = new NumberLiteral(2);
			var a = new IsExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeIsNotExpression()
		{
			var v = new NumberLiteral(2);
			var a = new IsNotExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeLessThanExpression()
		{
			var v = new NumberLiteral(2);
			var a = new LessThanExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeLessThanOrEqualExpression()
		{
			var v = new NumberLiteral(2);
			var a = new LessThanOrEqualExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeGreaterThanExpression()
		{
			var v = new NumberLiteral(2);
			var a = new GreaterThanExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeGreaterThanOrEqualExpression()
		{
			var v = new NumberLiteral(2);
			var a = new GreaterThanOrEqualExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region Literal

		[Test]
		public static void SerializeBooleanLiteral()
		{
			var a = new BooleanLiteral(true);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeNumberLiteral()
		{
			var a = new NumberLiteral(2);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeStringLiteral()
		{
			var a = new StringLiteral("Hello world!");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region Logic

		[Test]
		public static void SerializeAndExpression()
		{
			var v = new BooleanLiteral(true);
			var a = new AndExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeOrExpression()
		{
			var v = new BooleanLiteral(true);
			var a = new OrExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeXorExpression()
		{
			var v = new BooleanLiteral(true);
			var a = new XorExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region Math

		[Test]
		public static void SerializeAddExpression()
		{
			var v = new NumberLiteral(2);
			var a = new AddExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeDivideExpression()
		{
			var v = new NumberLiteral(2);
			var a = new DivideExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeMultiplyExpression()
		{
			var v = new NumberLiteral(2);
			var a = new MultiplyExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeSubtractExpression()
		{
			var v = new NumberLiteral(2);
			var a = new SubtractExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region String

		[Test]
		public static void SerializeConcatExpression()
		{
			var v = new StringLiteral("Hello");
			var a = new ConcatExpression(v, v);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeSubstitutionExpression()
		{
			var a = new SubstitutionExpression(new NumberLiteral(2));
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeToStringExpression()
		{
			var a = new ToStringExpression(new NumberLiteral(2));
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region Value

		[Test]
		public static void SerializeBooleanValue()
		{
			var a = new BooleanValue(true);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeNumberValue()
		{
			var a = new NumberValue(12345.6);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeStringValue()
		{
			var a = new StringValue("Hello world!");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region Variable

		[Test]
		public static void SerializeBooleanVariable()
		{
			var a = new BooleanVariable("foobar");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeNumberVariable()
		{
			var a = new NumberVariable("foobar");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeStringVariable()
		{
			var a = new StringVariable("foobar");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SerializeVariableExpression()
		{
			var a = new VariableExpression("foobar");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion
	}
}
