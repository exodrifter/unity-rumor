using Exodrifter.Rumor.Engine;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a literal.
	/// </summary>
	public class LiteralExpression : Expression, ISerializable
	{
		private readonly Value value;

		public LiteralExpression(int num)
		{
			value = new IntValue(num);
		}

		public LiteralExpression(float num)
		{
			value = new FloatValue(num);
		}

		public LiteralExpression(string str)
		{
			value = new StringValue(str);
		}

		public override Value Evaluate(Scope scope)
		{
			return value;
		}

		public override string ToString()
		{
			if (value != null)
				return value.ToString();
			return "";
		}

		#region Serialization

		public LiteralExpression(SerializationInfo info, StreamingContext context)
		{
			value = (Value)info.GetValue("value", typeof(Value));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(Value));
		}

		#endregion
	}
}
