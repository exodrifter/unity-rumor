namespace Exodrifter.Rumor.Engine
{
	public class PauseNode
	{
		public Expression<NumberValue> Time { get; }

		public PauseNode(double time)
		{
			Time = new NumberLiteral(time);
		}

		public PauseNode(Expression<NumberValue> time)
		{
			Time = time;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as PauseNode);
		}

		public bool Equals(PauseNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Time == other.Time;
		}

		public override int GetHashCode()
		{
			return Time.GetHashCode();
		}

		public override string ToString()
		{
			return "pause " + Time;
		}
	}
}
