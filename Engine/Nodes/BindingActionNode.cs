using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class BindingActionNode : Node, ISerializable
	{
		private readonly string id;
		private Expression[] param;

		public BindingActionNode(string id, params Expression[] param)
		{
			this.id = id;
			this.param = param;
		}

		public override Yield Execute(Rumor rumor)
		{
			object[] ps = new object[param.Length];
			for (int i = 0; i < param.Length; ++i)
			{
				ps[i] = param[i].Evaluate(rumor.Scope).InternalValue;
			}

			rumor.Bindings.CallBinding(BindingType.Action, id, ps);
			return null;
		}

		#region Equality

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

		#endregion

		#region Serialization

		public BindingActionNode
			(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			id = info.GetValue<string>("id");
			param = info.GetValue<Expression[]>("param");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("id", id);
			info.AddValue<Expression[]>("param", param);
		}

		#endregion

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
