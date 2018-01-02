#if UNITY_EDITOR

using Exodrifter.Rumor.Expressions;
using NUnit.Framework;
using System;

namespace Exodrifter.Rumor.Test.Expressions
{
	/// <summary>
	/// Ensure expressions work as expected when provided null values.
	/// </summary>
	public class NullExpressionsTest
	{
		/// <summary>
		/// Tests operations with null and bool values.
		/// </summary>
		[Test]
		public void NullAndBool()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("a", true);

			Expression exp = new AddExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new AddExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new SubtractExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new SubtractExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new MultiplyExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new MultiplyExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new DivideExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new DivideExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new BoolAndExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());
			exp = new BoolAndExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());

			exp = new BoolOrExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
			exp = new BoolOrExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
		}

		/// <summary>
		/// Tests operations with null and int values.
		/// </summary>
		[Test]
		public void NullAndInt()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("a", 1);

			Expression exp = new AddExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(1, exp.Evaluate(rumor).AsInt());
			exp = new AddExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(1, exp.Evaluate(rumor).AsInt());

			exp = new SubtractExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(1, exp.Evaluate(rumor).AsInt());
			exp = new SubtractExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(-1, exp.Evaluate(rumor).AsInt());

			exp = new MultiplyExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(0, exp.Evaluate(rumor).AsInt());
			exp = new MultiplyExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(0, exp.Evaluate(rumor).AsInt());

			exp = new DivideExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(0, exp.Evaluate(rumor).AsInt());
			exp = new DivideExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(0, exp.Evaluate(rumor).AsInt());

			exp = new BoolAndExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());
			exp = new BoolAndExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());

			exp = new BoolOrExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
			exp = new BoolOrExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
		}

		/// <summary>
		/// Tests operations with null and float values.
		/// </summary>
		[Test]
		public void NullAndFloat()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("a", 1f);

			Expression exp = new AddExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(1f, exp.Evaluate(rumor).AsFloat());
			exp = new AddExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(1f, exp.Evaluate(rumor).AsFloat());

			exp = new SubtractExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(1f, exp.Evaluate(rumor).AsFloat());
			exp = new SubtractExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(-1f, exp.Evaluate(rumor).AsFloat());

			exp = new MultiplyExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(0f, exp.Evaluate(rumor).AsFloat());
			exp = new MultiplyExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(0f, exp.Evaluate(rumor).AsFloat());

			exp = new DivideExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(0f, exp.Evaluate(rumor).AsFloat());
			exp = new DivideExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(0f, exp.Evaluate(rumor).AsFloat());

			exp = new BoolAndExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());
			exp = new BoolAndExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());

			exp = new BoolOrExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
			exp = new BoolOrExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
		}

		/// <summary>
		/// Tests operations with null and string values.
		/// </summary>
		[Test]
		public void NullAndString()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			rumor.Scope.SetVar("a", "1");

			Expression exp = new AddExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual("1", exp.Evaluate(rumor).AsString());
			exp = new AddExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual("1", exp.Evaluate(rumor).AsString());

			exp = new SubtractExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new SubtractExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new MultiplyExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new MultiplyExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new DivideExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());
			exp = new DivideExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.Throws<InvalidOperationException>(() => exp.Evaluate(rumor).AsString());

			exp = new BoolAndExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());
			exp = new BoolAndExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(false, exp.Evaluate(rumor).AsBool());

			exp = new BoolOrExpression(
				new VariableExpression("a"),
				new VariableExpression("b")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
			exp = new BoolOrExpression(
				new VariableExpression("b"),
				new VariableExpression("a")
			);
			Assert.AreEqual(true, exp.Evaluate(rumor).AsBool());
		}
	}
}

#endif
