using Exodrifter.Rumor.Engine;
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
		#region Nodes

		/// <summary>
		/// Ensure Add nodes are serialized properly
		/// </summary>
		[Test]
		public void SerializeAdd()
		{
			var a = new Add("add");
			var b = Reserialize(a);

			Assert.AreEqual(a.text, b.text);
		}

		/// <summary>
		/// Ensure Call nodes are serialized properly
		/// </summary>
		[Test]
		public void SerializeCall()
		{
			var a = new Call("call");
			var b = Reserialize(a);

			Assert.AreEqual(a.to, b.to);
		}

		/// <summary>
		/// Ensure Choice nodes are serialized properly
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
		/// Ensure Jump nodes are serialized properly
		/// </summary>
		[Test]
		public void SerializeJump()
		{
			var a = new Jump("jump");
			var b = Reserialize(a);

			Assert.AreEqual(a.to, b.to);
		}

		/// <summary>
		/// Ensure Label nodes are serialized properly
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
		/// Ensure Pause nodes are serialized properly
		/// </summary>
		[Test]
		public void SerializePause()
		{
			var a = new Pause(1);
			var b = Reserialize(a);

			Assert.AreEqual(a.seconds, b.seconds);
		}

		/// <summary>
		/// Ensure Say nodes are serialized properly
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
		/// Ensure the Rumor state is serialized properly
		/// </summary>
		[Test]
		public void SerializeState()
		{
			var a = new RumorState();
			a.AddChoice("choice", new List<Node>() {
				new Say("say"),
			});
			a.SetDialog("dialog");
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
