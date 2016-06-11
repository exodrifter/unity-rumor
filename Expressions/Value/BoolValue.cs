using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class BoolValue : Value, ISerializable
	{
		public BoolValue(bool value)
			: base(value)
		{
		}

		public override Value Add(Value value)
		{
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

		public BoolValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}