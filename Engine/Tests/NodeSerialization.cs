using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class NodeSerialization
	{
		[Test]
		public static void AddChoiceNode()
		{
			var a = new AddChoiceNode("label", "text");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void AppendDialogNode()
		{
			var a = new AppendDialogNode("speaker", "dialog");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void BindingActionNode()
		{
			var v = new NumberLiteral(2);
			var a = new BindingActionNode("fn", new Expression[] { v, v, v });
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void ChooseNode()
		{
			var v = new NumberLiteral(2);
			var a = new ChooseNode(v, MoveType.Jump, "foobar");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void ClearNode()
		{
			var a = new ClearNode(ClearType.Dialog, null);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void ControlNode()
		{
			var a = new ControlNode(
				new BooleanLiteral(false),
				"foo",
				new ControlNode(
					new BooleanLiteral(true),
					"bar",
					null
				)
			);
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void JumpNode()
		{
			var a = new JumpNode("label");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void PauseNode()
		{
			var a = new PauseNode(new NumberLiteral(2));
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void ReturnNode()
		{
			var a = new ReturnNode();
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SetDialogNode()
		{
			var a = new SetDialogNode("speaker", "dialog");
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void SetVariableNode()
		{
			var a = new SetVariableNode("foobar", new NumberLiteral(2));
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}

		[Test]
		public static void WaitNode()
		{
			var a = new WaitNode();
			var b = SerializationUtil.Reserialize(a);

			Assert.AreEqual(a, b);
		}
	}
}
