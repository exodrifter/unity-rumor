using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exodrifter.Rumor.Engine.Tests
{
	public class SerializationUtil
	{
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
		public static T Reserialize<T>(T o)
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
	}
}
