using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a variable.
	/// </summary>
	[Serializable]
	public class VariableExpression : Expression
	{
		/// <summary>
		/// The name of the variable
		/// </summary>
		public string Name { get { return name; } }
		private readonly string name;

		public VariableExpression(string str)
		{
			name = str;
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			return scope.GetVar(name);
		}

		public override string ToString()
		{
			return name;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			var other = obj as VariableExpression;
			if (other == null) {
				return false;
			}
			return other.name == name;
		}

		public bool Equals(VariableExpression other)
		{
			if (other == null) {
				return false;
			}
			return other.name == name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		#endregion

		#region Serialization

		public VariableExpression
			(SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
			name = info.GetValue<string>("name");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("name", name);
		}

		#endregion
	}
}
