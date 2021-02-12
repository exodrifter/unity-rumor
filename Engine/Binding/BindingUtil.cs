namespace Exodrifter.Rumor.Engine
{
	public static class BindingUtil
	{
		/// <summary>
		/// Munges a binding name.
		/// </summary>
		/// <param name="name">The binding name to munge.</param>
		/// <param name="paramCount">
		/// The number of parameters in the binding.
		/// </param>
		/// <returns>The munged name</returns>
		public static string MungeName(BindingType type, string name, int paramCount)
		{
			return string.Format("{0}:{1}@{2}", type, name, paramCount);
		}
	}
}
