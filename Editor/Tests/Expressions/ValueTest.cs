using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure Literal expressions operate as expected.
	/// </summary>
	public class ValueTest
	{
		/// <summary>
		/// Check the constructors of Values.
		/// </summary>
		[Test]
		public void ValueConstructor()
		{
			var @int = new IntValue(1);
			Assert.AreEqual(@int.AsInt(), 1);

			var @float = new FloatValue(1.0f);
			Assert.AreEqual(@float.AsFloat(), 1.0f);

			var @string = new StringValue("1");
			Assert.AreEqual(@string.AsString(), "1");

			var @bool = new BoolValue(true);
			Assert.AreEqual(@bool.AsBool(), true);

			var @obj = new ObjectValue(null);
			Assert.AreEqual(@obj.AsObject(), null);
		}

		/// <summary>
		/// Check Value equality.
		/// </summary>
		[Test]
		public void ValueEquality()
		{
			var a = new IntValue(1);
			var b = new IntValue(1);
			var c = new FloatValue(1.0f);
			var d = new StringValue("1");
			var e = new BoolValue(true);
			var f = new ObjectValue(null);

			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreEqual(c, c);
			Assert.AreEqual(d, d);
			Assert.AreEqual(e, e);
			Assert.AreEqual(f, f);

			Assert.AreEqual(a, b);
			Assert.AreNotEqual(a, c);
			Assert.AreNotEqual(a, d);
			Assert.AreNotEqual(a, e);
			Assert.AreNotEqual(a, f);
			Assert.AreNotEqual(c, d);
			Assert.AreNotEqual(c, e);
			Assert.AreNotEqual(c, f);
			Assert.AreNotEqual(d, e);
			Assert.AreNotEqual(d, f);
			Assert.AreNotEqual(e, f);
		}

		/// <summary>
		/// Check Value not-ing.
		/// </summary>
		[Test]
		public void ValueNot()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(true);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.Not().AsBool(), false);
			Assert.AreEqual(@float.Not().AsBool(), false);
			Assert.AreEqual(@string.Not().AsBool(), false);
			Assert.AreEqual(@bool.Not().AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @obj.Not());
		}

		/// <summary>
		/// Check Value addition.
		/// </summary>
		[Test]
		public void ValueAdd()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(true);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.Add(@int).AsInt(), 2);
			Assert.AreEqual(@int.Add(@float).AsFloat(), 2.0f);
			Assert.AreEqual(@int.Add(@string).AsString(), "11");
			Assert.Catch<InvalidOperationException>(() => @int.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Add(@obj));

			Assert.AreEqual(@float.Add(@int).AsFloat(), 2.0f);
			Assert.AreEqual(@float.Add(@float).AsFloat(), 2.0f);
			Assert.AreEqual(@float.Add(@string).AsString(), "11");
			Assert.Catch<InvalidOperationException>(() => @float.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.Add(@obj));

			Assert.AreEqual(@string.Add(@int).AsString(), "11");
			Assert.AreEqual(@string.Add(@float).AsString(), "11");
			Assert.AreEqual(@string.Add(@string).AsString(), "11");
			Assert.AreEqual(@string.Add(@bool).AsString(), "1true");
			Assert.AreEqual(@string.Add(@obj).AsString(), "1");

			Assert.Catch<InvalidOperationException>(() => @bool.Add(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.Add(@float));
			Assert.AreEqual(@bool.Add(@string).AsString(), "true1");
			Assert.Catch<InvalidOperationException>(() => @bool.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.Add(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.Add(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.Add(@float));
			Assert.AreEqual(@obj.Add(@string).AsString(), "1");
			Assert.Catch<InvalidOperationException>(() => @obj.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.Add(@obj));
		}

		/// <summary>
		/// Check Value subtraction.
		/// </summary>
		[Test]
		public void ValueSubtract()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(true);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.Subtract(@int).AsInt(), 0);
			Assert.AreEqual(@int.Subtract(@float).AsFloat(), 0f);
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@string));
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@obj));

			Assert.AreEqual(@float.Subtract(@int).AsFloat(), 0f);
			Assert.AreEqual(@float.Subtract(@float).AsFloat(), 0f);
			Assert.Catch<InvalidOperationException>(() => @float.Subtract(@string));
			Assert.Catch<InvalidOperationException>(() => @float.Subtract(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.Subtract(@obj));

			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@int));
			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@float));
			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@string));
			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@bool));
			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@obj));

			Assert.Catch<InvalidOperationException>(() => @bool.Subtract(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.Subtract(@float));
			Assert.Catch<InvalidOperationException>(() => @bool.Subtract(@string));
			Assert.Catch<InvalidOperationException>(() => @bool.Subtract(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.Subtract(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.Subtract(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.Subtract(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.Subtract(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.Subtract(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.Subtract(@obj));
		}

		/// <summary>
		/// Check Value multiplication.
		/// </summary>
		[Test]
		public void ValueMultiply()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(true);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.Multiply(@int).AsInt(), 1);
			Assert.AreEqual(@int.Multiply(@float).AsFloat(), 1f);
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@string));
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@obj));

			Assert.AreEqual(@float.Multiply(@int).AsFloat(), 1f);
			Assert.AreEqual(@float.Multiply(@float).AsFloat(), 1f);
			Assert.Catch<InvalidOperationException>(() => @float.Multiply(@string));
			Assert.Catch<InvalidOperationException>(() => @float.Multiply(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.Multiply(@obj));

			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@int));
			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@float));
			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@string));
			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@bool));
			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@obj));

			Assert.Catch<InvalidOperationException>(() => @bool.Multiply(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.Multiply(@float));
			Assert.Catch<InvalidOperationException>(() => @bool.Multiply(@string));
			Assert.Catch<InvalidOperationException>(() => @bool.Multiply(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.Multiply(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.Multiply(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.Multiply(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.Multiply(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.Multiply(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.Multiply(@obj));
		}

		/// <summary>
		/// Check Value division.
		/// </summary>
		[Test]
		public void ValueDivide()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(true);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.Divide(@int).AsInt(), 1);
			Assert.AreEqual(@int.Divide(@float).AsFloat(), 1f);
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@string));
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@obj));

			Assert.AreEqual(@float.Divide(@int).AsFloat(), 1f);
			Assert.AreEqual(@float.Divide(@float).AsFloat(), 1f);
			Assert.Catch<InvalidOperationException>(() => @float.Divide(@string));
			Assert.Catch<InvalidOperationException>(() => @float.Divide(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.Divide(@obj));

			Assert.Catch<InvalidOperationException>(() => @string.Divide(@int));
			Assert.Catch<InvalidOperationException>(() => @string.Divide(@float));
			Assert.Catch<InvalidOperationException>(() => @string.Divide(@string));
			Assert.Catch<InvalidOperationException>(() => @string.Divide(@bool));
			Assert.Catch<InvalidOperationException>(() => @string.Divide(@obj));

			Assert.Catch<InvalidOperationException>(() => @bool.Divide(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.Divide(@float));
			Assert.Catch<InvalidOperationException>(() => @bool.Divide(@string));
			Assert.Catch<InvalidOperationException>(() => @bool.Divide(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.Divide(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.Divide(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.Divide(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.Divide(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.Divide(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.Divide(@obj));
		}

		/// <summary>
		/// Check Boolean And.
		/// </summary>
		[Test]
		public void ValueBoolAnd()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(false);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.BoolAnd(@int).AsBool(), true);
			Assert.AreEqual(@int.BoolAnd(@float).AsBool(), true);
			Assert.AreEqual(@int.BoolAnd(@string).AsBool(), true);
			Assert.AreEqual(@int.BoolAnd(@bool).AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @int.BoolAnd(@obj));

			Assert.AreEqual(@float.BoolAnd(@int).AsBool(), true);
			Assert.AreEqual(@float.BoolAnd(@float).AsBool(), true);
			Assert.AreEqual(@float.BoolAnd(@string).AsBool(), true);
			Assert.AreEqual(@float.BoolAnd(@bool).AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @float.BoolAnd(@obj));

			Assert.AreEqual(@string.BoolAnd(@int).AsBool(), true);
			Assert.AreEqual(@string.BoolAnd(@float).AsBool(), true);
			Assert.AreEqual(@string.BoolAnd(@string).AsBool(), true);
			Assert.AreEqual(@string.BoolAnd(@bool).AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @string.BoolAnd(@obj));

			Assert.AreEqual(@bool.BoolAnd(@int).AsBool(), false);
			Assert.AreEqual(@bool.BoolAnd(@float).AsBool(), false);
			Assert.AreEqual(@bool.BoolAnd(@string).AsBool(), false);
			Assert.AreEqual(@bool.BoolAnd(@bool).AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @bool.BoolAnd(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.BoolAnd(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolAnd(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolAnd(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolAnd(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolAnd(@obj));
		}

		/// <summary>
		/// Check Boolean Or.
		/// </summary>
		[Test]
		public void ValueBoolOr()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(false);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.BoolOr(@int).AsBool(), true);
			Assert.AreEqual(@int.BoolOr(@float).AsBool(), true);
			Assert.AreEqual(@int.BoolOr(@string).AsBool(), true);
			Assert.AreEqual(@int.BoolOr(@bool).AsBool(), true);
			Assert.Catch<InvalidOperationException>(() => @int.BoolAnd(@obj));

			Assert.AreEqual(@float.BoolOr(@int).AsBool(), true);
			Assert.AreEqual(@float.BoolOr(@float).AsBool(), true);
			Assert.AreEqual(@float.BoolOr(@string).AsBool(), true);
			Assert.AreEqual(@float.BoolOr(@bool).AsBool(), true);
			Assert.Catch<InvalidOperationException>(() => @float.BoolAnd(@obj));

			Assert.AreEqual(@string.BoolOr(@int).AsBool(), true);
			Assert.AreEqual(@string.BoolOr(@float).AsBool(), true);
			Assert.AreEqual(@string.BoolOr(@string).AsBool(), true);
			Assert.AreEqual(@string.BoolOr(@bool).AsBool(), true);
			Assert.Catch<InvalidOperationException>(() => @string.BoolAnd(@obj));

			Assert.AreEqual(@bool.BoolOr(@int).AsBool(), true);
			Assert.AreEqual(@bool.BoolOr(@float).AsBool(), true);
			Assert.AreEqual(@bool.BoolOr(@string).AsBool(), true);
			Assert.AreEqual(@bool.BoolOr(@bool).AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @bool.BoolAnd(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.BoolOr(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolOr(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolOr(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolOr(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolOr(@obj));
		}

		/// <summary>
		/// Check Boolean Xor.
		/// </summary>
		[Test]
		public void ValueBoolXor()
		{
			var @int = new IntValue(1);
			var @float = new FloatValue(1.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(false);
			var @obj = new ObjectValue(null);

			Assert.AreEqual(@int.BoolXor(@int).AsBool(), false);
			Assert.AreEqual(@int.BoolXor(@float).AsBool(), false);
			Assert.AreEqual(@int.BoolXor(@string).AsBool(), false);
			Assert.AreEqual(@int.BoolXor(@bool).AsBool(), true);
			Assert.Catch<InvalidOperationException>(() => @int.BoolAnd(@obj));

			Assert.AreEqual(@float.BoolXor(@int).AsBool(), false);
			Assert.AreEqual(@float.BoolXor(@float).AsBool(), false);
			Assert.AreEqual(@float.BoolXor(@string).AsBool(), false);
			Assert.AreEqual(@float.BoolXor(@bool).AsBool(), true);
			Assert.Catch<InvalidOperationException>(() => @float.BoolAnd(@obj));

			Assert.AreEqual(@string.BoolXor(@int).AsBool(), false);
			Assert.AreEqual(@string.BoolXor(@float).AsBool(), false);
			Assert.AreEqual(@string.BoolXor(@string).AsBool(), false);
			Assert.AreEqual(@string.BoolXor(@bool).AsBool(), true);
			Assert.Catch<InvalidOperationException>(() => @string.BoolAnd(@obj));

			Assert.AreEqual(@bool.BoolXor(@int).AsBool(), true);
			Assert.AreEqual(@bool.BoolXor(@float).AsBool(), true);
			Assert.AreEqual(@bool.BoolXor(@string).AsBool(), true);
			Assert.AreEqual(@bool.BoolXor(@bool).AsBool(), false);
			Assert.Catch<InvalidOperationException>(() => @bool.BoolAnd(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.BoolXor(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolXor(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolXor(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolXor(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.BoolXor(@obj));
		}
	}
}
