using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	public class BindingActionNode : Node
	{
		private readonly string id;
		private Expression[] param;

		public BindingActionNode(string id, params Expression[] param)
		{
			this.id = id;
			this.param = param;
		}

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			object[] ps = new object[param.Length];
			for (int i = 0; i < param.Length; ++i)
			{
				ps[i] = param[i].Evaluate(rumor.Scope).InternalValue;
			}

			rumor.Bindings.CallBinding(BindingType.Action, id, ps);
			yield break;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as BindingActionNode);
		}

		public bool Equals(BindingActionNode other)
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
