namespace Exodrifter.Rumor.Engine
{
	internal class Util
	{
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
