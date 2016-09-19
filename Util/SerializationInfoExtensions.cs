using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Util
{
	public static class SerializationInfoExtensions
	{
		/// <summary>
		/// Retreives a value from the <see cref="SerializationInfo"/> store.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the value.
		/// </typeparam>
		/// <param name="self">
		/// The <see cref="SerializationInfo"/> to retreive the value from.
		/// </param>
		/// <param name="name">
		/// The name associated with the value to retreive.
		/// </param>
		/// <returns>The value.</returns>
		public static T GetValue<T>(this SerializationInfo self, string name)
		{
			return (T)self.GetValue(name, typeof(T));
		}

		/// <summary>
		/// Adds a value into the <see cref="SerializationInfo"/> store, where
		/// value is associated with name and is serialized as being of
		/// <see cref="System.Type"/> type.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the value.
		/// </typeparam>
		/// <param name="self">
		/// The <see cref="SerializationInfo"/> to add the value to.
		/// </param>
		/// <param name="name">
		/// The name to associate with the value.
		/// </param>
		/// <param name="@object">The value to add.</param>
		public static void AddValue<T>
			(this SerializationInfo self, string name, T @object)
		{
			self.AddValue(name, @object, typeof(T));
		}
	}
}
