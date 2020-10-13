namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		public static Parser<U> Then<T,U>(this Parser<T> first, Parser<U> second)
		{
			return state =>
			{
				var result = first.DoParse(state);
				return second.DoParse(result.NextState);
			};
		}
	}
}
