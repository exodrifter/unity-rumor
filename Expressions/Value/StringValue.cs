using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class StringValue : Value, ISerializable
	{
		public StringValue(string value)
			: base(value)
		{
		}

		public override Value Add(Value value)
		{
			if (value.IsInt()) {
				return new StringValue(AsString() + value.AsInt());
			}
			if (value.IsFloat()) {
				return new StringValue(AsString() + value.AsFloat());
			}
			if (value.IsString()) {
				return new StringValue(AsString() + value.AsString());
			}
			throw new InvalidOperationException();
		}

		public override Value Subtract(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			throw new InvalidOperationException();
		}

		#region Serialization

		public StringValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}