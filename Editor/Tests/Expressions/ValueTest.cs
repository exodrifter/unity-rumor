#if UNITY_EDITOR

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
		class Foo
		{
			public override string ToString ()
			{
				return "";
			}
		}

		/// <summary>
		/// Check the constructors of Values.
		/// </summary>
		[Test]
		public void ValueConstructor()
		{
			var @int = new IntValue(1);
			Assert.AreEqual(1, @int.AsObject());

			var @float = new FloatValue(1.0f);
			Assert.AreEqual(1.0f, @float.AsObject());

			var @string = new StringValue("1");
			Assert.AreEqual("1", @string.AsObject());

			var @bool = new BoolValue(true);
			Assert.AreEqual(true, @bool.AsObject());

			var @obj = new ObjectValue(null);
			Assert.AreEqual(null, @obj.AsObject());
		}

		/// <summary>
		/// Check Value equality.
		/// </summary>
		[Test]
		public void ValueEquality()
		{
			var a = new IntValue(1);
			var b = new FloatValue(1.0f);
			var c = new StringValue("1");
			var e = new BoolValue(true);
			var f = new ObjectValue(new Foo());

			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreEqual(c, c);
			Assert.AreEqual(e, e);
			Assert.AreEqual(f, f);

			Assert.AreNotEqual(a, b);
			Assert.AreNotEqual(a, c);
			Assert.AreNotEqual(a, e);
			Assert.AreNotEqual(a, f);
			Assert.AreNotEqual(b, c);
			Assert.AreNotEqual(b, e);
			Assert.AreNotEqual(b, f);
			Assert.AreNotEqual(c, e);
			Assert.AreNotEqual(c, f);
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(false, @int.Not().AsBool());
			Assert.AreEqual(false, @float.Not().AsBool());
			Assert.AreEqual(false, @string.Not().AsBool());
			Assert.AreEqual(false, @bool.Not().AsBool());
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(2, @int.Add(@int).AsObject());
			Assert.AreEqual(2.0f, @int.Add(@float).AsFloat());
			Assert.AreEqual("11", @int.Add(@string).AsString());
			Assert.Catch<InvalidOperationException>(() => @int.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Add(@obj));

			Assert.AreEqual(2.0f, @float.Add(@int).AsFloat());
			Assert.AreEqual(2.0f, @float.Add(@float).AsFloat(), 2.0f);
			Assert.AreEqual("11", @float.Add(@string).AsString(), "11");
			Assert.Catch<InvalidOperationException>(() => @float.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.Add(@obj));

			Assert.AreEqual("11", @string.Add(@int).AsString());
			Assert.AreEqual("11", @string.Add(@float).AsString());
			Assert.AreEqual("11", @string.Add(@string).AsString());
			Assert.AreEqual("1true", @string.Add(@bool).AsString());
			Assert.AreEqual("1", @string.Add(@obj).AsString());

			Assert.Catch<InvalidOperationException>(() => @bool.Add(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.Add(@float));
			Assert.AreEqual("true1", @bool.Add(@string).AsString());
			Assert.Catch<InvalidOperationException>(() => @bool.Add(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.Add(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.Add(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.Add(@float));
			Assert.AreEqual("1", @obj.Add(@string).AsString());
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(0, @int.Subtract(@int).AsObject());
			Assert.AreEqual(0f, @int.Subtract(@float).AsFloat());
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@string));
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@obj));

			Assert.AreEqual(0f, @float.Subtract(@int).AsFloat());
			Assert.AreEqual(0f, @float.Subtract(@float).AsFloat());
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(1, @int.Multiply(@int).AsObject());
			Assert.AreEqual(1f, @int.Multiply(@float).AsFloat());
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@string));
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@obj));

			Assert.AreEqual(1f, @float.Multiply(@int).AsFloat());
			Assert.AreEqual(1f, @float.Multiply(@float).AsFloat());
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(1, @int.Divide(@int).AsObject());
			Assert.AreEqual(1f, @int.Divide(@float).AsFloat());
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@string));
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@obj));

			Assert.AreEqual(1f, @float.Divide(@int).AsFloat());
			Assert.AreEqual(1f, @float.Divide(@float).AsFloat());
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
		/// Check Less Than.
		/// </summary>
		[Test]
		public void ValueLessThan()
		{
			var @int = new IntValue(1);
			var @int2 = new IntValue(2);
			var @float = new FloatValue(1.0f);
			var @float2 = new FloatValue(2.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(false);
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(false, @int.LessThan(@int).AsBool());
			Assert.AreEqual(false, @int.LessThan(@float).AsBool());
			Assert.AreEqual(true, @int.LessThan(@int2).AsBool());
			Assert.AreEqual(true, @int.LessThan(@float2).AsBool());
			Assert.AreEqual(false, @int2.LessThan(@int).AsBool());
			Assert.AreEqual(false, @int2.LessThan(@float).AsBool());
			Assert.Catch<InvalidOperationException>(() => @int.LessThan(@string));
			Assert.Catch<InvalidOperationException>(() => @int.LessThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.LessThan(@obj));

			Assert.AreEqual(false, @float.LessThan(@int).AsBool());
			Assert.AreEqual(false, @float.LessThan(@float).AsBool());
			Assert.AreEqual(true, @float.LessThan(@int2).AsBool());
			Assert.AreEqual(true, @float.LessThan(@float2).AsBool());
			Assert.AreEqual(false, @float2.LessThan(@int).AsBool());
			Assert.AreEqual(false, @float2.LessThan(@float).AsBool());
			Assert.Catch<InvalidOperationException>(() => @float.LessThan(@string));
			Assert.Catch<InvalidOperationException>(() => @float.LessThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.LessThan(@obj));

			Assert.Catch<InvalidOperationException>(() => @string.LessThan(@int));
			Assert.Catch<InvalidOperationException>(() => @string.LessThan(@float));
			Assert.Catch<InvalidOperationException>(() => @string.LessThan(@string));
			Assert.Catch<InvalidOperationException>(() => @string.LessThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @string.LessThan(@obj));

			Assert.Catch<InvalidOperationException>(() => @bool.LessThan(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.LessThan(@float));
			Assert.Catch<InvalidOperationException>(() => @bool.LessThan(@string));
			Assert.Catch<InvalidOperationException>(() => @bool.LessThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.LessThan(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.LessThan(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.LessThan(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.LessThan(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.LessThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.LessThan(@obj));
		}

		/// <summary>
		/// Check Less Than.
		/// </summary>
		[Test]
		public void ValueGreaterThan()
		{
			var @int = new IntValue(1);
			var @int2 = new IntValue(2);
			var @float = new FloatValue(1.0f);
			var @float2 = new FloatValue(2.0f);
			var @string = new StringValue("1");
			var @bool = new BoolValue(false);
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(false, @int.GreaterThan(@int).AsBool());
			Assert.AreEqual(false, @int.GreaterThan(@float).AsBool());
			Assert.AreEqual(false, @int.GreaterThan(@int2).AsBool());
			Assert.AreEqual(false, @int.GreaterThan(@float2).AsBool());
			Assert.AreEqual(true, @int2.GreaterThan(@int).AsBool());
			Assert.AreEqual(true, @int2.GreaterThan(@float).AsBool());
			Assert.Catch<InvalidOperationException>(() => @int.GreaterThan(@string));
			Assert.Catch<InvalidOperationException>(() => @int.GreaterThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @int.GreaterThan(@obj));

			Assert.AreEqual(false, @float.GreaterThan(@int).AsBool());
			Assert.AreEqual(false, @float.GreaterThan(@float).AsBool());
			Assert.AreEqual(false, @float.GreaterThan(@int2).AsBool());
			Assert.AreEqual(false, @float.GreaterThan(@float2).AsBool());
			Assert.AreEqual(true, @float2.GreaterThan(@int).AsBool());
			Assert.AreEqual(true, @float2.GreaterThan(@float).AsBool());
			Assert.Catch<InvalidOperationException>(() => @float.GreaterThan(@string));
			Assert.Catch<InvalidOperationException>(() => @float.GreaterThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @float.GreaterThan(@obj));

			Assert.Catch<InvalidOperationException>(() => @string.GreaterThan(@int));
			Assert.Catch<InvalidOperationException>(() => @string.GreaterThan(@float));
			Assert.Catch<InvalidOperationException>(() => @string.GreaterThan(@string));
			Assert.Catch<InvalidOperationException>(() => @string.GreaterThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @string.GreaterThan(@obj));

			Assert.Catch<InvalidOperationException>(() => @bool.GreaterThan(@int));
			Assert.Catch<InvalidOperationException>(() => @bool.GreaterThan(@float));
			Assert.Catch<InvalidOperationException>(() => @bool.GreaterThan(@string));
			Assert.Catch<InvalidOperationException>(() => @bool.GreaterThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @bool.GreaterThan(@obj));

			Assert.Catch<InvalidOperationException>(() => @obj.GreaterThan(@int));
			Assert.Catch<InvalidOperationException>(() => @obj.GreaterThan(@float));
			Assert.Catch<InvalidOperationException>(() => @obj.GreaterThan(@string));
			Assert.Catch<InvalidOperationException>(() => @obj.GreaterThan(@bool));
			Assert.Catch<InvalidOperationException>(() => @obj.GreaterThan(@obj));
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(true, @int.BoolAnd(@int).AsBool());
			Assert.AreEqual(true, @int.BoolAnd(@float).AsBool());
			Assert.AreEqual(true, @int.BoolAnd(@string).AsBool());
			Assert.AreEqual(false, @int.BoolAnd(@bool).AsBool());
			Assert.AreEqual(true, @int.BoolAnd(@obj).AsBool());

			Assert.AreEqual(true, @float.BoolAnd(@int).AsBool());
			Assert.AreEqual(true, @float.BoolAnd(@float).AsBool());
			Assert.AreEqual(true, @float.BoolAnd(@string).AsBool());
			Assert.AreEqual(false, @float.BoolAnd(@bool).AsBool());
			Assert.AreEqual(true, @float.BoolAnd(@obj).AsBool());

			Assert.AreEqual(true, @string.BoolAnd(@int).AsBool());
			Assert.AreEqual(true, @string.BoolAnd(@float).AsBool());
			Assert.AreEqual(true, @string.BoolAnd(@string).AsBool());
			Assert.AreEqual(false, @string.BoolAnd(@bool).AsBool());
			Assert.AreEqual(true, @string.BoolAnd(@obj).AsBool());

			Assert.AreEqual(false, @bool.BoolAnd(@int).AsBool());
			Assert.AreEqual(false, @bool.BoolAnd(@float).AsBool());
			Assert.AreEqual(false, @bool.BoolAnd(@string).AsBool());
			Assert.AreEqual(false, @bool.BoolAnd(@bool).AsBool());
			Assert.AreEqual(false, @bool.BoolAnd(@obj).AsBool());

			Assert.AreEqual(true, @obj.BoolAnd(@int).AsBool());
			Assert.AreEqual(true, @obj.BoolAnd(@float).AsBool());
			Assert.AreEqual(true, @obj.BoolAnd(@string).AsBool());
			Assert.AreEqual(false, @obj.BoolAnd(@bool).AsBool());
			Assert.AreEqual(true, @obj.BoolAnd(@obj).AsBool());
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(true, @int.BoolOr(@int).AsBool());
			Assert.AreEqual(true, @int.BoolOr(@float).AsBool());
			Assert.AreEqual(true, @int.BoolOr(@string).AsBool());
			Assert.AreEqual(true, @int.BoolOr(@bool).AsBool());
			Assert.AreEqual(true, @int.BoolOr(@obj).AsBool());

			Assert.AreEqual(true, @float.BoolOr(@int).AsBool());
			Assert.AreEqual(true, @float.BoolOr(@float).AsBool());
			Assert.AreEqual(true, @float.BoolOr(@string).AsBool());
			Assert.AreEqual(true, @float.BoolOr(@bool).AsBool());
			Assert.AreEqual(true, @float.BoolOr(@obj).AsBool());

			Assert.AreEqual(true, @string.BoolOr(@int).AsBool());
			Assert.AreEqual(true, @string.BoolOr(@float).AsBool());
			Assert.AreEqual(true, @string.BoolOr(@string).AsBool());
			Assert.AreEqual(true, @string.BoolOr(@bool).AsBool());
			Assert.AreEqual(true,  @string.BoolOr(@obj).AsBool());

			Assert.AreEqual(true, @bool.BoolOr(@int).AsBool());
			Assert.AreEqual(true, @bool.BoolOr(@float).AsBool());
			Assert.AreEqual(true, @bool.BoolOr(@string).AsBool());
			Assert.AreEqual(false, @bool.BoolOr(@bool).AsBool());
			Assert.AreEqual(true, @bool.BoolOr(@obj).AsBool());

			Assert.AreEqual(true, @obj.BoolOr(@int).AsBool());
			Assert.AreEqual(true, @obj.BoolOr(@float).AsBool());
			Assert.AreEqual(true, @obj.BoolOr(@string).AsBool());
			Assert.AreEqual(true, @obj.BoolOr(@bool).AsBool());
			Assert.AreEqual(true, @obj.BoolOr(@obj).AsBool());
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
			var @obj = new ObjectValue(new Foo());

			Assert.AreEqual(false, @int.BoolXor(@int).AsBool());
			Assert.AreEqual(false, @int.BoolXor(@float).AsBool());
			Assert.AreEqual(false, @int.BoolXor(@string).AsBool());
			Assert.AreEqual(true, @int.BoolXor(@bool).AsBool());
			Assert.AreEqual(false, @int.BoolXor(@obj).AsBool());

			Assert.AreEqual(false, @float.BoolXor(@int).AsBool());
			Assert.AreEqual(false, @float.BoolXor(@float).AsBool());
			Assert.AreEqual(false, @float.BoolXor(@string).AsBool());
			Assert.AreEqual(true, @float.BoolXor(@bool).AsBool());
			Assert.AreEqual(false, @float.BoolXor(@obj).AsBool());

			Assert.AreEqual(false, @string.BoolXor(@int).AsBool());
			Assert.AreEqual(false, @string.BoolXor(@float).AsBool());
			Assert.AreEqual(false, @string.BoolXor(@string).AsBool());
			Assert.AreEqual(true, @string.BoolXor(@bool).AsBool());
			Assert.AreEqual(false, @string.BoolXor(@obj).AsBool());

			Assert.AreEqual(true, @bool.BoolXor(@int).AsBool());
			Assert.AreEqual(true, @bool.BoolXor(@float).AsBool());
			Assert.AreEqual(true, @bool.BoolXor(@string).AsBool());
			Assert.AreEqual(false, @bool.BoolXor(@bool).AsBool());
			Assert.AreEqual(true, @bool.BoolXor(@obj).AsBool());

			Assert.AreEqual(false, @obj.BoolXor(@int).AsBool());
			Assert.AreEqual(false, @obj.BoolXor(@float).AsBool());
			Assert.AreEqual(false, @obj.BoolXor(@string).AsBool());
			Assert.AreEqual(true, @obj.BoolXor(@bool).AsBool());
			Assert.AreEqual(false, @obj.BoolXor(@obj).AsBool());
		}
	}
}

#endif
