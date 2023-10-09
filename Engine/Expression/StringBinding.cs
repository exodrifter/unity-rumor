using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class StringBinding : Expression, ISerializable
	{
		private readonly string id;
		private Expression[] param;

		public StringBinding(string id, params Expression[] param)
		{
			this.id = id;
			this.param = param;
		}

		public override Value Evaluate(RumorScope scope, RumorBindings bindings)
		{
			object[] ps = new object[param.Length];
			for (int i = 0; i < param.Length; ++i)
			{
				ps[i] = param[i].Evaluate(scope, bindings).InternalValue;
			}

			var value = bindings.CallBinding(BindingType.Function, id, ps);
			if (value == null)
			{
				throw new VariableTypeException(
					"Function \"" + id + "\" returned null!"
				);
			}
			if (!(value is string))
			{
				throw new VariableTypeException(
					"Function \"" + id + "\" did not return a string!"
				);
			}

			return new StringValue((string)value);
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as StringBinding);
		}

		public bool Equals(StringBinding other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(id, other.id);
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(id);
		}

		#endregion

		#region Serialization

		public StringBinding
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			id = info.GetValue<string>("id");
			param = info.GetValue<Expression[]>("param");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("id", id);
			info.AddValue<Expression[]>("param", param);
		}

		#endregion

		public override string ToString()
		{
			string[] ps = new string[param.Length];
			for (int i = 0; i < param.Length; ++i)
			{
				ps[i] = param[i].ToString();
			}

			return id + "(" + string.Join(", ", ps) + ")";
		}
	}
}
