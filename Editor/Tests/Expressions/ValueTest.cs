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
			Assert.AreEqual(@int.Value, 1);
			
			var @float = new FloatValue(1.0f);
			Assert.AreEqual(@float.Value, 1.0f);
			
			var @string = new StringValue("1");
			Assert.AreEqual(@string.Value, "1");
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

			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreEqual(c, c);
			Assert.AreEqual(d, d);

			Assert.AreEqual(a, b);
			Assert.AreNotEqual(a, c);
			Assert.AreNotEqual(a, d);
			Assert.AreNotEqual(c, d);
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
			
			Assert.AreEqual(@int.Add(@int), new IntValue(2));
			Assert.AreEqual(@int.Add(@float), new FloatValue(2.0f));
			Assert.AreEqual(@int.Add(@string), new StringValue("11"));
			
			Assert.AreEqual(@float.Add(@int), new FloatValue(2.0f));
			Assert.AreEqual(@float.Add(@float), new FloatValue(2.0f));
			Assert.AreEqual(@float.Add(@string), new StringValue("11"));

			Assert.AreEqual(@string.Add(@int), new StringValue("11"));
			Assert.AreEqual(@string.Add(@float), new StringValue("11"));
			Assert.AreEqual(@string.Add(@string), new StringValue("11"));
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
			
			Assert.AreEqual(@int.Subtract(@int), new IntValue(0));
			Assert.AreEqual(@int.Subtract(@float), new FloatValue(0f));
			Assert.Catch<InvalidOperationException>(() => @int.Subtract(@string));
			
			Assert.AreEqual(@float.Subtract(@int), new FloatValue(0f));
			Assert.AreEqual(@float.Subtract(@float), new FloatValue(0f));
			Assert.Catch<InvalidOperationException>(() => @float.Subtract(@string));

			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@int));
			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@float));
			Assert.Catch<InvalidOperationException>(() => @string.Subtract(@string));
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
			
			Assert.AreEqual(@int.Multiply(@int), new IntValue(1));
			Assert.AreEqual(@int.Multiply(@float), new FloatValue(1f));
			Assert.Catch<InvalidOperationException>(() => @int.Multiply(@string));
			
			Assert.AreEqual(@float.Multiply(@int), new FloatValue(1f));
			Assert.AreEqual(@float.Multiply(@float), new FloatValue(1f));
			Assert.Catch<InvalidOperationException>(() => @float.Multiply(@string));

			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@int));
			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@float));
			Assert.Catch<InvalidOperationException>(() => @string.Multiply(@string));
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
			
			Assert.AreEqual(@int.Divide(@int), new IntValue(1));
			Assert.AreEqual(@int.Divide(@float), new FloatValue(1f));
			Assert.Catch<InvalidOperationException>(() => @int.Divide(@string));
			
			Assert.AreEqual(@float.Divide(@int), new FloatValue(1f));
			Assert.AreEqual(@float.Divide(@float), new FloatValue(1f));
			Assert.Catch<InvalidOperationException>(() => @float.Divide(@string));

			Assert.Catch<InvalidOperationException>(() => @string.Divide(@int));
			Assert.Catch<InvalidOperationException>(() => @string.Divide(@float));
			Assert.Catch<InvalidOperationException>(() => @string.Divide(@string));
		}
	}
}
