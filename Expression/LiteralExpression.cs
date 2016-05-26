using Exodrifter.Rumor.Engine;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a literal.
	/// </summary>
	public class LiteralExpression : Expression, ISerializable
	{
		private readonly object value;

		public LiteralExpression(string str)
		{
			value = str;
		}

		public LiteralExpression(int num)
		{
			value = num;
		}

		public LiteralExpression(float num)
		{
			value = num;
		}

		public override object Evaluate(Scope scope)
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
			value = info.GetValue("value", typeof(object));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(object));
		}

		#endregion
	}
}
