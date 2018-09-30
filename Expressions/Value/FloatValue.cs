using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class FloatValue : Value, ISerializable
	{
		public FloatValue(float value)
			: base(value)
		{
		}

		public override Value Not()
		{
			return new BoolValue(!(AsFloat() != 0f));
		}

		public override Value Add(Value value)
		{
			if (value == null) {
				return new FloatValue(AsFloat());
			}
			if (value.IsObject() && value.AsObject() == null) {
				return new FloatValue(AsFloat());
			}
			if (value.IsInt()) {
				return new FloatValue(AsFloat() + value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() + value.AsFloat());
			}
			if (value.IsString()) {
				return new StringValue(AsFloat() + value.AsString());
			}
			throw new InvalidOperationException();
		}

		public override Value Subtract(Value value)
		{
			if (value == null) {
				return new FloatValue(AsFloat());
			}
			if (value.IsObject() && value.AsObject() == null) {
				return new FloatValue(AsFloat());
			}
			if (value.IsInt()) {
				return new FloatValue(AsFloat() - value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() - value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			if (value == null) {
				return new FloatValue(0);
			}
			if (value.IsObject() && value.AsObject() == null) {
				return new FloatValue(0);
			}
			if (value.IsInt()) {
				return new FloatValue(AsFloat() * value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() * value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			if (value == null) {
				return new FloatValue(0);
			}
			if (value.IsObject() && value.AsObject() == null) {
				return new FloatValue(0);
			}
			if (value.IsInt()) {
				return new FloatValue(AsFloat() / value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsFloat() / value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value LessThan(Value value)
		{
			if (value == null) {
				throw new InvalidOperationException();
			}
			if (value.IsInt()) {
				return new BoolValue(AsFloat() < value.AsInt());
			}
			if (value.IsFloat()) {
				return new BoolValue(AsFloat() < value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value GreaterThan(Value value)
		{
			if (value == null) {
				throw new InvalidOperationException();
			}
			if (value.IsInt()) {
				return new BoolValue(AsFloat() > value.AsInt());
			}
			if (value.IsFloat()) {
				return new BoolValue(AsFloat() > value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value EqualTo(Value value)
		{
			if (value == null) {
				return new BoolValue(false);
			}
			if (value.IsInt()) {
				return new BoolValue(AsFloat() == value.AsInt());
			}
			if (value.IsFloat()) {
				return new BoolValue(AsFloat() == value.AsFloat());
			}
			return new BoolValue(false);
		}

		public override Value BoolAnd(Value value)
		{
			if (value == null) {
				return new BoolValue(false);
			}
			if (value.IsObject()) {
				return new BoolValue(AsFloat() != 0 && value.AsObject() != null);
			}
			if (value.IsInt()) {
				return new BoolValue(AsFloat() != 0 && value.AsInt() != 0);
			}
			if (value.IsFloat()) {
				return new BoolValue(AsFloat() != 0 && value.AsFloat() != 0);
			}
			if (value.IsString()) {
				return new BoolValue(AsFloat() != 0 && value.AsString() != "");
			}
			if (value.IsBool()) {
				return new BoolValue(AsFloat() != 0 && value.AsBool());
			}
			throw new InvalidOperationException();
		}

		public override Value BoolOr(Value value)
		{
			if (value == null) {
				return new BoolValue(AsFloat() != 0);
			}
			if (value.IsObject()) {
				return new BoolValue(AsFloat() != 0 || value.AsObject() != null);
			}
			if (value.IsInt()) {
				return new BoolValue(AsFloat() != 0 || value.AsInt() != 0);
			}
			if (value.IsFloat()) {
				return new BoolValue(AsFloat() != 0 || value.AsFloat() != 0);
			}
			if (value.IsString()) {
				return new BoolValue(AsFloat() != 0 || value.AsString() != "");
			}
			if (value.IsBool()) {
				return new BoolValue(AsFloat() != 0 || value.AsBool());
			}
			throw new InvalidOperationException();
		}

		#region Serialization

		public FloatValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}