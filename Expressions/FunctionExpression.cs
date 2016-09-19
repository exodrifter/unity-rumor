using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a call to a function.
	/// </summary>
	[Serializable]
	public class FunctionExpression : Expression, ISerializable
	{
		/// <summary>
		/// The name of the function to call.
		/// </summary>
		public string Name { get { return name; } }
		private readonly string name;

		private readonly List<Expression> @params;

		public FunctionExpression(string name)
		{
			this.name = name;
			this.@params = new List<Expression>();
		}

		public FunctionExpression(string name, IEnumerable<Expression> @params)
		{
			this.name = name;
			this.@params = new List<Expression>(@params);
		}

		public override Value Evaluate(Scope scope)
		{
			var values = new List<object>(@params.Count);

			for (int i = 0; i < @params.Count; ++i) {
				var value = @params[i].Evaluate(scope);
				values.Add(value.AsObject());
			}

			var result = Engine.Rumor.CallBinding(name, values.ToArray());

			// TODO: Return results properly
			return null;
		}

		public override string ToString()
		{
			return name;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as FunctionExpression;
			return Equals(other);
		}

		public bool Equals(FunctionExpression other)
		{
			if (other == null) {
				return false;
			}
			if (@params.Count != other.@params.Count) {
				return false;
			}

			for (int i = 0; i < @params.Count; ++i) {
				if (@params[i] != other.@params[i]) {
					return false;
				}
			}

			return other.name == name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		#endregion

		#region Serialization

		public FunctionExpression
			(SerializationInfo info, StreamingContext context)
		{
			name = info.GetValue<string>("name");
			@params = info.GetValue<List<Expression>>("params");
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("name", name);
			info.AddValue<List<Expression>>("params", @params);
		}

		#endregion
	}
}
