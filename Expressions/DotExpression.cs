using Exodrifter.Rumor.Engine;
using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents a dot operator which is used to access member variables and
	/// functions.
	/// </summary>
	[Serializable]
	public class DotExpression : OpExpression
	{
		public DotExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override Value Evaluate(Scope scope, Bindings bindings)
		{
			var l = left.Evaluate(scope, bindings);

			if (l == null || l.AsObject() == null) {
				throw new NullReferenceException(
					"Left argument to dot operator is null");
			}
			if (right == null) {
				throw new NullReferenceException(
					"Right argument to dot operator is null");
			}

			if (right is FunctionExpression) {
				var fn = (FunctionExpression)right;
				return fn.Invoke(l.AsObject(), scope, bindings);
			}
			if (right is VariableExpression) {
				var v = (VariableExpression)right;
				var obj = l.AsObject();
				var type = obj.GetType();

				if (type.GetProperty(v.Name) != null) {
					var prop = type.GetProperty(v.Name);
					return Value.Covert(prop.GetValue(obj, null));
				}
				if (type.GetField(v.Name) != null) {
					var field = type.GetField(v.Name);
					return Value.Covert(field.GetValue(obj));
				}

				throw new InvalidOperationException(string.Format(
					"\"{0}\" does not have member \"{1}\"", type, v.Name));
			}

			throw new InvalidOperationException(
				"Right argument to dot Operator cannot be used; it is of "
				+ "type " + right.GetType());
		}

		public override string ToString()
		{
			return left + "." + right;
		}

		#region Serialization

		public DotExpression(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion
	}
}
