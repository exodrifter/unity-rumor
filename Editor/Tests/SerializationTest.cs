using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exodrifter.Rumor.Test
{
	/// <summary>
	/// Makes sure that Rumors can be serialized and deserialized properly.
	/// </summary>
	public class SerializationTest
	{
		/// <summary>
		/// Makes sure serializing empty Rumors works as expected.
		/// </summary>
		[Test]
		public void SerializeEmpty()
		{
			var rumor = new Engine.Rumor(new List<Node>());

			var a = Serialize(rumor);
			var b = Deserialize(a);

			Assert.False(b.Started);
			Assert.False(b.Finished);

			b.Run().MoveNext();
			Assert.True(b.Started);
			Assert.True(b.Finished);
		}

		/// <summary>
		/// Makes sure serializing simple Rumors that only have a root block
		/// works as expected.
		/// </summary>
		[Test]
		public void SerializeSimple()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Say("Hello!"),
				new Say("How are you?"),
			});

			var a = Serialize(rumor);
			var b = Deserialize(a);

			Assert.False(b.Started);
			Assert.False(b.Finished);

			var yield = b.Run();
			yield.MoveNext();
			Assert.True(b.Started);
			Assert.False(b.Finished);
			Assert.AreEqual("Hello!", (b.Current as Say).text);

			b.Advance();
			yield.MoveNext();
			Assert.AreEqual("How are you?", (b.Current as Say).text);

			b.Advance();
			yield.MoveNext();
			Assert.True(b.Started);
			Assert.True(b.Finished);
		}

		/// <summary>
		/// Makes sure serializing Rumors that have multiple blocks works as
		/// expected.
		/// </summary>
		[Test]
		public void SerializeNormal()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("Hello!"),
					new Say("How are you?"),
				}),
				new Label("b", new List<Node>() {
					new Say("Good."),
					new Say("How about yourself?"),
				}),
			});

			var a = Serialize(rumor);
			var b = Deserialize(a);

			Assert.False(b.Started);
			Assert.False(b.Finished);

			var yield = b.Run();
			yield.MoveNext();
			Assert.True(b.Started);
			Assert.False(b.Finished);
			Assert.AreEqual("Hello!", (b.Current as Say).text);

			b.Advance();
			yield.MoveNext();
			Assert.AreEqual("How are you?", (b.Current as Say).text);

			// Check to see if serializing during execution works
			a = Serialize(b);
			b = Deserialize(a);

			Assert.AreEqual("How are you?", b.State.Dialog);

			yield = b.Run();
			yield.MoveNext();
			Assert.True(b.Started);
			Assert.False(b.Finished);
			Assert.AreEqual("Good.", (b.Current as Say).text);

			b.Advance();
			yield.MoveNext();
			Assert.AreEqual("How about yourself?", (b.Current as Say).text);

			b.Advance();
			yield.MoveNext();
			Assert.True(b.Started);
			Assert.True(b.Finished);
		}

		private static MemoryStream Serialize(Engine.Rumor rumor)
		{
			MemoryStream stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, rumor);
			return stream;
		}

		private static Engine.Rumor Deserialize(MemoryStream stream)
		{
			IFormatter formatter = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);
			object rumor = formatter.Deserialize(stream);
			return (Engine.Rumor)rumor;
		}
	}
}
