using System;
using System.Collections.Generic;
using System.Linq;

namespace Exodrifter.Rumor.Engine
{
	public class ControlNode : Node
	{
		private Expression Condition { get; }

		/// <summary>
		/// The list of blocks that will be pushed onto the stack if the
		/// condition for this node is met.
		/// </summary>
		private List<Node> Block { get; }

		private ControlNode Next { get; }

		public ControlNode(Expression condition, List<Node> block, ControlNode next)
		{
			Condition = condition;
			Block = block;
			Next = next;
		}

		public override IEnumerator<Yield> Execute(Rumor rumor)
		{
			if (Condition.Evaluate(rumor.Scope).AsBoolean().Value)
			{
				rumor.Inject(Block);
			}
			else
			{
				Next?.Execute(rumor);
			}
			yield break;
		}

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
				&& Block.SequenceEqual(other.Block)
				&& (Next?.Equals(other.Next) ?? true);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			var lines = new List<string>();
			foreach (var node in Block)
			{
				lines.Add(node.ToString());
			}

			if (Next != null)
			{
				lines.Add(Next.ToString_Internal());
			}

			return "if {" + Condition + "}; " + string.Join("; ", lines);
		}

		private string ToString_Internal()
		{
			var lines = new List<string>();
			foreach (var node in Block)
			{
				lines.Add(node.ToString());
			}

			if (Condition != null && Next != null)
			{
				lines.Add(Next.ToString());
				return "elif {" + Condition + "}; " + string.Join("; ", lines);
			}
			else
			{
				return "else " + string.Join("; ", lines);
			}
		}
	}
}
