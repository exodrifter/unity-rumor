using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class StringValue : Value, ISerializable
	{
		public string Value { get { return (string)InternalValue; } }
		public override ValueType Type => ValueType.String;

		public StringValue(string str) : base(str) { }

		public static StringValue operator +(StringValue l, StringValue r) =>
			new StringValue(l.Value + r.Value);

		#region Serialization

		public StringValue(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
