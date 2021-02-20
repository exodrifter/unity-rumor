using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public abstract class LiteralExpression : Expression, ISerializable
	{
		public readonly Value Value;

		public LiteralExpression(Value value)
		{
			Value = value;
		}

		public override Value Evaluate(RumorScope _)
		{
			return Value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as LiteralExpression);
		}

		public bool Equals(LiteralExpression other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Value);
		}

		#endregion

		#region Serialization

		public LiteralExpression
			(SerializationInfo info, StreamingContext context)
		{
			Value = info.GetValue<Value>("value");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Value>("value", Value);
		}

		#endregion

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
