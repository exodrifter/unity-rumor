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

		public override Value Not()
		{
			return new BoolValue(!AsBool());
		}

		public override Value Add(Value value)
		{
			if (value == null) {
				throw new InvalidOperationException();
			}
			if (value.IsString()) {
				var @bool = AsBool().ToString().ToLower();
				return new StringValue(@bool + value.AsString());
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

		public override Value LessThan(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value GreaterThan(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value EqualTo(Value value)
		{
			if (value == null) {
				return new BoolValue(!AsBool());
			}
			if (value.IsBool()) {
				return new BoolValue(AsBool() == value.AsBool());
			}
			return new BoolValue(false);
		}

		public override Value Divide(Value value)
		{
			throw new InvalidOperationException();
		}

		public override Value BoolAnd(Value value)
		{
			if (value == null) {
				return new BoolValue(false);
			}
			if (value.IsObject()) {
				return new BoolValue(AsBool() && value.AsObject() != null);
			}
			if (value.IsInt()) {
				return new BoolValue(AsBool() && value.AsInt() != 0);
			}
			if (value.IsFloat()) {
				return new BoolValue(AsBool() && value.AsFloat() != 0);
			}
			if (value.IsString()) {
				return new BoolValue(AsBool() && value.AsString() != "");
			}
			if (value.IsBool()) {
				return new BoolValue(AsBool() && value.AsBool());
			}
			throw new InvalidOperationException();
		}

		public override Value BoolOr(Value value)
		{
			if (value == null) {
				return new BoolValue(AsBool());
			}
			if (value.IsObject()) {
				return new BoolValue(AsBool() || value.AsObject() != null);
			}
			if (value.IsInt()) {
				return new BoolValue(AsBool() || value.AsInt() != 0);
			}
			if (value.IsFloat()) {
				return new BoolValue(AsBool() || value.AsFloat() != 0);
			}
			if (value.IsString()) {
				return new BoolValue(AsBool() || value.AsString() != "");
			}
			if (value.IsBool()) {
				return new BoolValue(AsBool() || value.AsBool());
			}
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