#if UNITY_EDITOR

using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Nodes
{
	/// <summary>
	/// Ensure Label nodes operate as expected.
	/// </summary>
	public class LabelTest
	{
		/// <summary>
		/// Ensure labels automatically enter their block.
		/// </summary>
		[Test]
		public void LabelEnter()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("a"),
				}),
				new Say("b"),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure labels automatically exit their block.
		/// </summary>
		[Test]
		public void LabelExit()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("a"),
				}),
				new Label("b", new List<Node>() {
					new Say("b"),
				}),
				new Say("c"),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("b", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("c", (rumor.Current as Say).EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure labels properly exit their block when they are nested.
		/// </summary>
		[Test]
		public void LabelExitNested()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Label("b", new List<Node>() {
						new Label("c", new List<Node>() {
							new Say("1"),
						}),
					}),
				}),
				new Say("2"),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("1", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("2", (rumor.Current as Say).EvaluateText(rumor));
		}
	}
}

#endif
