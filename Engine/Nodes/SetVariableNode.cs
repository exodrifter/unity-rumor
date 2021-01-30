using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class SetVariableNode : Node
	{
		public string Name { get; }
		public Expression Expression { get; }

		public SetVariableNode(string name, Expression expression)
		{
			Name = name;
			Expression = expression;
		}

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			var value = Expression.Evaluate(rumor.Scope);
			rumor.Scope.Set(Name, value);
			yield break;
		}

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

		public override string ToString()
		{
			return Name + " = " + Expression;
		}
	}
}
