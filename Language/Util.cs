using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exodrifter.Rumor.Language
{
	public static class Util
	{

		/// <summary>
		/// Returns a character array as a string array.
		/// </summary>
		public static string[] AsStringArray(this char[] arr)
		{
			var str = new string[arr.Length];

			for (int i = 0; i < arr.Length; ++i)
			{
				str[i] = arr[i].ToString();
			}

			return str;
		}

		/// <summary>
		/// Returns true if the array contains the specified character.
		/// </summary>
		public static bool Contains(this char[] arr, char ch)
		{
			foreach (var other in arr)
			{
				if (other == ch)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the string contains the specified character.
		/// </summary>
		public static bool Contains(this string str, char ch)
		{
			return str.ToCharArray().Contains(ch);
		}

		/// <summary>
		/// Performs a python-esque array slice operation.
		/// </summary>
		public static List<T> Slice<T>(this List<T> list,
			int? begin = null, int? end = null)
		{
			begin = begin ?? 0;
			end = end ?? list.Count;
			while (begin < 0) {
				begin += list.Count;
			}
			while (end < 0) {
				end += list.Count;
			}

			var ret = new List<T>();
			for (int i = begin.Value; i < end.Value; ++i) {
				ret.Add(list[i]);
			}

			return ret;
		}
	}
}
