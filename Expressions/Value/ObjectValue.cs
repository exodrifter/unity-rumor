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
			if (AsObject() == null) {
				return new BoolValue(true);
			}
			return new BoolValue(false);
		}

		public override Value Add(Value value)
		{
			if (AsObject() == null) {
				if (value == null) {
					return new ObjectValue(null);
				}
				if (value.IsInt()) {
					return new IntValue(value.AsInt());
				}
				if (value.IsFloat()) {
					return new FloatValue(value.AsFloat());
				}
				if (value.IsObject() && value.AsObject() == null) {
					return new ObjectValue(null);
				}
			}
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
			if (AsObject() == null) {
				if (value == null) {
					return new ObjectValue(null);
				}
				if (value.IsInt()) {
					return new IntValue(0 - value.AsInt());
				}
				if (value.IsFloat()) {
					return new FloatValue(0 - value.AsFloat());
				}
				if (value.IsObject() && value.AsObject() == null) {
					return new ObjectValue(null);
				}
			}
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			if (AsObject() == null) {
				if (value == null) {
					return new ObjectValue(null);
				}
				if (value.IsInt()) {
					return new IntValue(0);
				}
				if (value.IsFloat()) {
					return new FloatValue(0);
				}
				if (value.IsObject() && value.AsObject() == null) {
					return new ObjectValue(null);
				}
			}
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			if (AsObject() == null) {
				if (value == null) {
					return new ObjectValue(null);
				}
				if (value.IsInt()) {
					return new IntValue(0);
				}
				if (value.IsFloat()) {
					return new FloatValue(0);
				}
				if (value.IsObject() && value.AsObject() == null) {
					return new ObjectValue(null);
				}
			}
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
				return new BoolValue(AsObject() == null);
			}
			else if (value.IsObject()) {
				return new BoolValue(AsObject() == value.AsObject());
			}
			return new BoolValue(false);
		}

		public override Value BoolAnd(Value value)
		{
			if (AsObject() == null) {
				return new BoolValue(false);
			}
			else {
				if (value == null) {
					return new BoolValue(false);
				}
				if (value.IsBool()) {
					return new BoolValue(value.AsBool());
				}
				else if (value.IsInt()) {
					return new BoolValue(value.AsInt() != 0);
				}
				else if (value.IsFloat()) {
					return new BoolValue(value.AsFloat() != 0);
				}
				else if (value.IsString()) {
					return new BoolValue(value.AsString() != "");
				}
				else if (value.IsObject()) {
					return new BoolValue(value.AsObject() != null);
				}
			}
			throw new InvalidOperationException();
		}

		public override Value BoolOr(Value value)
		{
			if (AsObject() == null) {
				if (value == null) {
					return new BoolValue(false);
				}
				if (value.IsBool()) {
					return new BoolValue(value.AsBool());
				}
				else if (value.IsInt()) {
					return new BoolValue(value.AsInt() != 0);
				}
				else if (value.IsFloat()) {
					return new BoolValue(value.AsFloat() != 0);
				}
				else if (value.IsString()) {
					return new BoolValue(value.AsString() != "");
				}
				else if (value.IsObject()) {
					return new BoolValue(value.AsObject() != null);
				}
			}
			else {
				return new BoolValue(true);
			}
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