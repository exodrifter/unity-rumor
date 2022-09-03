using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class StringFunction : Expression, ISerializable
	{
		private readonly string name;
		private readonly Expression[] args;

		public StringFunction(string name, params Expression[] args)
		{
			this.name = name;
			this.args = args;
		}

		public override Value Evaluate(RumorScope scope, RumorBindings bindings)
		{
			var evaluatedArgs = new List<object>(args.Length);
			foreach (var arg in args) {
				evaluatedArgs.Add(arg.Evaluate(scope, bindings).InternalValue);
			}

			var result = bindings.CallBinding(BindingType.Function, name, evaluatedArgs);
			var value = result as StringValue;
			if (value == null)
			{
				throw new FunctionTypeException(
					"Variable is not a string!"
				);
			}

			return value;
		}

		public override Expression Simplify()
		{
			var simplifiedArgs = new List<Expression>(args.Length);
			foreach (var arg in args) {
				simplifiedArgs.Add(arg.Simplify());
			}
			return new StringFunction(name, simplifiedArgs.ToArray());
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as StringFunction);
		}

		public bool Equals(StringFunction other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(name, other.name);
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		#endregion

		#region Serialization

		public StringFunction
			(SerializationInfo info, StreamingContext context)
		{
			name = info.GetValue<string>("name");
			args = info.GetValue<Expression[]>("args");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("name", name);
			info.AddValue<Expression[]>("args", args);
		}

		#endregion

		public override string ToString()
		{
			return name;
		}
	}
}
