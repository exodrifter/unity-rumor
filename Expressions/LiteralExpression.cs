using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a literal.
	/// </summary>
	[Serializable]
	public class LiteralExpression : Expression
	{
		/// <summary>
		/// The value of this literal.
		/// </summary>
		public Value Value { get { return value; } }
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

		public LiteralExpression(bool b)
		{
			value = new BoolValue(b);
		}

		public LiteralExpression(object obj)
		{
			if (obj is int)
			{
				value = new IntValue((int)obj);
			}
			else if (obj is float)
			{
				value = new FloatValue((float)obj);
			}
			else if (obj is string)
			{
				value = new StringValue((string)obj);
			}
			else if (obj is bool)
			{
				value = new BoolValue((bool)obj);
			}
			else
			{
				value = new ObjectValue(obj);
			}
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			return value;
		}

		public override string ToString()
		{
			if (value != null)
				return value.ToString();
			return "<null>";
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as LiteralExpression;
			if (other == null) {
				return false;
			}
			return other.value == value;
		}

		public bool Equals(LiteralExpression other)
		{
			if (other == null) {
				return false;
			}
			return other.value == value;
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		#endregion

		#region Serialization

		public LiteralExpression
			(SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
			value = info.GetValue<Value>("value");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Value>("value", value);
		}

		#endregion
	}
}
