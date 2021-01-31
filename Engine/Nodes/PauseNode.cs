using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class PauseNode : Node
	{
		public Expression Time { get; }

		public PauseNode(double seconds)
		{
			Time = new NumberLiteral(seconds);
		}

		public PauseNode(Expression time)
		{
			Time = time;
		}

		public override Yield Execute(Rumor rumor)
		{
			var seconds = Time.Evaluate(rumor.Scope).AsNumber().Value;
			return new ForSeconds(seconds);
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
