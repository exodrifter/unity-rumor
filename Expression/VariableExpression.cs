using Exodrifter.Rumor.Engine;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Expressions
{
	/// <summary>
	/// Represents an expression that is a variable.
	/// </summary>
	public class VariableExpression : Expression, ISerializable
	{
		/// <summary>
		/// The name of the variable
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}
		private readonly string name;

		public VariableExpression(string str)
		{
			name = str;
		}

		public override object Evaluate(Scope scope)
		{
			return scope.GetVar(name);
		}

		public override string ToString()
		{
			return name;
		}

		#region Serialization

		public VariableExpression(SerializationInfo info, StreamingContext context)
		{
			name = (string)info.GetValue("name", typeof(string));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", name, typeof(string));
		}

		#endregion
	}
}
