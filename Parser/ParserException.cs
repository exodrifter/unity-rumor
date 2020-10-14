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

	public override string ToString()
	{
		return "Parser Exception at index " + Index
			+ ": expected " + string.Join(", ", Expected);
	}
}
