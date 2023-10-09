using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class ControlNode : Node, ISerializable
	{
		private Expression Condition { get; }

		/// <summary>
		/// The label that will be pushed onto the stack if the condition for
		/// this node is met.
		/// </summary>
		private string Label { get; }

		private ControlNode Next { get; }

		public ControlNode(Expression condition, string label, ControlNode next)
		{
			Condition = condition;
			Label = label;
			Next = next;
		}

		public override Yield Execute(Rumor rumor)
		{
			bool call;
			try
			{
				// If there is no condition, this is an else and we will always
				// want to call
				if (Condition == null)
				{
					call = true;
				}
				else
				{
					var value = Condition.Evaluate(rumor.Scope, rumor.Bindings)?.AsBoolean();
					call = value?.Value ?? false;
				}
			}
			catch (UndefinedVariableException)
			{
				call = false;
			}
			catch (VariableTypeException)
			{
				call = false;
			}

			if (call)
			{
				rumor.Call(Label);
			}
			else
			{
				Next?.Execute(rumor);
			}

			return null;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as ControlNode);
		}

		public bool Equals(ControlNode other)
		{
			if (other == null)
			{
				return false;
			}

			return (Condition?.Equals(other.Condition) ?? true)
				&& Label.SequenceEqual(other.Label)
				&& (Next?.Equals(other.Next) ?? true);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region Serialization

		public ControlNode(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Condition = info.GetValue<Expression>("condition");
			Label = info.GetValue<string>("label");
			Next = info.GetValue<ControlNode>("next");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Expression>("condition", Condition);
			info.AddValue<string>("label", Label);
			info.AddValue<ControlNode>("next", Next);
		}

		#endregion

		public override string ToString()
		{
			var lines = new List<string>();
			foreach (var node in Label)
			{
				lines.Add(node.ToString());
			}

			if (Next != null)
			{
				lines.Add(Next.ToString_Internal());
			}

			return "if {" + Condition + "}; call " + Label + ";"
				+ Next.ToString_Internal();
		}

		private string ToString_Internal()
		{
			if (Condition != null && Next != null)
			{
				return "elif {" + Condition + "}; call " + Label + ";"
					+ Next.ToString_Internal();
			}
			else
			{
				return "else; call " + Label;
			}
		}
	}
}
