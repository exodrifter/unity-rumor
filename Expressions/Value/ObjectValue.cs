using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class ObjectValue : Value
	{
		public ObjectValue(object value)
			: base(value)
		{
		}

		public override Value Not()
		{
			throw new InvalidOperationException();
		}

		public override Value Add(Value value)
		{
			if (value == null) {
				throw new InvalidOperationException();
			}
			if (value.IsString()) {
				return new StringValue(AsObject() + value.AsString());
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

		public override Value BoolAnd(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value BoolOr(Value value)
		{
			throw new InvalidOperationException();
		}

		#region Serialization

		public ObjectValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}