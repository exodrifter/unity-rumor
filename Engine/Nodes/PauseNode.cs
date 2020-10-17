using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class PauseNode : Node
	{
		public Expression<NumberValue> Time { get; }

		public PauseNode(double seconds)
		{
			Time = new NumberLiteral(seconds);
		}

		public PauseNode(Expression<NumberValue> time)
		{
			Time = time;
		}

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			var seconds = Time.Evaluate().Value;
			yield return new ForSeconds(seconds);
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
