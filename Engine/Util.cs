using System;

namespace Exodrifter.Rumor.Engine
{
	internal class Util
	{
		public static bool ArrayEquals<T>(T[] l, T[] r)
		{
			if (l == null && r == null)
			{
				return true;
			}
			if (l == null || r == null)
			{
				return false;
			}

			if (l.Length != r.Length)
			{
				return false;
			}

			for (int i = 0; i < l.Length; ++i)
			{
				if (l[i] == null && r[i] == null) {
					continue;
				}
				else if (l[i] == null || r[i] == null) {
					return false;
				}
				else if (!l[i].Equals(r[i])) {
					return false;
				}
			}

			return true;
		}

		public static int GetHashCode(params object[] args)
		{
			unchecked
			{
				int hash = 17;
				foreach (var arg in args)
				{
					hash *= 23;
					if (arg != null)
					{
						hash += arg.GetHashCode();
					}
				}
				return hash;
			}
		}
	}
}
