using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class NumberValue : Value, ISerializable
	{
		public double Value { get { return (double)InternalValue; } }
		public override ValueType Type => ValueType.Number;

		public NumberValue(double n) : base(n) { }

		public static NumberValue operator +(NumberValue n) => n;
		public static NumberValue operator -(NumberValue n) =>
			new NumberValue(-n.Value);

		public static NumberValue operator +(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value + r.Value);
		public static NumberValue operator -(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value - r.Value);
		public static NumberValue operator *(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value * r.Value);
		public static NumberValue operator /(NumberValue l, NumberValue r) =>
			new NumberValue(l.Value / r.Value);

		public static bool operator >(NumberValue l, NumberValue r) =>
			l.Value > r.Value;
		public static bool operator <(NumberValue l, NumberValue r) =>
			l.Value < r.Value;
		public static bool operator >=(NumberValue l, NumberValue r) =>
			l.Value >= r.Value;
		public static bool operator <=(NumberValue l, NumberValue r) =>
			l.Value <= r.Value;

		#region Serialization

		public NumberValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
