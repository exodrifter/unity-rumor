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
			var a = new Add("add");
			var b = Reserialize(a);

			Assert.AreEqual(a.text, b.text);
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
			var a = new Choice("choice", new List<Node>() {
				new Say("say")
			});
			var b = Reserialize(a);

			Assert.AreEqual(a.text, b.text);
			Assert.AreEqual(1, a.Children.Count);
			Assert.AreEqual(a.Children.Count, b.Children.Count);
			Assert.AreEqual(
				(a.Children[0] as Say).text,
				(b.Children[0] as Say).text);
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
			var a = new Label("label", new List<Node>() {
				new Say("say")
			});
			var b = Reserialize(a);

			Assert.AreEqual(a.name, b.name);
			Assert.AreEqual(1, a.Children.Count);
			Assert.AreEqual(a.Children.Count, b.Children.Count);
			Assert.AreEqual(
				(a.Children[0] as Say).text,
				(b.Children[0] as Say).text);
		}

		/// <summary>
		/// Ensure Pause nodes are serialized properly.
		/// </summary>
		[Test]
		public void SerializePause()
		{
			var a = new Pause(1);
			var b = Reserialize(a);

			Assert.AreEqual(a.seconds, b.seconds);
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
			var a = new Say("say");
			var b = Reserialize(a);

			Assert.AreEqual(a.text, b.text);
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

			rumor.Run().MoveNext();
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
			var b = Reserialize(a);

			Assert.AreEqual(a.Scope.GetVar("a"), b.Scope.GetVar("a"));
			Assert.AreEqual(a.Scope.GetVar("b"), b.Scope.GetVar("b"));
			Assert.AreEqual(a.Scope.GetVar("c"), b.Scope.GetVar("c"));
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

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("Hello!", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("How are you?", (rumor.Current as Say).text);

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

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("Hello!", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("How are you?", (rumor.Current as Say).text);

			// Check to see if serializing during execution works
			rumor = Reserialize(rumor);

			Assert.AreEqual("How are you?", rumor.State.Dialog);

			yield = rumor.Run();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("Good.", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("How about yourself?", (rumor.Current as Say).text);

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

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("a!", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("b!", (rumor.Current as Say).text);

			// Check to see if serializing during execution works
			rumor = Reserialize(rumor);

			yield = rumor.Run();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.False(rumor.Finished);
			Assert.AreEqual("c!", (rumor.Current as Say).text);

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
			a.SetDialog("dialog");
			var b = Reserialize(a);

			Assert.AreEqual(a.Dialog, b.Dialog);
		}

		[Test]
		public void SerializeRumorStateChoice()
		{
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
				(a.Consequences[0][0] as Say).text,
				(b.Consequences[0][0] as Say).text);
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
