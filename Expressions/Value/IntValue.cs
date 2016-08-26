﻿using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	[Serializable]
	public class IntValue : Value
	{
		public IntValue(int value)
			: base(value)
		{
		}

		public override Value Not()
		{
			return new BoolValue(!(AsInt() != 0));
		}

		public override Value Add(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() + value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() + value.AsFloat());
			}
			if (value.IsString()) {
				return new StringValue(AsInt() + value.AsString());
			}
			throw new InvalidOperationException();
		}

		public override Value Subtract(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() - value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() - value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Multiply(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() * value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() * value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value Divide(Value value)
		{
			if (value.IsInt()) {
				return new IntValue(AsInt() / value.AsInt());
			}
			if (value.IsFloat()) {
				return new FloatValue(AsInt() / value.AsFloat());
			}
			throw new InvalidOperationException();
		}

		public override Value BoolAnd(Value value)
		{
			if (value.IsInt()) {
				return new BoolValue(AsInt() != 0 && value.AsInt() != 0);
			}
			if (value.IsFloat()) {
				return new BoolValue(AsInt() != 0 && value.AsFloat() != 0);
			}
			if (value.IsString()) {
				return new BoolValue(AsInt() != 0 && value.AsString() != "");
			}
			if (value.IsBool()) {
				return new BoolValue(AsInt() != 0 && value.AsBool());
			}
			throw new InvalidOperationException();
		}

		public override Value BoolOr(Value value)
		{
			if (value.IsInt()) {
				return new BoolValue(AsInt() != 0 || value.AsInt() != 0);
			}
			if (value.IsFloat()) {
				return new BoolValue(AsInt() != 0 || value.AsFloat() != 0);
			}
			if (value.IsString()) {
				return new BoolValue(AsInt() != 0 || value.AsString() != "");
			}
			if (value.IsBool()) {
				return new BoolValue(AsInt() != 0 || value.AsBool());
			}
			throw new InvalidOperationException();
		}

		#region Serialization

		public IntValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}