using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class SetVariableNode : Node, ISerializable
	{
		public string Name { get; }
		public Expression Expression { get; }

		public SetVariableNode(string name, Expression expression)
		{
			Name = name;
			Expression = expression;
		}

		public override Yield Execute(Rumor rumor)
		{
			var value = Expression.Evaluate(rumor.Scope);
			rumor.Scope.Set(Name, value);
			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as SetVariableNode);
		}

		public bool Equals(SetVariableNode other)
		{
			if (other == null)
			{
				return false;
			}

			return Name == other.Name
				&& Expression == other.Expression;
		}

		public override int GetHashCode()
		{
			return Util.GetHashCode(Name, Expression);
		}

		#endregion

		#region Serialization

		public SetVariableNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Name = info.GetValue<string>("name");
			Expression = info.GetValue<Expression>("expression");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("name", Name);
			info.AddValue<Expression>("expression", Expression);
		}

		#endregion

		public override string ToString()
		{
			return Name + " = " + Expression;
		}
	}
}
