using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class PauseNode : Node, ISerializable
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

		#region Equality

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

		#endregion

		#region Serialization

		public PauseNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Time = info.GetValue<Expression>("time");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("time", Time);
		}

		#endregion

		public override string ToString()
		{
			return "pause " + Time;
		}
	}
}
