using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Util
{
	/// <summary>
	/// A wrapper around a dictionary that can return a result for any key even
	/// if the key doesn't exist and can store a key value pair for a null key.
	/// </summary>
	[Serializable]
	public class LazyDictionary<K, V> : ISerializable
		where K : class where V : class
	{
		private Dictionary<K, V> dictionary;

		private V nullValue;

		public List<K> Keys
		{
			get
			{
				var list = new List<K>(dictionary.Keys);
				if (nullValue != null) {
					list.Add(null);
				}
				return list;
			}
		}

		public List<V> Values
		{
			get
			{
				var list = new List<V>(dictionary.Values);
				if (nullValue != null) {
					list.Add(nullValue);
				}
				return list;
			}
		}

		public V this[K key]
		{
			get
			{
				if (key == null) {
					return nullValue;
				}
				if (dictionary.ContainsKey(key)) {
					return dictionary[key];
				}
				return default(V);
			}
			set
			{
				if (key == null) {
					nullValue = value;
				}
				else {
					dictionary[key] = value;
				}
			}
		}

		public LazyDictionary()
		{
			dictionary = new Dictionary<K, V>();
			nullValue = default(V);
		}

		public void Clear()
		{
			dictionary.Clear();
			nullValue = default(V);
		}

		#region Serialization

		public LazyDictionary(SerializationInfo info, StreamingContext context)
		{
			dictionary = info.GetValue<Dictionary<K, V>>("dictionary");
			nullValue = info.GetValue<V>("nullValue");
		}

		public void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Dictionary<K, V>>("dictionary", dictionary);
			info.AddValue<V>("nullValue", nullValue);
		}

		#endregion
	}
}