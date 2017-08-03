﻿using Exodrifter.Rumor.Engine;
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
	public class FunctionExpression : Expression
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

		public override Value Evaluate(Engine.Rumor rumor)
		{
			return Invoke(null, rumor);
		}

		public Value Invoke
			(object self, Engine.Rumor rumor, Expression inject = null)
		{
			var p = new List<Expression>(@params);
			if (inject != null) {
				p.Insert(0, inject);
			}

			var values = new object[p.Count];

			for (int i = 0; i < p.Count; ++i) {
				var value = p[i].Evaluate(rumor);

				if (value == null) {
					values[i] = null;
				}
				else {
					values[i] = value.AsObject();
				}
			}

			if (self == null) {
				var result = rumor.CallBinding(name, values);
				return Value.Covert(result);
			}
			else {
				var method = self.GetType().GetMethod(name);
				if (method == null) {
					throw new InvalidOperationException(string.Format(
						"\"{0}\" does not have function \"{1}\"",
						self.GetType(), name));
				}
				var result = method.Invoke(self, values);
				return Value.Covert(result);
			}
		}

		public override string ToString()
		{
			return name + "()";
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
			: base (info, context)
		{
			name = info.GetValue<string>("name");
			@params = info.GetValue<List<Expression>>("params");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("name", name);
			info.AddValue<List<Expression>>("params", @params);
		}

		#endregion
	}
}
