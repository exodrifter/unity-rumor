using System;

public class ParserException : Exception
{
	public int Index { get; }
	public string[] Expected { get; }

	public ParserException(int index, params string[] expected)
	{
		Index = index;
		Expected = expected;
	}
}
