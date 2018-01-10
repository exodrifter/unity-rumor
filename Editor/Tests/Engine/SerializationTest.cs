#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exodrifter.Rumor.Test.Engine
{
	/// <summary>
	/// Ensure Rumors can be serialized and deserialized properly.
	/// </summary>
	public class SerializationTest
	{
		#region Expressions

		/// <summary>
		/// Ensure Add expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeAddExpression()
		{
			var num = new LiteralExpression(1);
			var a = new AddExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure BoolAnd expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeBoolAndExpression()
		{
			var num = new LiteralExpression(1);
			var a = new BoolAndExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure BoolOr expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeBoolOrExpression()
		{
			var num = new LiteralExpression(1);
			var a = new BoolOrExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure BoolXor expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeBoolXorExpression()
		{
			var num = new LiteralExpression(1);
			var a = new BoolXorExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Divide expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeDivideExpression()
		{
			var num = new LiteralExpression(1);
			var a = new DivideExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Equals expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeEqualsExpression()
		{
			var num = new LiteralExpression(1);
			var a = new EqualsExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Literal expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeLiteralExpression()
		{
			var a = new LiteralExpression(1);
			var b = Reserialize(a);
			Assert.AreEqual(a, b);

			a = new LiteralExpression(1f);
			b = Reserialize(a);
			Assert.AreEqual(a, b);

			a = new LiteralExpression("1");
			b = Reserialize(a);
			Assert.AreEqual(a, b);

			a = new LiteralExpression(true);
			b = Reserialize(a);
			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Multiply expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeMultiplyExpression()
		{
			var num = new LiteralExpression(1);
			var a = new MultiplyExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure NoOp expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeNoOpExpression()
		{
			var a = new NoOpExpression();
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure NotEquals expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeNotEqualsExpression()
		{
			var num = new LiteralExpression(1);
			var a = new NotEqualsExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Not expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeNotExpression()
		{
			var num = new LiteralExpression(1);
			var a = new NotExpression(num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Set expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeSetExpression()
		{
			var left = new VariableExpression("x");
			var right = new LiteralExpression(1);
			var a = new SetExpression(left, right);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Subtract expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeSubtractExpression()
		{
			var num = new LiteralExpression(1);
			var a = new MultiplyExpression(num, num);
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Value expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeValue()
		{
			Value a = new IntValue(1);
			Value b = Reserialize(a);
			Assert.AreEqual(a, b);

			a = new FloatValue(1.0f);
			b = Reserialize(a);
			Assert.AreEqual(a, b);

			a = new StringValue("1");
			b = Reserialize(a);
			Assert.AreEqual(a, b);

			a = new BoolValue(true);
			b = Reserialize(a);
			Assert.AreEqual(a, b);
		}

		/// <summary>
		/// Ensure Variable expressions are serialized properly.
		/// </summary>
		[Test]
		public void SerializeVariableExpression()
		{
			var a = new VariableExpression("x");
			var b = Reserialize(a);

			Assert.AreEqual(a, b);
		}

		#endregion

		#region Nodes

		/// <summary>
		/// Ensure Add nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeAdd()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			var a = new Add("add");
			var b = Reserialize(a);

			Assert.AreEqual(a.EvaluateText(rumor), b.EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure Call nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeCall()
		{
			var a = new Call("call");
			var b = Reserialize(a);

			Assert.AreEqual(a.to, b.to);
		}

		/// <summary>
		/// Ensure Choice nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeChoice()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			var a = new Choice("choice", new List<Node>() {
				new Say("say")
			});
			var b = Reserialize(a);

			Assert.AreEqual(a.EvaluateText(rumor), b.EvaluateText(rumor));
			Assert.AreEqual(1, a.Children.Count);
			Assert.AreEqual(a.Children.Count, b.Children.Count);
			Assert.AreEqual(
				(a.Children[0] as Say).EvaluateText(rumor),
				(b.Children[0] as Say).EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure Jump nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeJump()
		{
			var a = new Jump("jump");
			var b = Reserialize(a);

			Assert.AreEqual(a.to, b.to);
		}

		/// <summary>
		/// Ensure Label nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeLabel()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			var a = new Label("label", new List<Node>() {
				new Say("say")
			});
			var b = Reserialize(a);

			Assert.AreEqual(a.name, b.name);
			Assert.AreEqual(1, a.Children.Count);
			Assert.AreEqual(a.Children.Count, b.Children.Count);
			Assert.AreEqual(
				(a.Children[0] as Say).EvaluateText(rumor),
				(b.Children[0] as Say).EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure Pause nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializePause()
		{
			var a = new Pause(1, true);
			var b = Reserialize(a);

			Assert.AreEqual(a.seconds, b.seconds);
			Assert.AreEqual(a.cantSkip, b.cantSkip);

			a = new Pause(2, false);
			b = Reserialize(a);

			Assert.AreEqual(a.seconds, b.seconds);
			Assert.AreEqual(a.cantSkip, b.cantSkip);
		}

		/// <summary>
		/// Ensure Return nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeReturn()
		{
			// Just make sure no exceptions are thrown
			Reserialize(new Return());
		}

		/// <summary>
		/// Ensure Say nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeSay()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			var a = new Say("say");
			var b = Reserialize(a);

			Assert.AreEqual(a.EvaluateText(rumor), b.EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure Statement nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializeStatement()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			var a = new Statement(new LiteralExpression(1));
			var b = Reserialize(a);

			Assert.AreEqual(
				a.expression.Evaluate(rumor),
				b.expression.Evaluate(rumor));
		}

		#endregion

		#region Rumor

		/// <summary>
		/// Ensure empty Rumors are serialized properly.
		/// </summary>
		[Test]
		public void SerializeRumorEmpty()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>());

			rumor = Reserialize(rumor);

			Assert.False(rumor.Started);
			Assert.False(rumor.Finished);

			rumor.Start().MoveNext();
			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);
		}

		[Test]
		public void SerializeRumorScope()
		{
			var a = new Rumor.Engine.Rumor(new List<Node>());
			a.Scope.SetVar("a", 1);
			a.Scope.SetVar("b", 1f);
			a.Scope.SetVar("c", "1");
			a.Scope.SetVar("d", true);
			var b = Reserialize(a);

			Assert.AreEqual(a.Scope.GetVar("a"), b.Scope.GetVar("a"));
			Assert.AreEqual(a.Scope.GetVar("b"), b.Scope.GetVar("b"));
			Assert.AreEqual(a.Scope.GetVar("c"), b.Scope.GetVar("c"));
			Assert.AreEqual(a.Scope.GetVar("d"), b.Scope.GetVar("d"));
		}

		/// <summary>
		/// Ensure simple Rumors that only have a root block are serialized
		/// properly.
		/// </summary>
		[Test]
		public void SerializeRumorSimple()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Say("Hello!"),
				new Say("How are you?"),
			});

			rumor = Reserialize(rumor);

			Assert.False(rumor.Started);
			Assert.False(rumor.Finished);

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("Hello!", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("How are you?", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);
		}

		/// <summary>
		/// Ensure Rumors that have multiple blocks are serialized properly.
		/// </summary>
		[Test]
		public void SerializeRumorNormal()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("Hello!"),
					new Say("How are you?"),
				}),
				new Label("b", new List<Node>() {
					new Say("Good."),
					new Say("How about yourself?"),
				}),
			});

			rumor = Reserialize(rumor);

			Assert.False(rumor.Started);
			Assert.False(rumor.Finished);

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("Hello!", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("How are you?", (rumor.Current as Say).EvaluateText(rumor));

			// Check to see if serializing during execution works
			rumor = Reserialize(rumor);

			Assert.AreEqual("How are you?", rumor.State.Dialog[null]);

			yield = rumor.Start();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("Good.", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("How about yourself?", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);
		}

		/// <summary>
		/// Ensure Rumors that have nested blocks are serialized properly.
		/// </summary>
		[Test]
		public void SerializeRumorNested()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("a!"),
					new Label("b", new List<Node>() {
						new Say("b!"),
						new Label("c", new List<Node>() {
							new Say("c!"),
						}),
					}),
				})
			});

			rumor = Reserialize(rumor);

			Assert.False(rumor.Started);
			Assert.False(rumor.Finished);

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("a!", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("b!", (rumor.Current as Say).EvaluateText(rumor));

			// Check to see if serializing during execution works
			rumor = Reserialize(rumor);

			yield = rumor.Start();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("c!", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);
		}

		#endregion

		#region RumorState

		/// <summary>
		/// Ensure the Rumor dialog state is serialized properly.
		/// </summary>
		[Test]
		public void SerializeRumorStateDialog()
		{
			var a = new RumorState();
			a.SetDialog(null, "dialog");
			var b = Reserialize(a);

			Assert.AreEqual(a.Dialog[null], b.Dialog[null]);
		}

		[Test]
		public void SerializeRumorStateChoice()
		{
			var rumor = new Rumor.Engine.Rumor("return");
			var a = new RumorState();
			a.AddChoice("choice", new List<Node>() {
				new Say("say"),
			});

			var b = Reserialize(a);
			Assert.AreEqual(1, a.Choices.Count);
			Assert.AreEqual(a.Choices.Count, b.Choices.Count);
			Assert.AreEqual(a.Choices[0], b.Choices[0]);

			Assert.AreEqual(1, a.Consequences.Count);
			Assert.AreEqual(a.Consequences.Count, b.Consequences.Count);
			Assert.AreEqual(1, a.Consequences[0].Count);
			Assert.AreEqual(a.Consequences[0].Count, b.Consequences[0].Count);
			Assert.AreEqual(
				(a.Consequences[0][0] as Say).EvaluateText(rumor),
				(b.Consequences[0][0] as Say).EvaluateText(rumor));
		}

		#endregion

		#region Utility

		/// <summary>
		/// Serializes and then deserializes the passed object.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the object.
		/// </typeparam>
		/// <param name="o">
		/// The object to re-serialize.
		/// </param>
		/// <returns>
		/// The object after it has been re-serialized.
		/// </returns>
		private static T Reserialize<T>(T o)
		{
			return Deserialize<T>(Serialize(o));
		}

		private static MemoryStream Serialize<T>(T o)
		{
			MemoryStream stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, o);
			return stream;
		}

		private static T Deserialize<T>(MemoryStream stream)
		{
			IFormatter formatter = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);
			object rumor = formatter.Deserialize(stream);
			return (T)rumor;
		}

		#endregion
	}
}

#endif
