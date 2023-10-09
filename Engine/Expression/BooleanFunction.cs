using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class BooleanFunction : FunctionExpression, ISerializable
	{
		private readonly string name;
		private readonly Expression[] args;

		public BooleanFunction(string name, params Expression[] args)
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
			var argArr = evaluatedArgs.ToArray();

			var result = bindings.CallBinding(BindingType.Function, name, argArr);
			try {
				return new BooleanValue(Convert.ToBoolean(result));
			} catch (Exception) {
				throw new FunctionTypeException(
					"Function \"" + name + "\" did not return a boolean!"
				);
			}
		}

		public override Expression Simplify()
		{
			var simplifiedArgs = new List<Expression>(args.Length);
			foreach (var arg in args) {
				simplifiedArgs.Add(arg.Simplify());
			}
			return new BooleanFunction(name, simplifiedArgs.ToArray());
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as BooleanFunction);
		}

		public bool Equals(BooleanFunction other)
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

		public BooleanFunction
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
			var argStrings = new List<string>();
			foreach (var arg in args) {
				argStrings.Add(arg.ToString());
			}
			return name + "<boolean>(" + string.Join(",", argStrings) + ")";
		}
	}
}
